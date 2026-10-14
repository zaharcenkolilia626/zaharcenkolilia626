'use strict';
// Build a Roslyn response file that type-checks Assets/Scripts/Generated against the REAL
// UnityEngine reference assemblies plus prebuilt UGUI / TextMeshPro / InputSystem assemblies.
// Prebuilt beats recompiling their sources: those packages' internals are visible only to
// their own assembly names, so a source build reports dozens of errors that are not ours.
// Arguments go in a response file because a long -r: list on the command line fails.
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const EDITOR = process.argv[2] && process.argv[2].charAt(0) !== '-'
  ? process.argv[2]
  : 'C:/Program Files/Unity/Hub/Editor/6000.5.10f1';
const DATA = EDITOR + '/Editor/Data';
const LIBCACHE = DATA + '/Resources/PackageManager/ProjectTemplates/libcache/' +
  'com.unity.template.2d-cross-platform-2d-6.1.5/ScriptAssemblies';
const PKG_REFS = ['UnityEngine.UI.dll', 'Unity.TextMeshPro.dll', 'Unity.InputSystem.dll'];
const OUT = path.join(__dirname, 'typecheck');

function walk(dir, hit) {
  let entries = [];
  try { entries = fs.readdirSync(dir, { withFileTypes: true }); } catch (_) { return; }
  for (const e of entries) {
    const p = path.join(dir, e.name);
    if (e.isDirectory()) walk(p, hit);
    else if (e.name.endsWith('.cs')) hit.push(p);
  }
}

const refs = [];
for (const dll of fs.readdirSync(DATA + '/Managed/UnityEngine')) {
  if (!dll.endsWith('.dll')) continue;
  // The monolithic UnityEngine.dll beside the modules gives 300+ CS0433 duplicates.
  if (dll === 'UnityEngine.dll') continue;
  refs.push(DATA + '/Managed/UnityEngine/' + dll);
}
refs.push(DATA + '/NetStandard/ref/2.1.0/netstandard.dll');
let missing = 0;
for (const dll of PKG_REFS) {
  const p = LIBCACHE + '/' + dll;
  if (fs.existsSync(p)) refs.push(p);
  else { console.error('MISSING package ref: ' + dll); missing++; }
}

const sources = [];
walk(path.join(ROOT, 'Assets/Scripts/Generated'), sources);

fs.mkdirSync(OUT, { recursive: true });
const lines = [
  '-target:library',
  '-out:' + OUT.replace(/\\/g, '/') + '/Generated.dll',
  '-nostdlib+',
  '-noconfig',
  '-langversion:9.0',
  '-preferreduilang:en',
  '-define:UNITY_2023_1_OR_NEWER;UNITY_6000_0_OR_NEWER;UNITY_ANDROID;TMP_PRESENT',
  '-nowarn:0414,0649,0108,0168,0219,1591,0618,0067',
];
if (process.argv.includes('--logs')) lines.push('-define:B_LOGS');
for (const r of refs) lines.push('-r:"' + r.replace(/\\/g, '/') + '"');
for (const s of sources) lines.push('"' + s.replace(/\\/g, '/') + '"');

fs.writeFileSync(path.join(OUT, 'check.rsp'), lines.join('\n'));
console.log('sources: ' + sources.length + '  refs: ' + refs.length + '  missing: ' + missing);
