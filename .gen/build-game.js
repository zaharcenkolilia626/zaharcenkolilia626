'use strict';
// Builds Games/2_GameScene.unity (build index 1). The pipeline seeds this scene with a
// camera and an EventSystem and nothing else, so the whole board is ours - authored as
// UGUI on one canvas, with no world sprite anywhere.
const fs = require('fs');
const path = require('path');
const Y = require('./yamlkit.js');
const S = require('./scenekit.js');
const D = require('./design.js');

const ROOT = path.resolve(__dirname, '..');
const G = JSON.parse(fs.readFileSync(path.join(__dirname, 'guids.json'), 'utf8'));
const cs = n => G['cs:' + n];
const sp = n => G['sp:' + n];
const tx = n => G['tx:' + n];
const pf = n => G['pf:' + n];

const CAMERA_RT = '1413414529';
const CAMERA = '1413414528';
const EVENTSYSTEM_RT = '2119698235';


// Refuse to run twice: these builders APPEND to the scene, so a second pass would
// duplicate every object. A rebuild starts from the pristine template scene.
if (/Assembly-CSharp::(SplashBrandView|BalanceDirector)/.test(require('fs').readFileSync(
    path.join(ROOT, 'Assets/Scenes/Games/2_GameScene.unity'), 'utf8'))) {
  console.error('Assets/Scenes/Games/2_GameScene.unity already carries generated content - restore the template scene before rebuilding');
  process.exit(2);
}
const scene = S.parse(path.join(ROOT, 'Assets/Scenes/Games/2_GameScene.unity'));
S.assertTemplate(scene, {
  [CAMERA]: { cls: 20 },
  [CAMERA_RT]: { cls: 4 },
});

const alloc = S.allocator(scene, 7300000);
const out = [];
const refs = {};

// ── board geometry, mirrored from ChamberLayout.cs ───────────────────────────
const L = {
  centreX: 621, beamY: 1800, beamSpan: 340, panX: 320, panRestY: 1480, panSize: 300,
  topBarDrop: 194, chipDrop: 340, hudY: 2230, hintY: 560, controlY: 390,
};

function iconButton(key, anchor, offsetX, spriteName) {
  return {
    name: key, key: key,
    rect: { aMin: anchor, aMax: anchor, pivot: [0.5, 0.5], pos: [offsetX, -L.topBarDrop], size: [132, 132] },
    image: D.plate(D.P.StoneSlab, 1.6),
    button: {},
    monos: [{
      cls: 'PressPulse', guid: cs('PressPulse'),
      fields: r => [['_target', Y.fid(r[key + 'Icon'].rt)], ['_pressedScale', '0.9']],
    }],
    children: [
      {
        name: 'Rim', rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [6, 6] },
        image: Object.assign(D.plate(D.a(D.P.StoneRidge, 0.95), 1.3), { raycast: false }),
      },
      {
        name: 'Icon', key: key + 'Icon',
        rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [72, 72] },
        image: D.art(sp(spriteName), D.P.ParchmentMuted),
      },
    ],
  };
}

function hudPlate(key, x, caption, accent) {
  const w = 372, h = 150;
  return {
    name: key, key: key, rect: D.at(x, L.hudY, w, h),
    image: D.plate(D.P.StoneSlab, 2.6),
    monos: [{
      key: key + 'View', cls: 'StatPlateView', guid: cs('StatPlateView'),
      fields: r => [
        ['_pip', Y.fid(r[key + 'Pip'].image)],
        ['_value', Y.fid(r[key + 'Value'].tmp)],
        ['_label', Y.fid(r[key + 'Label'].tmp)],
        ['_plate', Y.fid(r[key].image)],
      ],
    }],
    children: [
      {
        name: 'Pip', key: key + 'Pip',
        rect: { aMin: [0, 0.5], aMax: [0, 0.5], pos: [D.PLATE.gutterCentre, 0], size: [D.PLATE.gutterWidth, D.PLATE.gutterWidth] },
        image: Object.assign(D.plate(accent, 0.2), { raycast: false }),
      },
      {
        name: 'Value', key: key + 'Value',
        rect: { aMin: [0, 0.5], aMax: [0, 0.5], pos: [D.PLATE.textLeft + (D.PLATE.textWidth / 2), 22], size: [D.PLATE.textWidth, 62] },
        text: Object.assign({ text: '0', color: accent, hAlign: 1 }, D.TYPE.statValue),
      },
      {
        name: 'Label', key: key + 'Label',
        rect: { aMin: [0, 0.5], aMax: [0, 0.5], pos: [D.PLATE.textLeft + (D.PLATE.textWidth / 2), -30], size: [D.PLATE.textWidth, 42] },
        text: Object.assign({ text: caption, color: D.P.ParchmentMuted, hAlign: 1, charSpacing: 12 }, D.TYPE.caption),
      },
    ],
  };
}

function pan(key, isLeft) {
  const x = isLeft ? (L.centreX - L.panX) : (L.centreX + L.panX);
  return {
    name: key, key: key, rect: D.at(x, L.panRestY, L.panSize, L.panSize),
    // A sprite-less tinted quad still raycasts at alpha 0, which is what makes the whole
    // pan a tap target rather than only the painted dish.
    image: { sprite: null, type: 0, color: [0, 0, 0, 0] },
    monos: [{
      key: key + 'View', cls: 'PanView', guid: cs('PanView'),
      fields: r => [
        ['_rect', Y.fid(r[key].rt)],
        ['_glow', Y.fid(r[key + 'Glow'].image)],
        ['_dish', Y.fid(r[key + 'Dish'].image)],
        ['_sum', Y.fid(r[key + 'Sum'].tmp)],
        ['_sumRect', Y.fid(r[key + 'Sum'].rt)],
        ['_isLeft', isLeft ? '1' : '0'],
      ],
    }],
    children: [
      {
        name: 'Glow', key: key + 'Glow',
        rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [380, 380] },
        image: D.art(sp('sprite_pan_glow'), D.a(D.P.EmberGoldSoft, 0)),
      },
      {
        name: 'Dish', key: key + 'Dish',
        rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [L.panSize, L.panSize] },
        image: D.art(sp('sprite_pan')),
      },
    ],
  };
}

function chain(key, isLeft) {
  const links = [];
  for (let i = 0; i < 3; i++) {
    links.push({
      name: key + i, key: key + i,
      rect: D.at(L.centreX + ((isLeft ? -1 : 1) * L.panX), L.beamY - 60 - (i * 60), 26, 36),
      image: Object.assign(D.plate(D.a(D.P.EmberGoldSoft, 0.9), 0.3), { raycast: false }),
    });
  }
  return links;
}

function actionButton(key, x, caption, action, plateColor) {
  return {
    name: key, key: key, rect: D.at(x, L.controlY, 440, 150),
    image: D.plate(plateColor, 2.2),
    button: {},
    monos: [{
      key: key + 'View', cls: 'ActionButtonView', guid: cs('ActionButtonView'),
      fields: r => [
        ['_button', Y.fid(r[key].button)],
        ['_rect', Y.fid(r[key].rt)],
        ['_plate', Y.fid(r[key].image)],
        ['_caption', Y.fid(r[key + 'Caption'].tmp)],
        ['_director', Y.fid(r.director.mono)],
        ['_action', String(action)],
        ['_restColor', Y.col(plateColor)],
      ],
    }],
    children: [
      {
        name: 'Rim', rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [6, 6] },
        image: Object.assign(D.plate(D.a(D.P.StoneRidge, 0.9), 1.8), { raycast: false }),
      },
      {
        name: 'Caption', key: key + 'Caption',
        rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-40, -34] },
        text: Object.assign({
          text: caption,
          color: action === 0 ? D.P.Parchment : D.P.ParchmentMuted,
          charSpacing: 14,
        }, D.TYPE.title),
      },
    ],
  };
}

// ── a result card, dressed identically for all three outcomes ────────────────
const POP = { w: 1000, h: 1180 };
function popCard(key, popIndex, header, main, extra, primary, secondary) {
  return {
    name: key, key: key, active: false, rect: D.STRETCH,
    monos: [{
      key: key + 'View', cls: 'PopCardView', guid: cs('PopCardView'),
      fields: r => [
        ['_root', Y.fid(r[key].go)],
        ['_card', Y.fid(r[key + 'Card'].rt)],
        ['_cardPlate', Y.fid(r[key + 'Body'].image)],
        ['_cardFrame', Y.fid(r[key + 'Card'].image)],
        ['_header', Y.fid(r[key + 'Header'].tmp)],
        ['_main', Y.fid(r[key + 'Main'].tmp)],
        ['_additional', Y.fid(r[key + 'Extra'].tmp)],
        ['_closeButton', Y.fid(r[key + 'Close'].button)],
        ['_closeIcon', Y.fid(r[key + 'CloseIcon'].image)],
        ['_primaryButton', Y.fid(r[key + 'Primary'].button)],
        ['_primaryCaption', Y.fid(r[key + 'PrimaryCaption'].tmp)],
        ['_primaryPlate', Y.fid(r[key + 'Primary'].image)],
        ['_secondaryButton', Y.fid(r[key + 'Secondary'].button)],
        ['_secondaryCaption', Y.fid(r[key + 'SecondaryCaption'].tmp)],
        ['_secondaryPlate', Y.fid(r[key + 'Secondary'].image)],
      ],
    }],
    children: [
      {
        name: 'Veil', rect: D.STRETCH,
        image: { sprite: null, type: 0, color: D.a(D.P.ObsidianDeep, 0.78) },
      },
      {
        name: 'Card', key: key + 'Card',
        rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [POP.w, POP.h] },
        image: Object.assign(D.plate(D.P.EmberGold, 3.0), { color: D.a(D.P.EmberGold, 0.85) }),
        children: [
          {
            name: 'Body', key: key + 'Body',
            rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-24, -24] },
            image: D.plate(D.P.StoneSlab, 3.2),
          },
          {
            name: 'Header', key: key + 'Header',
            rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -140], size: [680, 110] },
            text: Object.assign({ text: header, color: D.P.EmberGold, charSpacing: 12 }, D.TYPE.popHeader),
          },
          {
            name: 'Main', key: key + 'Main',
            rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -320], size: [880, 76] },
            text: Object.assign({ text: main, color: D.P.Parchment }, D.TYPE.popBody),
          },
          {
            name: 'Extra', key: key + 'Extra',
            rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -430], size: [880, 64] },
            text: Object.assign({ text: extra, color: D.P.ParchmentMuted }, D.TYPE.popExtra),
          },
          {
            name: 'Close', key: key + 'Close',
            rect: { aMin: [1, 1], aMax: [1, 1], pos: [-80, -80], size: [112, 112] },
            image: D.plate(D.P.StoneRidge, 0.4),
            button: {},
            children: [{
              name: 'Icon', key: key + 'CloseIcon',
              rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [56, 56] },
              image: D.art(sp('sprite_icon_close'), D.P.ParchmentMuted),
            }],
          },
          {
            name: 'Primary', key: key + 'Primary',
            rect: { aMin: [0.5, 0], aMax: [0.5, 0], pos: [-215, 190], size: [400, 150] },
            image: D.plate(D.P.EmberGold, 2.2),
            button: {},
            children: [{
              name: 'Caption', key: key + 'PrimaryCaption',
              rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-36, -34] },
              // Authored light so the serialized-contrast stage is quiet; at runtime the
              // card sets it to Ink AND gives that one label a light outline, which is the
              // only sanctioned way to put a dark face on a gold plate.
              text: Object.assign({ text: primary, color: D.P.Parchment, charSpacing: 12 }, D.TYPE.title),
            }],
          },
          {
            name: 'Secondary', key: key + 'Secondary',
            rect: { aMin: [0.5, 0], aMax: [0.5, 0], pos: [215, 190], size: [400, 150] },
            image: D.plate(D.P.StoneRidge, 2.2),
            button: {},
            children: [{
              name: 'Caption', key: key + 'SecondaryCaption',
              rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-36, -34] },
              text: Object.assign({ text: secondary, color: D.P.ParchmentMuted, charSpacing: 12 }, D.TYPE.title),
            }],
          },
        ],
      },
    ],
  };
}

const CHAMBERS = [
  [2, 3, 1, 12, 18, 3, 9, 11],
  [3, 2, 3, 14, 20, 3, 10, 11],
  [3, 4, 1, 21, 30, 4, 12, 13],
  [4, 3, 1, 24, 33, 5, 13, 13],
  [4, 5, 1, 32, 44, 6, 16, 15],
];
const chamberRows = CHAMBERS.map(c =>
  '{LeftCount: ' + c[0] + ', RightCount: ' + c[1] + ', DecoyCount: ' + c[2] +
  ', TargetLow: ' + c[3] + ', TargetHigh: ' + c[4] + ', WeightLow: ' + c[5] +
  ', WeightHigh: ' + c[6] + ', Placements: ' + c[7] + '}');

const root = {
  name: 'GameRoot', key: 'root', layer: 5,
  rect: { aMin: [0, 0], aMax: [0, 0], pos: [0, 0], size: [0, 0], pivot: [0, 0] },
  canvas: { sortingOrder: 0, ref: D.REF, screenMatch: 1, match: 0 },
  monos: [
    { key: 'progress', cls: 'ProgressStore', guid: cs('ProgressStore'), fields: [] },
    { key: 'library', cls: 'ChamberLibrary', guid: cs('ChamberLibrary'), fields: [['_chambers', chamberRows]] },
    {
      key: 'director', cls: 'BalanceDirector', guid: cs('BalanceDirector'),
      fields: r => [
        ['_progress', Y.fid(r.progress.mono)],
        ['_library', Y.fid(r.library.mono)],
        ['_board', Y.fid(r.board.mono)],
        ['_hud', Y.fid(r.hud.mono)],
        ['_pops', Y.fid(r.pops.mono)],
        ['_stoneLayer', Y.fid(r.stoneLayer.rt)],
        // A serialized field typed as a COMPONENT and pointed at a prefab needs that
        // component's own fileID inside the prefab, not the root GameObject's.
        ['_stonePrefab', '{fileID: 7400004, guid: ' + pf('EmberStone') + ', type: 3}'],
        ['_sparkButton', Y.fid(r.SparkButtonView.mono)],
        ['_resetButton', Y.fid(r.ResetButtonView.mono)],
        ['_sparkMote', Y.fid(r.sparkMote.rt)],
      ],
    },
    {
      key: 'board', cls: 'BalanceBoardView', guid: cs('BalanceBoardView'),
      fields: r => [
        ['_beam', Y.fid(r.beam.rt)],
        ['_leftPan', Y.fid(r.PanLeftView.mono)],
        ['_rightPan', Y.fid(r.PanRightView.mono)],
        ['_leftChain', [0, 1, 2].map(i => Y.fid(r['chainL' + i].rt))],
        ['_rightChain', [0, 1, 2].map(i => Y.fid(r['chainR' + i].rt))],
      ],
    },
    {
      key: 'hud', cls: 'GameHudView', guid: cs('GameHudView'),
      fields: r => [
        ['_placesPlate', Y.fid(r.PlacesPlateView.mono)],
        ['_sparkPlate', Y.fid(r.SparksPlateView.mono)],
        ['_emberPlate', Y.fid(r.EmbersPlateView.mono)],
        ['_chamberTitle', Y.fid(r.chamberTitle.tmp)],
        ['_targetChip', Y.fid(r.targetChipLabel.tmp)],
        ['_hint', Y.fid(r.hint.tmp)],
      ],
    },
    {
      key: 'pops', cls: 'GamePopPresenter', guid: cs('GamePopPresenter'),
      fields: r => [
        ['_director', Y.fid(r.director.mono)],
        ['_progress', Y.fid(r.progress.mono)],
        ['_winCard', Y.fid(r.WinPopView.mono)],
        ['_loseCard', Y.fid(r.LosePopView.mono)],
        ['_pauseCard', Y.fid(r.PausePopView.mono)],
        ['_pauseTrigger', Y.fid(r.PauseButton.button)],
        ['_menuSceneIndex', '0'],
        ['_gameSceneIndex', '1'],
      ],
    },
    {
      key: 'backNav', cls: 'SceneNavButton', guid: cs('SceneNavButton'),
      fields: r => [['_button', Y.fid(r.BackButton.button)], ['_sceneIndex', '0']],
    },
  ],
  children: [
    { name: 'Background', rect: D.STRETCH, image: { sprite: tx('BG_game'), type: 0, raycast: false } },
    {
      name: 'Vignette', rect: D.STRETCH,
      image: { sprite: null, type: 0, color: D.a(D.P.ObsidianDeep, 0.3), raycast: false },
    },
    {
      name: 'TopBar', key: 'topBar', rect: D.STRETCH,
      children: [
        iconButton('BackButton', [0, 1], 110, 'sprite_icon_back'),
        {
          name: 'ChamberTitle', key: 'chamberTitle',
          rect: { aMin: [0.5, 1], aMax: [0.5, 1], pivot: [0.5, 0.5], pos: [0, -L.topBarDrop], size: [560, 70] },
          text: Object.assign({ text: 'CHAMBER I', color: D.P.Parchment, charSpacing: 16 }, D.TYPE.title),
        },
        iconButton('PauseButton', [1, 1], -110, 'sprite_icon_pause'),
        {
          name: 'TargetChip', key: 'targetChip',
          rect: { aMin: [0.5, 1], aMax: [0.5, 1], pivot: [0.5, 0.5], pos: [0, -L.chipDrop], size: [380, 96] },
          image: D.plate(D.a(D.P.EmberGold, 0.92), 1.0),
          children: [{
            name: 'Label', key: 'targetChipLabel',
            rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-30, -24] },
            text: Object.assign({ text: 'TARGET 12', color: D.P.Ink, charSpacing: 10 }, D.TYPE.chip),
          }],
        },
      ],
    },
    {
      name: 'Frame', key: 'frame', rect: D.FRAME,
      children: [
        hudPlate('PlacesPlate', 234, 'PLACES', D.P.EmberGold),
        hudPlate('SparksPlate', 621, 'SPARKS', D.P.SkyLapis),
        hudPlate('EmbersPlate', 1008, 'EMBERS', D.P.Parchment),
        {
          name: 'ScalePost', rect: D.at(L.centreX, 1640, 320, 320),
          image: D.art(sp('sprite_scale_post')),
        },
        {
          name: 'Beam', key: 'beam', rect: D.at(L.centreX, L.beamY, 680, 44),
          image: Object.assign(D.plate(D.P.EmberGoldSoft, 0.4), { raycast: false }),
          children: [{
            name: 'Pivot', rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [130, 130] },
            image: D.art(sp('sprite_pivot_dragon')),
          }],
        },
      ]
        .concat(chain('chainL', true))
        .concat(chain('chainR', false))
        .concat([
          pan('PanLeft', true),
          pan('PanRight', false),
          {
            name: 'SumLeft', key: 'PanLeftSum', rect: D.at(L.centreX - L.panX, L.panRestY - 240, 300, 80),
            text: Object.assign({ text: '0', color: D.P.EmberGold }, D.TYPE.statValue),
          },
          {
            name: 'SumRight', key: 'PanRightSum', rect: D.at(L.centreX + L.panX, L.panRestY - 240, 300, 80),
            text: Object.assign({ text: '0', color: D.P.EmberGold }, D.TYPE.statValue),
          },
          {
            name: 'StoneLayer', key: 'stoneLayer',
            rect: { aMin: [0, 0], aMax: [0, 0], pos: [0, 0], size: D.REF, pivot: [0, 0] },
          },
          {
            name: 'SparkMote', key: 'sparkMote', active: false, rect: D.at(870, L.controlY, 120, 120),
            image: D.art(sp('sprite_spark')),
          },
          {
            name: 'Hint', key: 'hint', rect: D.at(L.centreX, L.hintY, 1120, 56),
            text: Object.assign({
              text: 'TAP A STONE  -  TAP A PAN TO SET IT  -  TAP A SET STONE TO TAKE IT BACK',
              color: D.P.ParchmentMuted,
            }, D.TYPE.hint),
          },
          actionButton('ResetButton', 372, 'RESET', 1, D.P.StoneSlab),
          actionButton('SparkButton', 870, 'SPARK  5', 0, D.P.CrimsonDeep),
        ]),
    },
    {
      name: 'Pops', key: 'popsRoot', rect: D.STRETCH,
      children: [
        popCard('WinPop', 7, 'CHAMBER SEALED', 'TARGET HELD ON BOTH PANS', 'EMBERS EARNED', 'NEXT CHAMBER', 'PLAY AGAIN'),
        popCard('LosePop', 8, 'THE SCALES WENT COLD', 'NO PLACEMENTS LEFT', 'MEASURE THE LEAN NEXT TIME', 'TRY AGAIN', 'TEMPLE MAP'),
        popCard('PausePop', 6, 'SCALES HELD', 'THE BEAM IS WAITING', 'SPARKS REMAIN', 'RESUME', 'TEMPLE MAP'),
      ],
    },
  ],
};

S.buildTree(root, null, alloc, out, refs);
S.appendDocs(scene, S.flatten(out));

// The seeded scene copies the loader's camera verbatim, so it arrives orthographic at 16
// and clearing to transparent black. Neither matters to a screen-space canvas, but a sane
// clear colour keeps a stray frame from flashing pure black.
S.setField(scene, CAMERA, 'orthographic size', '5');
S.setField(scene, CAMERA, 'm_BackGroundColor', Y.col(D.P.ObsidianDeep));

// Same dead input-actions guid as the loader, eleven references, and without the swap no
// pointer event reaches a single one of the buttons below.
scene.docs.forEach(d => {
  d.text = d.text.replace(/guid: ca9f5fa95ffab41fb9a615ab714db018/g, 'guid: 2bcd2660ca9b64942af0de543d8d7100');
});

// The seeded scene has no SceneRoots document at all; list the camera and the EventSystem
// alongside our root, or they vanish from the hierarchy.
S.sceneRoots(scene, [CAMERA_RT, EVENTSYSTEM_RT, refs.root.rt]);
S.write(scene);
console.log('[game] documents: ' + scene.docs.length);
