// Print the GameObject hierarchy of a Unity scene/prefab text file.
const fs = require('fs');
const path = process.argv[2];
const txt = fs.readFileSync(path, 'utf8');
const docs = [];
const re = /^--- !u!(\d+) &(-?\d+)(?: stripped)?$/gm;
let m, last = null;
while ((m = re.exec(txt))) {
  if (last) last.body = txt.slice(last.start, m.index);
  last = { cls: +m[1], id: m[2], start: re.lastIndex + 1 };
  docs.push(last);
}
if (last) last.body = txt.slice(last.start);
const byId = new Map(docs.map(d => [d.id, d]));
const field = (b, n) => { const r = new RegExp('^  ' + n + ': (.*)$', 'm').exec(b); return r ? r[1].trim() : null; };
const gos = docs.filter(d => d.cls === 1);
const rts = docs.filter(d => d.cls === 4 || d.cls === 224);
const rtOfGo = new Map();
for (const rt of rts) rtOfGo.set(field(rt.body, 'm_GameObject').replace(/[{}]/g, '').replace('fileID: ', ''), rt);
const childrenOf = id => {
  const b = byId.get(id).body;
  const mm = /m_Children:\n((?:  - \{fileID: -?\d+\}\n)*)/.exec(b);
  if (!mm) return [];
  return [...mm[1].matchAll(/fileID: (-?\d+)/g)].map(x => x[1]);
};
const compsOf = goId => {
  const b = byId.get(goId).body;
  const mm = /m_Component:\n((?:  - component: \{fileID: -?\d+\}\n)*)/.exec(b);
  if (!mm) return [];
  return [...mm[1].matchAll(/fileID: (-?\d+)/g)].map(x => x[1]);
};
const typeOf = id => {
  const d = byId.get(id); if (!d) return '??(' + id + ')';
  if (d.cls === 114) { const e = field(d.body, 'm_EditorClassIdentifier'); return (e ? e.split('::').pop() : 'MonoBehaviour?') ; }
  return ({1:'GameObject',4:'Transform',224:'RectTransform',222:'CanvasRenderer',223:'Canvas',20:'Camera',81:'AudioListener',212:'SpriteRenderer',95:'Animator'})[d.cls] || ('cls' + d.cls);
};
const rect = id => {
  const d = byId.get(id); if (!d || d.cls !== 224) return '';
  const g = n => field(d.body, n) || '';
  return ` [aMin${g('m_AnchorMin')} aMax${g('m_AnchorMax')} pos${g('m_AnchoredPosition')} size${g('m_SizeDelta')} piv${g('m_Pivot')}]`;
};
const walk = (rtId, depth) => {
  const rt = byId.get(rtId); if (!rt) { console.log('  '.repeat(depth) + '?? rt ' + rtId); return; }
  const goId = field(rt.body, 'm_GameObject').replace(/[{}]/g, '').replace('fileID: ', '');
  const go = byId.get(goId);
  const name = field(go.body, 'm_Name');
  const act = field(go.body, 'm_IsActive');
  const comps = compsOf(goId).filter(c => c !== rtId).map(c => typeOf(c) + '@' + c);
  console.log('  '.repeat(depth) + `${name} (go ${goId}, rt ${rtId})${act === '0' ? ' [INACTIVE]' : ''} ${comps.join(' ')}` + rect(rtId));
  for (const c of childrenOf(rtId)) walk(c, depth + 1);
};
const sr = docs.find(d => d.cls === 1660057539);
if (sr) { for (const r of [...sr.body.matchAll(/fileID: (-?\d+)/g)].map(x => x[1])) walk(r, 0); }
else { console.log('(no SceneRoots) roots:'); for (const rt of rts) if ((field(rt.body,'m_Father')||'').includes('fileID: 0}')) walk(rt.id, 0); }
