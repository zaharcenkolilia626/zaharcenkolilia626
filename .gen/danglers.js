const fs=require('fs');
const files=process.argv.slice(2);
// collect all guids known to the project
const known=new Set();
const walk=d=>{for(const e of fs.readdirSync(d,{withFileTypes:true})){const p=d+'/'+e.name;
 if(e.name.startsWith('._'))continue;
 if(e.isDirectory())walk(p);else if(e.name.endsWith('.meta')){const t=fs.readFileSync(p,'utf8');const m=/^guid: ([0-9a-f]{32})/m.exec(t);if(m)known.add(m[1]);}}};
walk('Assets');
console.log('known guids:',known.size);
for(const f of files){const t=fs.readFileSync(f,'utf8');const bad=new Map();
 for(const m of t.matchAll(/guid: ([0-9a-f]{32})/g)){ if(!known.has(m[1])&&m[1]!=='0000000000000000f000000000000000'&&m[1]!=='0000000000000000e000000000000000') bad.set(m[1],(bad.get(m[1])||0)+1); }
 console.log('\n==',f); for(const [g,n] of bad) console.log('  DANGLING',g,'x'+n);
}
