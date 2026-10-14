'use strict';
// Document-level surgery on an existing Unity scene/prefab text file, plus a
// declarative tree builder. Nothing here parses YAML semantically: a Unity file is
// a list of "--- !u!CLS &ID" blocks and that is the only structure we rely on.

const fs = require('fs');
const Y = require('./yamlkit.js');

const SCENE_ROOTS_ANCHOR = '9223372036854775807'; // a STRING: as a JS number it rounds.

function parse(path) {
  const txt = fs.readFileSync(path, 'utf8');
  const head = txt.slice(0, txt.indexOf('\n--- '));
  const docs = [];
  for (const part of txt.slice(txt.indexOf('\n--- ') + 1).split(/\n(?=--- )/)) {
    const h = part.match(/^--- !u!(\d+) &(-?\d+)( stripped)?/);
    if (!h) throw new Error('unparsable document in ' + path + ': ' + part.slice(0, 60));
    docs.push({ cls: h[1], id: h[2], text: part.replace(/\n*$/, '\n') });
  }
  return { path, head, docs };
}

function render(scene) {
  return scene.head + '\n' + scene.docs.map(d => d.text).join('') ;
}

function write(scene) { fs.writeFileSync(scene.path, render(scene)); }

function doc(scene, id) {
  const d = scene.docs.find(x => x.id === String(id));
  if (!d) throw new Error('no document &' + id + ' in ' + scene.path);
  return d;
}

// Assert a template id really is what the hardcoded table claims before writing through it.
function assertTemplate(scene, table) {
  for (const [id, want] of Object.entries(table)) {
    const d = doc(scene, id);
    if (d.cls !== String(want.cls)) throw new Error('&' + id + ': class is !u!' + d.cls + ', expected !u!' + want.cls);
    if (want.ident && !d.text.includes('m_EditorClassIdentifier: ' + want.ident))
      throw new Error('&' + id + ': not a ' + want.ident);
    if (want.name && !new RegExp('^  m_Name: ' + want.name + '\\s*$', 'm').test(d.text))
      throw new Error('&' + id + ': m_Name is not ' + want.name);
  }
}

// Replace a top-level "  key: value" line (single-line values only).
function setField(scene, id, key, value) {
  const d = doc(scene, id);
  const re = new RegExp('^  ' + key.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + ': .*$', 'm');
  if (!re.test(d.text)) throw new Error('&' + id + ': no field "' + key + '"');
  d.text = d.text.replace(re, '  ' + key + ': ' + value);
}

function hasField(scene, id, key) {
  return new RegExp('^  ' + key.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + ': ', 'm').test(doc(scene, id).text);
}

function getField(scene, id, key) {
  const m = doc(scene, id).text.match(new RegExp('^  ' + key.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + ': (.*)$', 'm'));
  return m ? m[1].trim() : null;
}

// Replace a nested "    key: value" line under a named parent block.
function setNested(scene, id, parent, key, value) {
  const d = doc(scene, id);
  const re = new RegExp('(^  ' + parent + ':\\n(?:    .*\\n)*?    ' + key + ': )(.*)$', 'm');
  if (!re.test(d.text)) throw new Error('&' + id + ': no nested field ' + parent + '/' + key);
  d.text = d.text.replace(re, '$1' + value);
}

function setChildren(scene, rtId, childIds) {
  const d = doc(scene, rtId);
  const body = childIds.length ? '\n' + childIds.map(c => '  - {fileID: ' + c + '}').join('\n') + '\n' : ' []\n';
  const re = /^  m_Children:(?: \[\]\n|\n(?:  - \{fileID: -?\d+\}\n)*)/m;
  if (!re.test(d.text)) throw new Error('&' + rtId + ': no m_Children block');
  d.text = d.text.replace(re, '  m_Children:' + body);
}

function children(scene, rtId) {
  const m = doc(scene, rtId).text.match(/^  m_Children:(?: \[\]|\n((?:  - \{fileID: -?\d+\}\n)*))/m);
  if (!m || !m[1]) return [];
  return [...m[1].matchAll(/fileID: (-?\d+)/g)].map(x => x[1]);
}

function addComponent(scene, goId, compId) {
  const d = doc(scene, goId);
  const re = /^(  m_Component:\n(?:  - component: \{fileID: -?\d+\}\n)*)/m;
  if (!re.test(d.text)) throw new Error('&' + goId + ': no m_Component block');
  d.text = d.text.replace(re, '$1  - component: {fileID: ' + compId + '}\n');
}

// Append documents, always BEFORE the SceneRoots document (Unity writes it last).
function appendDocs(scene, texts) {
  const blocks = texts.filter(Boolean).map(t => {
    const h = t.match(/^--- !u!(\d+) &(-?\d+)/);
    if (!h) throw new Error('appendDocs: not a document: ' + t.slice(0, 60));
    return { cls: h[1], id: h[2], text: t.replace(/\n*$/, '\n') };
  });
  const seen = new Set(scene.docs.map(d => d.id));
  for (const b of blocks) {
    if (seen.has(b.id)) throw new Error('appendDocs: duplicate fileID ' + b.id);
    seen.add(b.id);
  }
  const at = scene.docs.findIndex(d => d.cls === '1660057539');
  if (at < 0) scene.docs.push(...blocks);
  else scene.docs.splice(at, 0, ...blocks);
}

function sceneRoots(scene, rootRtIds) {
  const text = '--- !u!1660057539 &' + SCENE_ROOTS_ANCHOR + '\nSceneRoots:\n  m_ObjectHideFlags: 0\n' +
    '  m_Roots:\n' + rootRtIds.map(r => '  - {fileID: ' + r + '}').join('\n') + '\n';
  const at = scene.docs.findIndex(d => d.cls === '1660057539');
  if (at >= 0) scene.docs.splice(at, 1);
  scene.docs.push({ cls: '1660057539', id: SCENE_ROOTS_ANCHOR, text });
}

function existingRootRts(scene) {
  const sr = scene.docs.find(d => d.cls === '1660057539');
  if (sr) return [...sr.text.matchAll(/fileID: (-?\d+)/g)].map(x => x[1]);
  return scene.docs.filter(d => (d.cls === '4' || d.cls === '224') && /m_Father: \{fileID: 0\}/.test(d.text)).map(d => d.id);
}

// ── allocator ───────────────────────────────────────────────────────────────
// Ids are drawn from a high, template-free band so they can never collide.
function allocator(scene, start) {
  const used = new Set(scene.docs.map(d => d.id));
  let next = start;
  return () => {
    let id;
    do { id = String(next++); } while (used.has(id));
    used.add(id);
    return id;
  };
}

// ── declarative tree ────────────────────────────────────────────────────────
// node = { name, key?, active?, rect:{aMin,aMax,pos,size,pivot,scale,rotZ},
//          image?:{...}, text?:{...}, button?:{...}, group?:{...},
//          canvas?:{...}, monos?:[{cls,guid,fields(refs)=>[]}], children?:[] }
// Returns {docs, refs}. mono field bodies are deferred until every id exists.
function buildTree(node, fatherRt, alloc, out, refs) {
  const go = alloc();
  const rt = alloc();
  const comps = [rt];
  const entry = { go, rt };

  if (node.canvas) {
    const c = alloc(), sc = alloc(), rc = alloc();
    comps.push(c, sc, rc);
    out.push(Y.canvas(c, { go, sortingOrder: node.canvas.sortingOrder }));
    out.push(Y.canvasScaler(sc, { go, ref: node.canvas.ref, screenMatch: node.canvas.screenMatch, match: node.canvas.match }));
    out.push(Y.raycaster(rc, { go }));
    entry.canvas = c;
  }
  if (node.group) {
    const g = alloc();
    comps.push(g);
    out.push(Y.canvasGroup(g, Object.assign({ go }, node.group)));
    entry.group = g;
  }
  if (node.image) {
    const cr = alloc(), im = alloc();
    comps.push(cr, im);
    out.push(Y.canvasRenderer(cr, { go }));
    out.push(Y.image(im, Object.assign({ go }, node.image)));
    entry.image = im;
  }
  if (node.text) {
    const cr = alloc(), tm = alloc();
    comps.push(cr, tm);
    out.push(Y.canvasRenderer(cr, { go }));
    out.push(Y.tmpText(tm, Object.assign({ go }, node.text)));
    entry.tmp = tm;
  }
  if (node.button) {
    const b = alloc();
    comps.push(b);
    out.push(Y.button(b, Object.assign({ go, target: entry.image }, node.button)));
    entry.button = b;
  }
  const monoIds = [];
  for (const m of node.monos || []) {
    const id = alloc();
    comps.push(id);
    monoIds.push({ id, m });
    if (m.key) refs[m.key] = { mono: id };
  }

  const kids = [];
  for (const ch of node.children || []) kids.push(buildTree(ch, rt, alloc, out, refs).rt);

  out.push(Y.gameObject(go, { name: node.name, components: comps, active: node.active, layer: node.layer }));
  out.push(Y.rectTransform(rt, Object.assign({ go, father: fatherRt, children: kids }, node.rect || {})));

  // Deferred: a mono's serialized fields usually point at siblings built later.
  for (const { id, m } of monoIds) {
    out.push(() => Y.mono(id, { go, guid: m.guid, cls: m.cls, fields: typeof m.fields === 'function' ? m.fields(refs) : (m.fields || []) }));
  }

  if (node.key) refs[node.key] = Object.assign(refs[node.key] || {}, entry);
  return entry;
}

function flatten(out) {
  return out.map(x => (typeof x === 'function' ? x() : x));
}

module.exports = {
  parse, render, write, doc, assertTemplate, setField, getField, hasField, setNested,
  setChildren, children, addComponent, appendDocs, sceneRoots, existingRootRts,
  allocator, buildTree, flatten, SCENE_ROOTS_ANCHOR,
};
