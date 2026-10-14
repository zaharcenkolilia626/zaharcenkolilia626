'use strict';
// Resolve EVERY serialized field of every generated component and report it as
//   field -> <Type> on "<GameObject path>"
// plus the two failure modes that look identical in a diff and are invisible at runtime:
//   * a reference field left at {fileID: 0}, or never written into the YAML at all;
//   * a reference that resolves to the WRONG declared type.
// Eyeballing fileIDs does not catch either. Usage: node audit.js <file> [file...]
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');

function readGuidIndex() {
  const index = new Map();
  const walk = dir => {
    let entries = [];
    try { entries = fs.readdirSync(dir, { withFileTypes: true }); } catch (_) { return; }
    for (const e of entries) {
      const p = path.join(dir, e.name);
      if (e.isDirectory()) walk(p);
      else if (e.name.endsWith('.meta')) {
        const m = fs.readFileSync(p, 'utf8').match(/^guid: ([0-9a-f]{32})/m);
        if (m) index.set(m[1], p.replace(/\.meta$/, ''));
      }
    }
  };
  walk(path.join(ROOT, 'Assets'));
  return index;
}

// [SerializeField] private <T> _x;   (an initialiser before the semicolon is allowed:
// without that, array fields - the ones most worth checking - are never seen)
function declaredFields(csFile) {
  const text = fs.readFileSync(csFile, 'utf8');
  const fields = new Map();
  const re = /\[SerializeField\]\s*(?:private|protected|public|internal)?\s*([A-Za-z0-9_<>\[\].]+)\s+([A-Za-z0-9_]+)\s*(?:=[^;]*)?;/g;
  let m;
  while ((m = re.exec(text))) fields.set(m[2], m[1]);
  return fields;
}

function parseDocs(file) {
  const text = fs.readFileSync(file, 'utf8');
  const docs = new Map();
  for (const part of text.split(/\n(?=--- )/)) {
    const h = part.match(/^--- !u!(\d+) &(-?\d+)/);
    if (h) docs.set(h[2], { cls: h[1], body: part });
  }
  return docs;
}

const CLASS_NAMES = {
  1: 'GameObject', 4: 'Transform', 224: 'RectTransform', 222: 'CanvasRenderer',
  223: 'Canvas', 225: 'CanvasGroup', 20: 'Camera', 81: 'AudioListener', 212: 'SpriteRenderer',
};

function componentType(doc) {
  if (!doc) return null;
  if (doc.cls === '114') {
    const m = doc.body.match(/m_EditorClassIdentifier: (.*)/);
    if (!m) return 'MonoBehaviour';
    // "UnityEngine.UI::UnityEngine.UI.Image" -> "Image": the assembly prefix and the
    // namespace are both noise when comparing against a C# field's declared type.
    return m[1].trim().split('::').pop().split('.').pop();
  }
  return CLASS_NAMES[doc.cls] || ('cls' + doc.cls);
}

function goPath(docs, goId) {
  const names = [];
  let current = goId;
  for (let hop = 0; hop < 40 && current; hop++) {
    const go = docs.get(current);
    if (!go || go.cls !== '1') break;
    const n = go.body.match(/m_Name: (.*)/);
    names.unshift(n ? n[1].trim() : '?');
    let rt = null;
    for (const [id, d] of docs) {
      if ((d.cls === '4' || d.cls === '224') && d.body.includes('m_GameObject: {fileID: ' + current + '}')) { rt = d; break; }
    }
    if (!rt) break;
    const father = rt.body.match(/m_Father: \{fileID: (-?\d+)\}/);
    if (!father || father[1] === '0') break;
    const fd = docs.get(father[1]);
    if (!fd) break;
    const fgo = fd.body.match(/m_GameObject: \{fileID: (-?\d+)\}/);
    current = fgo ? fgo[1] : null;
  }
  return names.join('/');
}

const guidIndex = readGuidIndex();
// Component-typed fields that legitimately point at a PREFAB rather than into the scene.
const prefabDocs = new Map();
for (const [guid, file] of guidIndex) {
  if (file.endsWith('.prefab') && fs.existsSync(file)) prefabDocs.set(guid, parseDocs(file));
}

const VALUE_TYPES = /^(int|float|bool|string|double|long|Color|Vector2|Vector3|Rect|char|byte)(\[\])?$/;

let problems = 0;
let checked = 0;
const verbose = process.argv.includes('--verbose');

for (const file of process.argv.slice(2).filter(a => !a.startsWith('--'))) {
  const docs = parseDocs(path.isAbsolute(file) ? file : path.join(ROOT, file));
  console.log('\n== ' + file);

  for (const [id, doc] of docs) {
    if (doc.cls !== '114') continue;
    const ident = doc.body.match(/m_EditorClassIdentifier: Assembly-CSharp::([A-Za-z0-9_]+)/);
    if (!ident) continue;
    const cls = ident[1];
    const csFile = path.join(ROOT, 'Assets/Scripts/Generated', cls + '.cs');
    if (!fs.existsSync(csFile)) continue;

    const declared = declaredFields(csFile);
    const goId = (doc.body.match(/m_GameObject: \{fileID: (-?\d+)\}/) || [])[1];
    const where = goPath(docs, goId) || '(root)';

    for (const [field, type] of declared) {
      checked++;
      const isArray = type.endsWith('[]');
      const bare = type.replace('[]', '');
      const valueTyped = VALUE_TYPES.test(type);

      // Sequence items are written flush with the parent's indentation.
      // A document split off at "--- " carries no trailing newline, so the final sequence
      // item of the final field has to be allowed to end at end-of-string. Without this the
      // last array in a document always reads as empty.
      const listRe = new RegExp('^  ' + field + ':(?: \\[\\]|\\n((?:  - .*(?:\\n|$))*))', 'm');
      const scalarRe = new RegExp('^  ' + field + ': (.*)$', 'm');
      const listMatch = isArray ? doc.body.match(listRe) : null;
      const scalarMatch = !isArray ? doc.body.match(scalarRe) : null;

      if (!listMatch && !scalarMatch) {
        if (!valueTyped) {
          console.log('  MISSING  ' + cls + '.' + field + ' (' + type + ') on "' + where + '" - never written to the YAML');
          problems++;
        }
        continue;
      }

      if (valueTyped) continue;

      const values = isArray
        ? [...((listMatch && listMatch[1]) || '').matchAll(/\{fileID: (-?\d+)(?:, guid: ([0-9a-f]{32}))?/g)]
            .map(m => ({ id: m[1], guid: m[2] }))
        : [(() => {
            const m = scalarMatch[1].match(/\{fileID: (-?\d+)(?:, guid: ([0-9a-f]{32}))?/);
            return m ? { id: m[1], guid: m[2] } : null;
          })()].filter(Boolean);

      // An array of a [Serializable] data class is inline YAML, not references.
      if (isArray && values.length === 0 && listMatch && listMatch[1] && !/\{fileID:/.test(listMatch[1])) {
        continue;
      }

      if (values.length === 0) {
        console.log('  EMPTY    ' + cls + '.' + field + ' (' + type + ') on "' + where + '"');
        if (!isArray) problems++;
        continue;
      }

      for (const v of values) {
        if (v.guid) {
          const target = guidIndex.get(v.guid);
          if (!target) {
            console.log('  DANGLING ' + cls + '.' + field + ' -> guid ' + v.guid + ' resolves to nothing');
            problems++;
            continue;
          }
          // A component-typed field pointing into a prefab must name that component.
          if (target.endsWith('.prefab') && v.id !== '0') {
            const inner = prefabDocs.get(v.guid);
            const innerType = inner ? componentType(inner.get(v.id)) : null;
            const ok = innerType === bare || bare === 'GameObject';
            if (!ok) {
              console.log('  WRONGTYPE ' + cls + '.' + field + ' wants ' + bare + ' but prefab id ' + v.id + ' is ' + innerType);
              problems++;
            } else if (verbose) {
              console.log('  ok       ' + cls + '.' + field + ' -> ' + innerType + ' in ' + path.basename(target));
            }
          } else if (verbose) {
            console.log('  ok       ' + cls + '.' + field + ' -> ' + path.basename(target));
          }
          continue;
        }

        if (v.id === '0') {
          console.log('  ZERO     ' + cls + '.' + field + ' (' + type + ') on "' + where + '" is {fileID: 0} - a dead reference');
          problems++;
          continue;
        }

        const target = docs.get(v.id);
        if (!target) {
          console.log('  NOANCHOR ' + cls + '.' + field + ' -> &' + v.id + ' has no document in this file');
          problems++;
          continue;
        }

        const actual = componentType(target);
        const wanted = bare;
        const compatible =
          actual === wanted ||
          (wanted === 'GameObject' && actual === 'GameObject') ||
          (wanted === 'TMP_Text' && actual === 'TextMeshProUGUI') ||
          (wanted === 'TextMeshProUGUI' && actual === 'TextMeshProUGUI') ||
          (wanted === 'Graphic' && (actual === 'Image' || actual === 'TextMeshProUGUI'));
        if (!compatible) {
          console.log('  WRONGTYPE ' + cls + '.' + field + ' wants ' + wanted + ' but &' + v.id + ' is ' + actual +
            ' on "' + goPath(docs, (target.body.match(/m_GameObject: \{fileID: (-?\d+)\}/) || [])[1]) + '"');
          problems++;
        } else if (verbose) {
          console.log('  ok       ' + cls + '.' + field + ' -> ' + actual +
            ' on "' + goPath(docs, (target.body.match(/m_GameObject: \{fileID: (-?\d+)\}/) || [])[1]) + '"');
        }
      }
    }
  }
}

console.log('\naudit: ' + checked + ' field(s) checked, ' + problems + ' problem(s)');
process.exit(problems === 0 ? 0 : 1);
