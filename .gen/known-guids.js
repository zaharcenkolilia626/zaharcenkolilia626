const fs=require('fs'),path=require('path');
const known=new Map();
const walk=d=>{let es;try{es=fs.readdirSync(d,{withFileTypes:true})}catch(e){return}
 for(const e of es){const p=d+'/'+e.name; if(e.name.startsWith('._'))continue;
  if(e.isDirectory()){if(e.name==='node_modules')continue;walk(p)}
  else if(e.name.endsWith('.meta')){try{const t=fs.readFileSync(p,'utf8');const m=/^guid: ([0-9a-f]{32})/m.exec(t);if(m)known.set(m[1],p.replace(/\.meta$/,''))}catch(e){}}}};
for(const r of ['Assets','Packages','Library/PackageCache']) walk(r);
module.exports=known;
if(require.main===module){console.error('known: '+known.size);
 for(const g of process.argv.slice(2)) console.log(g, known.get(g)||'*** NOT FOUND ***');}
