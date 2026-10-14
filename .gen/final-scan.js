'use strict';
// The rules that no compiler and no pipeline gate enforces, checked over the files
// codegen owns. Each one maps to a defect that has shipped before.
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const GEN = path.join(ROOT, 'Assets/Scripts/Generated');
const SCENES = [
  path.join(ROOT, 'Assets/Scenes/Loader.unity'),
  path.join(ROOT, 'Assets/Scenes/Games/2_GameScene.unity'),
];
const PREFABS = fs.existsSync(path.join(ROOT, 'Assets/Prefabs/Generated'))
  ? fs.readdirSync(path.join(ROOT, 'Assets/Prefabs/Generated')).filter(f => f.endsWith('.prefab'))
      .map(f => path.join(ROOT, 'Assets/Prefabs/Generated', f))
  : [];

const problems = [];
const say = (rule, msg) => problems.push(rule + '  ' + msg);

// Comments and string literals both have to go before searching for an API name, or the
// scan reports a doc comment that says a thing is NOT used.
function codeOnly(text) {
  return text
    .replace(/\/\*[\s\S]*?\*\//g, ' ')
    .split('\n').filter(l => !/^\s*(\/\/|\*)/.test(l)).join('\n')
    .replace(/"(?:[^"\\]|\\.)*"/g, '""');
}

const files = fs.readdirSync(GEN).filter(f => f.endsWith('.cs'));
for (const file of files) {
  const full = path.join(GEN, file);
  const raw = fs.readFileSync(full, 'utf8');
  const code = codeOnly(raw);
  const name = file.replace(/\.cs$/, '');

  // 1. one top-level type per file, named after the file, and sealed or static.
  const types = [...raw.matchAll(/^(public|internal)\s+(?:(sealed|static|abstract|partial)\s+)*(class|struct|enum|interface)\s+([A-Za-z0-9_]+)/gm)];
  if (types.length !== 1) say('TYPES   ', file + ' declares ' + types.length + ' top-level types');
  else {
    const [, , modifier, kind, typeName] = types[0];
    if (typeName !== name) say('TYPES   ', file + ' declares ' + typeName);
    if (kind === 'class' && modifier !== 'sealed' && modifier !== 'static') {
      say('SEALED  ', file + ': ' + typeName + ' is neither sealed nor static');
    }
  }

  // 2. nothing that obfuscation renames out from under us.
  const BANNED = [
    [/\bGameObject\.Find\b|\bTransform\.Find\b|\bFindObjectOfType\b|\bFindFirstObjectByType\b|\bFindAnyObjectByType\b|\bFindObjectsOfType\b|\bFindObjectsByType\b/, 'Find* lookup'],
    [/GetComponent\s*\(\s*"/, 'string GetComponent'],
    [/\bResources\.Load\b/, 'Resources.Load'],
    [/\bType\.GetType\b|\bSystem\.Reflection\b/, 'reflection'],
    [/\bSendMessage\b|\bInvoke\s*\(\s*nameof|\bInvoke\s*\(\s*"/, 'name-based invoke'],
    [/\bnameof\s*\(/, 'nameof'],
    [/\bInput\.(GetKey|GetAxis|GetButton|GetMouseButton|mousePosition|touchCount|GetTouch|acceleration|gyro)/, 'legacy Input'],
    [/\bSceneManager\.LoadScene\s*\(\s*"/, 'LoadScene by name'],
    [/\bAudioSource\b|\bAudioClip\b|\bPlayOneShot\b|\bAudioListener\b/, 'audio API'],
    [/\bRigidbody2D\b|\bAddForce\b|\bAddTorque\b|\bMovePosition\b|\bgravityScale\b|\bOnCollision\w*2D\b|\bOnTrigger\w*2D\b/, 'body-driving API'],
    [/\bDOTween\b|\bDOFade\b|\bDOScale\b|\bDOMove\b/, 'tween library (none is installed)'],
  ];
  for (const [re, label] of BANNED) {
    if (re.test(code)) say('BANNED  ', file + ': ' + label);
  }
  // The physics stage reads comments too, so the vocabulary must be absent from them.
  if (/\bRigidbody2D\b|\bgravityScale\b|\bAddForce\b/.test(raw)) {
    say('BANNED  ', file + ': physics vocabulary appears even in a comment');
  }

  // 3. AddListener always with a lambda: a method group survives obfuscation unrenamed.
  for (const m of code.matchAll(/AddListener\s*\(([\s\S]{0,60})/g)) {
    if (!/^\s*\(\s*\)\s*=>/.test(m[1])) {
      say('LISTENER', file + ': AddListener(' + m[1].trim().slice(0, 40) + ') is not a lambda');
    }
  }

  // 4. placeholder strings.
  if (/\b(lorem|ipsum|placeholder text|dummy text|TODO)\b/i.test(code)) {
    say('PLACEHOLD', file + ': placeholder text in the code');
  }

  // 5. #if blocks must carry their own braces, or the obfuscator drops the #endif.
  const lines = raw.split('\n');
  for (let i = 0; i < lines.length; i++) {
    if (!/^\s*#if\b/.test(lines[i])) continue;
    const before = (lines[i - 1] || '').trim();
    let j = i + 1;
    while (j < lines.length && lines[j].trim() === '') j++;
    const after = (lines[j] || '').trim();
    if (before !== '{' || !after.startsWith('{')) {
      say('PREPROC ', file + ':' + (i + 1) + ': #if is not wrapped in its own brace block');
    }
  }

  // 6. the game's own name must not reach any string. Two title words together is the
  // test that matters: EMBERS alone is a currency, DRAGON is out entirely.
  for (const m of raw.matchAll(/"([^"\\]*)"/g)) {
    const s = m[1].toUpperCase();
    if (/\bDRAGON\b/.test(s)) say('NAME    ', file + ': a UI string contains DRAGON: "' + m[1] + '"');
    const words = ['EMBER', 'BALANCE', 'RUSH'].filter(w => new RegExp('\\b' + w).test(s));
    if (words.length >= 2) say('NAME    ', file + ': two title words in one string: "' + m[1] + '"');
  }
}

// 7. scenes and prefabs: no inspector-wired events, ASCII-only labels, no leftover text.
for (const file of SCENES.concat(PREFABS)) {
  const text = fs.readFileSync(file, 'utf8');
  const rel = path.relative(ROOT, file);
  if (/m_MethodName:/.test(text)) say('EVENT   ', rel + ': a persistent UnityEvent call (m_MethodName) survives in the file');
  for (const m of text.matchAll(/^  m_text: (.*)$/gm)) {
    const value = m[1];
    if (!value.trim()) continue;
    if (/[^\x20-\x7E]/.test(value)) say('ASCII   ', rel + ': non-ASCII in a label: ' + JSON.stringify(value));
    if (/^(New Text|Button|Text|HEADER|Lorem)/i.test(value.trim())) {
      say('PLACEHOLD', rel + ': template placeholder label still reads "' + value.trim() + '"');
    }
    const upper = value.toUpperCase();
    if (/\bDRAGON\b/.test(upper)) say('NAME    ', rel + ': a label contains DRAGON: "' + value + '"');
    const words = ['EMBER', 'BALANCE', 'RUSH'].filter(w => new RegExp('\\b' + w).test(upper));
    if (words.length >= 2) say('NAME    ', rel + ': two title words in one label: "' + value + '"');
  }
  // Every UGUI label ships at or above the readability floor; autosize squeezes to the
  // minimum, so the minimum is the size that actually reaches the screen.
  for (const m of text.matchAll(/m_fontSizeMin: ([\d.]+)/g)) {
    if (parseFloat(m[1]) < 32) say('FONTSIZE', rel + ': m_fontSizeMin ' + m[1] + ' is under the 32 floor');
  }
  for (const m of text.matchAll(/m_enableAutoSizing: (\d)/g)) {
    if (m[1] === '0') say('AUTOSIZE', rel + ': a label ships with autosize off');
  }
  for (const m of text.matchAll(/m_TextWrappingMode: (\d)/g)) {
    if (m[1] !== '0') say('WRAP    ', rel + ': a label ships with word wrap on');
  }
}

// 8. every .cs has its .cs.meta and every guid is unique.
const guids = new Map();
const walkMeta = dir => {
  for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, e.name);
    if (e.isDirectory()) walkMeta(p);
    else if (e.name.endsWith('.meta')) {
      const m = fs.readFileSync(p, 'utf8').match(/^guid: ([0-9a-f]{32})/m);
      if (!m) continue;
      if (guids.has(m[1])) say('GUID    ', 'duplicate guid ' + m[1] + ': ' + guids.get(m[1]) + ' and ' + p);
      guids.set(m[1], p);
    }
  }
};
walkMeta(path.join(ROOT, 'Assets'));
for (const file of files) {
  if (!fs.existsSync(path.join(GEN, file + '.meta'))) say('META    ', file + ' has no .cs.meta');
}

console.log('final-scan: ' + files.length + ' script(s), ' + SCENES.length + ' scene(s), ' +
  PREFABS.length + ' prefab(s), ' + guids.size + ' guid(s)');
for (const p of problems) console.log('  ' + p);
console.log(problems.length === 0 ? 'CLEAN' : problems.length + ' PROBLEM(S)');
process.exit(problems.length === 0 ? 0 : 1);
