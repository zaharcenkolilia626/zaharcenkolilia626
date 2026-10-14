'use strict';
// Resolve every RectTransform to a screen rectangle at several device shapes and report:
//   * an element that leaves the screen;
//   * a LABEL that overlaps a sibling it is not contained by.
// Inactive subtrees are NOT skipped - on this template the whole menu ships inactive, and
// a walker that skips them measures five objects and reports clean for ever. They are
// measured inside their own group instead, and only compared against that group.
//
// Usage: node layout-check.js <scene> [--canary] [--verbose]
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const REF = [1242, 2688];
// Portrait shapes that actually ship, widest aspect first.
const DEVICES = [[1080, 1920], [1080, 2400], [1284, 2778], [1440, 3200]];

function parseDocs(file) {
  const text = fs.readFileSync(file, 'utf8');
  const docs = new Map();
  for (const part of text.split(/\n(?=--- )/)) {
    const h = part.match(/^--- !u!(\d+) &(-?\d+)/);
    if (h) docs.set(h[2], { cls: h[1], body: part });
  }
  return docs;
}

const v2 = (body, key) => {
  const m = body.match(new RegExp('^  ' + key + ': \\{x: (-?[\\d.e-]+), y: (-?[\\d.e-]+)\\}', 'm'));
  return m ? [parseFloat(m[1]), parseFloat(m[2])] : [0, 0];
};
const fid = (body, key) => {
  const m = body.match(new RegExp('^  ' + key + ': \\{fileID: (-?\\d+)\\}', 'm'));
  return m ? m[1] : null;
};

function analyse(file, canary) {
  const docs = parseDocs(file);
  let text = fs.readFileSync(file, 'utf8');
  const rects = [...docs].filter(([, d]) => d.cls === '224');
  const byGo = new Map();
  for (const [id, d] of docs) {
    if (d.cls !== '224' && d.cls !== '4') continue;
    const go = fid(d.body, 'm_GameObject');
    if (go) byGo.set(go, id);
  }

  // Canary: shove one label far off the frame and assert the checker says so.
  let canaryId = null;
  if (canary) {
    for (const [id, d] of rects) {
      const go = fid(d.body, 'm_GameObject');
      const goDoc = docs.get(go);
      if (goDoc && /m_Name: Hint/.test(goDoc.body)) { canaryId = id; break; }
    }
    if (!canaryId) return { findings: ['canary target not found'], canaryHit: false };
    const d = docs.get(canaryId);
    d.body = d.body.replace(/m_AnchoredPosition: \{x: [-\d.]+, y: [-\d.]+\}/, 'm_AnchoredPosition: {x: 3000, y: 0}');
  }

  const info = new Map();
  for (const [id, d] of rects) {
    const go = fid(d.body, 'm_GameObject');
    const goDoc = docs.get(go);
    const name = goDoc ? (goDoc.body.match(/m_Name: (.*)/) || [, '?'])[1].trim() : '?';
    const active = goDoc ? !/m_IsActive: 0/.test(goDoc.body) : true;
    const isLabel = [...docs.values()].some(c =>
      c.cls === '114' && c.body.includes('m_GameObject: {fileID: ' + go + '}') &&
      /TMPro\.TextMeshProUGUI/.test(c.body));
    // A Canvas rect IS the screen: Unity overwrites its size every frame, so the authored
    // anchors and sizeDelta on it mean nothing and must not be used to derive children.
    const isCanvas = [...docs.values()].some(c =>
      c.cls === '223' && c.body.includes('m_GameObject: {fileID: ' + go + '}'));
    info.set(id, {
      id, name, active, isLabel, isCanvas,
      father: fid(d.body, 'm_Father'),
      aMin: v2(d.body, 'm_AnchorMin'), aMax: v2(d.body, 'm_AnchorMax'),
      pos: v2(d.body, 'm_AnchoredPosition'), size: v2(d.body, 'm_SizeDelta'),
      pivot: v2(d.body, 'm_Pivot'),
      children: [...(d.body.match(/^  m_Children:\n((?:  - \{fileID: -?\d+\}\n)*)/m) || [, ''])[1]
        .matchAll(/fileID: (-?\d+)/g)].map(m => m[1]),
    });
  }

  const findings = [];
  for (const device of DEVICES) {
    // CanvasScaler Expand: the canvas is the screen divided by the smaller of the two ratios.
    const scale = Math.min(device[0] / REF[0], device[1] / REF[1]);
    const canvas = [device[0] / scale, device[1] / scale];

    // Walk each canvas root; a canvas RectTransform is the screen rect itself.
    const roots = [...info.values()].filter(r => {
      if (!r.father || r.father === '0') return true;
      return !info.has(r.father);
    });

    const measured = [];
    const walk = (id, parentRect, groupKey) => {
      const r = info.get(id);
      if (!r) return;
      if (r.isCanvas) {
        const screen = [0, 0, canvas[0], canvas[1]];
        measured.push({ r, rect: screen, group: groupKey });
        for (const c of r.children) walk(c, screen, groupKey);
        return;
      }
      const pw = parentRect[2] - parentRect[0];
      const ph = parentRect[3] - parentRect[1];
      // Unity's own offset form: anchors give the reference box, the pivot places the rect.
      const axMin = parentRect[0] + (pw * r.aMin[0]);
      const axMax = parentRect[0] + (pw * r.aMax[0]);
      const ayMin = parentRect[1] + (ph * r.aMin[1]);
      const ayMax = parentRect[1] + (ph * r.aMax[1]);
      const w = (axMax - axMin) + r.size[0];
      const h = (ayMax - ayMin) + r.size[1];
      const cx = axMin + ((axMax - axMin) * r.pivot[0]) + r.pos[0];
      const cy = ayMin + ((ayMax - ayMin) * r.pivot[1]) + r.pos[1];
      const x0 = cx - (w * r.pivot[0]);
      const y0 = cy - (h * r.pivot[1]);
      const rect = [x0, y0, x0 + w, y0 + h];
      // An object switched off forms its OWN group: it is a real screen, just not now.
      const key = r.active ? groupKey : ('off:' + id);
      measured.push({ r, rect, group: key });
      for (const c of r.children) walk(c, rect, key);
    };

    // Each root is its own group: two panels of the same scene are different screens,
    // and comparing an object in one against an object in the other is meaningless.
    for (const root of roots) walk(root.id, [0, 0, canvas[0], canvas[1]], 'root:' + root.id);

    for (const m of measured) {
      const [x0, y0, x1, y1] = m.rect;
      const w = x1 - x0, h = y1 - y0;
      if (w <= 0 || h <= 0) continue;
      if (x1 < -1 || y1 < -1 || x0 > canvas[0] + 1 || y0 > canvas[1] + 1) {
        findings.push(device.join('x') + '  OFFSCREEN  "' + m.r.name + '" at [' +
          x0.toFixed(0) + ',' + y0.toFixed(0) + ' ' + x1.toFixed(0) + ',' + y1.toFixed(0) +
          '] canvas ' + canvas.map(n => n.toFixed(0)).join('x'));
      }
    }

    // Overlap: only pairs where at least one side is a LABEL, in the same group, and
    // neither contains the other (a plate under its own caption is the design, not a bug).
    const contains = (a, b) => a[0] <= b[0] + 1 && a[1] <= b[1] + 1 && a[2] >= b[2] - 1 && a[3] >= b[3] - 1;
    for (let i = 0; i < measured.length; i++) {
      for (let j = i + 1; j < measured.length; j++) {
        const a = measured[i], b = measured[j];
        if (a.group !== b.group) continue;
        if (!a.r.isLabel && !b.r.isLabel) continue;
        if (a.r.children.includes(b.r.id) || b.r.children.includes(a.r.id)) continue;
        if (a.r.father === b.r.id || b.r.father === a.r.id) continue;
        if (contains(a.rect, b.rect) || contains(b.rect, a.rect)) continue;
        const ox = Math.min(a.rect[2], b.rect[2]) - Math.max(a.rect[0], b.rect[0]);
        const oy = Math.min(a.rect[3], b.rect[3]) - Math.max(a.rect[1], b.rect[1]);
        if (ox > 2 && oy > 2) {
          findings.push(device.join('x') + '  OVERLAP    "' + a.r.name + '" x "' + b.r.name +
            '" by ' + ox.toFixed(0) + 'x' + oy.toFixed(0));
        }
      }
    }
  }

  return { findings, canaryHit: findings.some(f => /OFFSCREEN\s+"Hint"/.test(f)), measured: true };
}

const files = process.argv.slice(2).filter(a => !a.startsWith('--'));
const verbose = process.argv.includes('--verbose');
let bad = 0;
for (const f of files) {
  const abs = path.isAbsolute(f) ? f : path.join(ROOT, f);
  const r = analyse(abs, false);
  const unique = [...new Set(r.findings)];
  console.log('\n== ' + f + '  (' + unique.length + ' finding(s))');
  for (const line of unique.slice(0, verbose ? 500 : 30)) console.log('  ' + line);
  bad += unique.length;
}

if (process.argv.includes('--canary')) {
  const target = path.join(ROOT, 'Assets/Scenes/Games/2_GameScene.unity');
  const r = analyse(target, true);
  console.log('\ncanary: ' + (r.canaryHit ? 'surfaced (the checker really measures)' : 'DID NOT SURFACE - this check is blind'));
  if (!r.canaryHit) bad += 1000;
}

console.log('\nlayout-check: ' + bad + ' finding(s)');
process.exit(bad === 0 ? 0 : 1);
