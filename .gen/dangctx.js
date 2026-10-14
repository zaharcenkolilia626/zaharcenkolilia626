const fs=require('fs');
const known=new Set();
const walk=d=>{for(const e of fs.readdirSync(d,{withFileTypes:true})){const p=d+'/'+e.name;
 if(e.name.startsWith('._'))continue;
 if(e.isDirectory())walk(p);else if(e.name.endsWith('.meta')){const t=fs.readFileSync(p,'utf8');const m=/^guid: ([0-9a-f]{32})/m.exec(t);if(m)known.add(m[1]);}}};
walk('Assets');
for(const f of process.argv.slice(2)){
  const txt=fs.readFileSync(f,'utf8');
  const docs=[];const re=/^--- !u!(\d+) &(-?\d+)(?: stripped)?$/gm;let m,last=null;
  while((m=re.exec(txt))){if(last)last.body=txt.slice(last.start,m.index);last={cls:+m[1],id:m[2],start:re.lastIndex+1};docs.push(last);}
  if(last)last.body=txt.slice(last.start);
  console.log('\n== '+f);
  for(const d of docs){
    for(const line of d.body.split('\n')){
      const g=/guid: ([0-9a-f]{32})/.exec(line);
      if(g&&!known.has(g[1])&&!/^0{16}[ef]0{15}$/.test(g[1]))
        console.log(`  cls${d.cls} &${d.id}  ${line.trim()}`);
    }
  }
}
