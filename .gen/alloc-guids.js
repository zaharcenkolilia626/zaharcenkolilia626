// Allocate guids that are provably unused anywhere in the project (B.2).
const fs = require('fs'), crypto = require('crypto');
const known = new Set();
const walk = d => { for (const e of fs.readdirSync(d, { withFileTypes: true })) {
  const p = d + '/' + e.name;
  if (e.isDirectory()) walk(p);
  else { const t = fs.readFileSync(p, 'latin1');
    for (const m of t.matchAll(/[0-9a-f]{32}/g)) known.add(m[0]); } } };
for (const root of ['Assets', 'ProjectSettings', 'Packages']) walk(root);
console.error('guid-ish tokens already present: ' + known.size);
const names = JSON.parse(fs.readFileSync(process.argv[2], 'utf8'));
const out = {};
for (const n of names) {
  let g; do { g = crypto.randomBytes(16).toString('hex'); } while (known.has(g));
  known.add(g); out[n] = g;
}
fs.writeFileSync(process.argv[3], JSON.stringify(out, null, 2));
console.error('allocated ' + names.length + ' guids -> ' + process.argv[3]);
