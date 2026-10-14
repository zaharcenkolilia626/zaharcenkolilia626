'use strict';
// Theme the one font material every label in the app shares (hard rule 2).
//
// OUTLINE_ON is the part that is easy to miss: without the keyword TMP draws no outline at
// all, whatever _OutlineWidth says. The numbers stay inside the documented band because the
// atlas is small - dilate plus outline over ~0.45 fills the holes of P, R, 6 and A, which is
// how "PLAY" once shipped reading as "FLAY".
const fs = require('fs');
const path = require('path');

const FILE = path.resolve(__dirname, '..',
  'Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset');
const MATERIAL_ANCHOR = '--- !u!21 &2180264';

const COLORS = {
  _FaceColor: [1, 0.957, 0.902, 1],          // Parchment
  _OutlineColor: [0.09, 0.075, 0.125, 1],    // Ink
  _UnderlayColor: [0.055, 0.043, 0.078, 0.55],
};
const FLOATS = {
  _OutlineWidth: 0.24,   // 0.20 .. 0.30
  _FaceDilate: 0.19,     // 0.15 .. 0.25, and 0.24 + 0.19 = 0.43 <= 0.45
  _WeightBold: 0.4,      // <= 0.5
  _UnderlayDilate: 0.1,  // <= 0.25 - the shadow comes from the offset, not from dilate
  _UnderlayOffsetX: 0,
  _UnderlayOffsetY: -0.08,
  _UnderlaySoftness: 0.12,
};

let text = fs.readFileSync(FILE, 'utf8');
const start = text.indexOf(MATERIAL_ANCHOR);
if (start < 0) {
  console.error('font material document 2180264 not found');
  process.exit(1);
}
let head = text.slice(0, start);
let doc = text.slice(start);
const nextDoc = doc.indexOf('\n--- ', 1);
let tail = '';
if (nextDoc > 0) { tail = doc.slice(nextDoc); doc = doc.slice(0, nextDoc); }

let changes = 0;
function setFloat(name, value) {
  const re = new RegExp('(- ' + name + ': )(-?[\\d.eE+-]+)');
  if (!re.test(doc)) { console.error('missing float ' + name); return; }
  doc = doc.replace(re, '$1' + value);
  changes++;
}
function setColor(name, c) {
  const re = new RegExp('(- ' + name + ': )\\{[^}]*\\}');
  if (!re.test(doc)) { console.error('missing color ' + name); return; }
  doc = doc.replace(re, '$1{r: ' + c[0] + ', g: ' + c[1] + ', b: ' + c[2] + ', a: ' + c[3] + '}');
  changes++;
}

for (const [k, v] of Object.entries(FLOATS)) setFloat(k, v);
for (const [k, v] of Object.entries(COLORS)) setColor(k, v);

// This asset serialises m_ShaderKeywords and has no m_ValidKeywords list at all, so write
// both: the keyword list is what actually switches the outline pass on.
doc = doc.replace(/^(\s*)m_ShaderKeywords:.*$/m, '$1m_ShaderKeywords: OUTLINE_ON UNDERLAY_ON');
if (!/m_ValidKeywords:/.test(doc)) {
  doc = doc.replace(/^(\s*)m_ShaderKeywords: .*$/m,
    '$1m_ShaderKeywords: OUTLINE_ON UNDERLAY_ON\n$1m_ValidKeywords:\n$1- OUTLINE_ON\n$1- UNDERLAY_ON\n$1m_InvalidKeywords: []');
}
changes++;

fs.writeFileSync(FILE, head + doc + tail);
console.log('[font] material 2180264 themed (' + changes + ' edits)');
