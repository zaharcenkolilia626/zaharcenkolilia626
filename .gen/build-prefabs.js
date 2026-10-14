'use strict';
// The one spawnable object in the game: an ember stone. Everything else is authored
// directly in the scenes, so there is nothing else to instantiate.
//
// fileIDs here are fixed on purpose. A serialized field typed as a COMPONENT and pointed
// at a prefab needs that component's own id inside the prefab file, so the director's
// _stonePrefab must name 7400004 (the StoneView), not the root GameObject.
const fs = require('fs');
const path = require('path');
const Y = require('./yamlkit.js');
const D = require('./design.js');

const ROOT = path.resolve(__dirname, '..');
const G = JSON.parse(fs.readFileSync(path.join(__dirname, 'guids.json'), 'utf8'));
const sp = n => G['sp:' + n];
const cs = n => G['cs:' + n];

const HEAD = '%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n';

// root: 7400000 go / 7400001 rt / 7400002 cr / 7400003 image / 7400004 StoneView
// body: 7400010 go / 7400011 rt
// ring: 7400020 go / 7400021 rt / 7400022 cr / 7400023 image
// face: 7400030 go / 7400031 rt / 7400032 cr / 7400033 image
// numeral: 7400040 go / 7400041 rt / 7400042 cr / 7400043 tmp
const docs = [
  Y.gameObject('7400000', { name: 'EmberStone', components: ['7400001', '7400002', '7400003', '7400004'] }),
  Y.rectTransform('7400001', {
    go: '7400000', father: 0, children: ['7400011'],
    aMin: [0, 0], aMax: [0, 0], pivot: [0.5, 0.5], pos: [0, 0], size: [200, 200],
  }),
  Y.canvasRenderer('7400002', { go: '7400000' }),
  // Sprite-less and fully transparent: it still raycasts, so the whole square is tappable.
  Y.image('7400003', { go: '7400000', sprite: null, color: [0, 0, 0, 0] }),
  Y.mono('7400004', {
    go: '7400000', guid: cs('StoneView'), cls: 'StoneView',
    fields: [
      ['_rect', '{fileID: 7400001}'],
      ['_body', '{fileID: 7400011}'],
      ['_face', '{fileID: 7400033}'],
      ['_ring', '{fileID: 7400023}'],
      ['_numeral', '{fileID: 7400043}'],
    ],
  }),

  Y.gameObject('7400010', { name: 'Body', components: ['7400011'] }),
  Y.rectTransform('7400011', {
    go: '7400010', father: '7400001', children: ['7400021', '7400031', '7400041'],
    aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [0, 0],
  }),

  Y.gameObject('7400020', { name: 'Ring', components: ['7400021', '7400022', '7400023'], active: false }),
  Y.rectTransform('7400021', {
    go: '7400020', father: '7400011', children: [],
    aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [22, 22],
  }),
  Y.canvasRenderer('7400022', { go: '7400020' }),
  Y.image('7400023', Object.assign({ go: '7400020' }, D.plate(D.a(D.P.EmberGoldSoft, 0.9), 0.2), { raycast: false })),

  Y.gameObject('7400030', { name: 'Face', components: ['7400031', '7400032', '7400033'] }),
  Y.rectTransform('7400031', {
    go: '7400030', father: '7400011', children: [],
    aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-8, -8],
  }),
  Y.canvasRenderer('7400032', { go: '7400030' }),
  Y.image('7400033', Object.assign({ go: '7400030' }, D.art(sp('sprite_stone_ember')))),

  Y.gameObject('7400040', { name: 'Numeral', components: ['7400041', '7400042', '7400043'] }),
  Y.rectTransform('7400041', {
    go: '7400040', father: '7400011', children: [],
    aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, -4], size: [150, 80],
  }),
  Y.canvasRenderer('7400042', { go: '7400040' }),
  Y.tmpText('7400043', { go: '7400040', text: '0', size: 56, min: 40, color: D.P.Parchment }),
];

const file = path.join(ROOT, 'Assets/Prefabs/Generated/EmberStone.prefab');
fs.mkdirSync(path.dirname(file), { recursive: true });
fs.writeFileSync(file, HEAD + docs.join(''));

// Guids were reserved for three more prefabs that ended up authored directly in the
// scenes. An orphan .meta with no asset beside it is debt, so drop them.
for (const stale of ['ChamberNode', 'ArchiveCell', 'StatPlate']) {
  const p = path.join(ROOT, 'Assets/Prefabs/Generated', stale + '.prefab.meta');
  if (fs.existsSync(p)) fs.unlinkSync(p);
}

console.log('[prefabs] EmberStone.prefab: ' + docs.length + ' documents');
