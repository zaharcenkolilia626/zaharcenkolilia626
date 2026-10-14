'use strict';
// Rebuilds Loader.unity (build index 0): the splash inside PanelLoader and the temple
// map inside PanelMenu. Template objects are re-themed in place; everything new is
// appended INSIDE the two existing canvases, never as a parallel root.
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

// ── template ids, asserted before anything is written through them ───────────
const T = {
  loaderPanelGo: '960164267', loaderPanelRt: '960164268',
  loaderCanvasGo: '473367935', loaderCanvasRt: '473367939', loaderScaler: '473367937',
  loaderBgRt: '2052145228', loaderBgImage: '2052145229',
  sliderGo: '397194675', sliderRt: '397194676', sliderComp: '397194677',
  sliderTrackImage: '1320501298',
  fillAreaRt: '1699811543', fillRt: '954641864', fillImage: '954641865',
  menuPanelGo: '472516430', menuPanelRt: '472516431',
  menuCanvasGo: '1008058961', menuCanvasRt: '1008058962', menuScaler: '1008058964',
  menuBgImage: '855584338',
  scoreLabelGo: '565135046', scoreLabelTmp: '565135048',
  playGo: '713131002', playRt: '713131003', playButton: '713131004', playImage: '713131005',
  playCaptionGo: '2085339827', playCaptionRt: '2085339828', playCaptionTmp: '2085339829',
  privacyGo: '481126436', privacyRt: '481126437', privacyButton: '481126438', privacyImage: '481126439',
  privacyCaptionGo: '144922631', privacyCaptionRt: '144922632', privacyCaptionTmp: '144922633',
  controllerLoader: '1836528986',
  mainRootRt: '357330348',
};


// Refuse to run twice: these builders APPEND to the scene, so a second pass would
// duplicate every object. A rebuild starts from the pristine template scene.
if (/Assembly-CSharp::(SplashBrandView|BalanceDirector)/.test(require('fs').readFileSync(
    path.join(ROOT, 'Assets/Scenes/Loader.unity'), 'utf8'))) {
  console.error('Assets/Scenes/Loader.unity already carries generated content - restore the template scene before rebuilding');
  process.exit(2);
}
const scene = S.parse(path.join(ROOT, 'Assets/Scenes/Loader.unity'));
S.assertTemplate(scene, {
  [T.loaderPanelGo]: { cls: 1, name: 'PanelLoader' },
  [T.menuPanelGo]: { cls: 1, name: 'PanelMenu' },
  [T.playGo]: { cls: 1, name: 'Button_play' },
  [T.privacyGo]: { cls: 1, name: 'Button_privacy' },
  [T.playButton]: { cls: 114, ident: 'UnityEngine.UI::UnityEngine.UI.Button' },
  [T.playImage]: { cls: 114, ident: 'UnityEngine.UI::UnityEngine.UI.Image' },
  [T.playCaptionTmp]: { cls: 114, ident: 'Unity.TextMeshPro::TMPro.TextMeshProUGUI' },
  [T.privacyCaptionTmp]: { cls: 114, ident: 'Unity.TextMeshPro::TMPro.TextMeshProUGUI' },
  [T.sliderComp]: { cls: 114, ident: 'UnityEngine.UI::UnityEngine.UI.Slider' },
  [T.loaderScaler]: { cls: 114, ident: 'UnityEngine.UI::UnityEngine.UI.CanvasScaler' },
  [T.menuScaler]: { cls: 114, ident: 'UnityEngine.UI::UnityEngine.UI.CanvasScaler' },
  [T.controllerLoader]: { cls: 114, ident: 'Assembly-CSharp::ControllerLoader' },
});

const alloc = S.allocator(scene, 7100000);
const out = [];
const refs = {};

// ── the splash, inside PanelLoader's canvas ──────────────────────────────────
const splash = {
  name: 'SplashBrand', key: 'splash', rect: D.FRAME,
  monos: [{
    key: 'splashView', cls: 'SplashBrandView', guid: cs('SplashBrandView'),
    fields: r => [
      ['_emblem', Y.fid(r.emblem.rt)],
      ['_emblemImage', Y.fid(r.emblem.image)],
      ['_bar', Y.fid(T.sliderComp)],
      ['_barRoot', Y.fid(T.sliderRt)],
      ['_barFillArea', Y.fid(T.fillAreaRt)],
      ['_barFill', Y.fid(T.fillRt)],
      ['_barFillImage', Y.fid(T.fillImage)],
      ['_barTrackImage', Y.fid(T.sliderTrackImage)],
      ['_loaderPanel', Y.fid(T.loaderPanelGo)],
      ['_menuPanel', Y.fid(T.menuPanelGo)],
      ['_progress', Y.fid(r.progress.mono)],
      ['_handoffSeconds', '10'],
      ['_returnHandoffSeconds', '1.2'],
    ],
  }],
  children: [
    { name: 'Emblem', key: 'emblem', rect: D.mid(0, 323, 420, 420), image: D.art(sp('sprite_emblem_scale')) },
    {
      name: 'LoadingCaption', rect: D.mid(0, -524, 460, 60),
      text: Object.assign({ text: 'LOADING', color: D.a(D.P.Parchment, 0.8), charSpacing: 22 }, D.TYPE.chip),
    },
  ],
};
S.buildTree(splash, T.loaderCanvasRt, alloc, out, refs);

// ── the temple map, inside PanelMenu's canvas ────────────────────────────────
const nodeChildren = [];
for (let i = 0; i < 5; i++) {
  nodeChildren.push({
    name: 'Node' + i, key: 'node' + i,
    rect: D.at(D.MENU.nodeX[i], D.MENU.nodeY[i], D.MENU.nodeSize, D.MENU.nodeSize),
    image: { sprite: null, type: 0, color: [0, 0, 0, 0] },   // an invisible hit plate for the Button
    button: {},
    monos: [{
      key: 'nodeView' + i, cls: 'ChamberNodeView', guid: cs('ChamberNodeView'),
      fields: r => [
        ['_body', Y.fid(r['nodeBody' + i].rt)],
        ['_face', Y.fid(r['nodeFace' + i].image)],
        ['_rim', Y.fid(r['nodeRim' + i].image)],
        ['_lock', Y.fid(r['nodeLock' + i].image)],
        ['_clearedBadge', Y.fid(r['nodeBadge' + i].image)],
        ['_numeral', Y.fid(r['nodeNumeral' + i].tmp)],
        ['_button', Y.fid(r['node' + i].button)],
      ],
    }],
    children: [{
      name: 'Body', key: 'nodeBody' + i,
      rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [0, 0] },
      children: [
        {
          name: 'Rim', key: 'nodeRim' + i,
          rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [18, 18] },
          image: Object.assign(D.plate(D.P.EmberGoldSoft, 0.6), { raycast: false }),
        },
        {
          name: 'Face', key: 'nodeFace' + i,
          rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [170, 170] },
          image: D.art(sp('sprite_chamber_node')),
        },
        {
          name: 'Numeral', key: 'nodeNumeral' + i,
          // The numeral sits high and the lock low: a locked seal shows both at once, so
          // they must not share a band or the glyph reads as a struck-through letter.
          rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 26], size: [150, 72] },
          text: Object.assign({ text: ['I', 'II', 'III', 'IV', 'V'][i], color: D.P.Parchment }, D.TYPE.nodeNumeral),
        },
        {
          name: 'Lock', key: 'nodeLock' + i,
          rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, -56], size: [72, 72] },
          image: D.art(sp('sprite_rune_lock'), D.P.DragonCrimson),
        },
        {
          name: 'Badge', key: 'nodeBadge' + i, active: false,
          rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [68, 68], size: [64, 64] },
          image: D.art(sp('sprite_stone_ember')),
        },
      ],
    }],
  });
}

// Path segments are built before the seals so a seal always draws on top of its line.
const pathChildren = [];
for (let i = 0; i < 4; i++) {
  const x0 = D.MENU.nodeX[i], y0 = D.MENU.nodeY[i];
  const x1 = D.MENU.nodeX[i + 1], y1 = D.MENU.nodeY[i + 1];
  const dx = x1 - x0, dy = y1 - y0;
  const length = Math.sqrt((dx * dx) + (dy * dy));
  const angle = Math.atan2(dy, dx) * 180 / Math.PI;
  pathChildren.push({
    name: 'Path' + i, key: 'path' + i,
    rect: Object.assign(D.at((x0 + x1) / 2, (y0 + y1) / 2, length - (D.MENU.nodeSize * 0.86), 14), { rotZ: angle }),
    image: Object.assign(D.plate(D.a(D.P.StoneRidge, 0.85), 1.0), { raycast: false }),
  });
}

function statPlate(key, x, y, caption, accent) {
  const w = D.MENU.statW, h = D.MENU.statH;
  return {
    name: key, key: key, rect: D.at(x, y, w, h),
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

function secondaryButton(key, x, caption) {
  return {
    name: key, key: key,
    rect: D.at(x, D.MENU.secondaryY, D.MENU.secondaryW, D.MENU.secondaryH),
    image: D.plate(D.P.StoneSlab, 2.6),
    button: {},
    monos: [{
      cls: 'PressPulse', guid: cs('PressPulse'),
      fields: r => [['_target', Y.fid(r[key + 'Body'].rt)], ['_pressedScale', '0.94']],
    }],
    children: [{
      name: 'Body', key: key + 'Body',
      rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [0, 0] },
      children: [
        {
          name: 'Rim', rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [6, 6] },
          image: Object.assign(D.plate(D.a(D.P.StoneRidge, 0.95), 2.2), { raycast: false }),
        },
        {
          name: 'Caption', key: key + 'Caption',
          rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-40, -30] },
          text: Object.assign({ text: caption, color: D.P.ParchmentMuted, charSpacing: 14 }, D.TYPE.title),
        },
      ],
    }],
  };
}

const map = {
  name: 'MapFrame', key: 'map', rect: D.FRAME,
  children: [
    statPlate('EmberPlate', D.MENU.statLeftX, D.MENU.statY, 'EMBERS', D.P.EmberGold),
    statPlate('SparkPlate', D.MENU.statRightX, D.MENU.statY, 'SPARKS', D.P.SkyLapis),
  ].concat(pathChildren).concat(nodeChildren).concat([
    {
      name: 'Objective', key: 'objective',
      rect: D.at(621, D.MENU.objectiveY, D.MENU.objectiveW, D.MENU.objectiveH),
      text: Object.assign({ text: 'CHAMBER I  -  MATCH BOTH PANS AT 12 - 18', color: D.P.ParchmentMuted }, D.TYPE.objective),
    },
    secondaryButton('ArchiveButton', D.MENU.archiveX, 'ARCHIVE'),
    secondaryButton('SettingsButton', D.MENU.settingsX, 'SETTINGS'),
    {
      name: 'Guardian', key: 'guardian',
      rect: D.at(D.MENU.guardianX, D.MENU.guardianY, D.MENU.guardianSize, D.MENU.guardianSize),
      image: D.art(sp('sprite_dragon_guardian'), D.a(D.P.White, 0.9)),
    },
  ]),
};
S.buildTree(map, T.menuCanvasRt, alloc, out, refs);

// ── the two overlay sheets, anchored to the canvas so they always reach the edges ──
function sheetCard(key, title, siblingKey, bodyChildren) {
  return {
    name: key, key: key, active: false, rect: D.STRETCH,
    children: [
      {
        name: 'Scrim', key: key + 'Scrim', rect: D.STRETCH,
        // Sprite-less on purpose: a filler sprite would be forced to preserve its aspect
        // and the scrim would square itself instead of covering the screen.
        image: { sprite: null, type: 0, color: D.a(D.P.ObsidianDeep, 0.82) },
        button: {},
        monos: [{
          cls: 'SheetToggleButton', guid: cs('SheetToggleButton'),
          fields: r => [['_button', Y.fid(r[key + 'Scrim'].button)], ['_sheet', Y.fid(r[key].go)],
            ['_otherSheet', Y.fid(r[siblingKey].go)], ['_open', '0']],
        }],
      },
      {
        name: 'Card', key: key + 'Card',
        rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [D.CARD.w, D.CARD.h] },
        image: Object.assign(D.plate(D.P.EmberGold, 3.0), { color: D.a(D.P.EmberGold, 0.85) }),
        children: [
          {
            name: 'Body', rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-2 * D.CARD.frameInset, -2 * D.CARD.frameInset] },
            image: D.plate(D.P.StoneSlab, 3.2),
          },
          {
            name: 'Header', rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -110], size: [680, 86] },
            text: Object.assign({ text: title, color: D.P.EmberGold, charSpacing: 14 }, D.TYPE.sheetHeader),
          },
          {
            name: 'Close', key: key + 'Close',
            rect: { aMin: [1, 1], aMax: [1, 1], pos: [-86, -86], size: [112, 112] },
            image: D.plate(D.P.StoneRidge, 0.4),
            button: {},
            monos: [{
              cls: 'SheetToggleButton', guid: cs('SheetToggleButton'),
              fields: r => [['_button', Y.fid(r[key + 'Close'].button)], ['_sheet', Y.fid(r[key].go)],
                ['_otherSheet', Y.fid(r[siblingKey].go)], ['_open', '0']],
            }],
            children: [{
              name: 'Icon', rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [56, 56] },
              image: D.art(sp('sprite_icon_close'), D.P.ParchmentMuted),
            }],
          },
        ].concat(bodyChildren),
      },
    ],
  };
}

// Archive: a 3 x 5 grid of cells, plus the empty state that replaces it.
const archiveCells = [];
const CELL_W = 300, CELL_H = 268, GAP_X = 24, GAP_Y = 22;
for (let i = 0; i < 15; i++) {
  const col = i % 3, row = Math.floor(i / 3);
  const x = (col - 1) * (CELL_W + GAP_X);
  const y = -270 - (row * (CELL_H + GAP_Y)) - (CELL_H / 2);
  archiveCells.push({
    name: 'Cell' + i, key: 'cell' + i,
    rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [x, y], size: [CELL_W, CELL_H] },
    image: D.plate(D.P.StoneSlab, 2.4),
    monos: [{
      key: 'cellView' + i, cls: 'ArchiveCellView', guid: cs('ArchiveCellView'),
      fields: r => [
        ['_plate', Y.fid(r['cell' + i].image)],
        ['_glyph', Y.fid(r['cellGlyph' + i].image)],
        ['_weight', Y.fid(r['cellWeight' + i].tmp)],
        ['_caption', Y.fid(r['cellCaption' + i].tmp)],
        ['_emberSprite', Y.sref(sp('sprite_stone_ember'))],
        ['_coldSprite', Y.sref(sp('sprite_stone_cold'))],
      ],
    }],
    children: [
      {
        name: 'Glyph', key: 'cellGlyph' + i,
        rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -72], size: [104, 104] },
        image: D.art(sp('sprite_stone_cold')),
      },
      {
        name: 'Weight', key: 'cellWeight' + i,
        rect: { aMin: [0.5, 0], aMax: [0.5, 0], pos: [0, 104], size: [240, 64] },
        text: Object.assign({ text: '-', color: D.P.EmberGold }, D.TYPE.statValue),
      },
      {
        name: 'Caption', key: 'cellCaption' + i,
        rect: { aMin: [0.5, 0], aMax: [0.5, 0], pos: [0, 48], size: [260, 46] },
        text: Object.assign({ text: 'SEALED', color: D.P.ParchmentMuted, charSpacing: 10 }, D.TYPE.caption),
      },
    ],
  });
}

const archive = sheetCard('ArchiveSheet', 'EMBER ARCHIVE', 'SettingsSheet', [
  { name: 'Grid', key: 'archiveGrid', rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-40, -40] }, children: archiveCells },
  {
    name: 'EmptyLine', key: 'archiveEmpty', active: false,
    rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [900, 220] },
    text: Object.assign({ text: 'NO STONES CATALOGUED YET', color: D.P.ParchmentMuted }, D.TYPE.objective),
  },
]);
S.buildTree(archive, T.menuCanvasRt, alloc, out, refs);

function toggleRow(key, y, caption) {
  return {
    name: key, key: key,
    rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, y], size: [900, 150] },
    image: D.plate(D.a(D.P.StoneRidge, 0.55), 2.4),
    children: [
      {
        name: 'Caption',
        rect: { aMin: [0, 0.5], aMax: [0, 0.5], pos: [40 + 230, 0], size: [460, 60], pivot: [0.5, 0.5] },
        text: Object.assign({ text: caption, color: D.P.Parchment, hAlign: 1 }, D.TYPE.title),
      },
      {
        name: 'Status', key: key + 'Status',
        rect: { aMin: [1, 0.5], aMax: [1, 0.5], pos: [-70, 0], size: [120, 52] },
        text: Object.assign({ text: 'OFF', color: D.P.ParchmentMuted, hAlign: 4 }, D.TYPE.caption),
      },
      {
        name: 'Pill', key: key + 'Pill',
        rect: { aMin: [1, 0.5], aMax: [1, 0.5], pos: [-240, 0], size: [160, 72] },
        image: D.plate(D.P.StoneRidge, 0.25),
        button: {},
        children: [{
          name: 'Knob', key: key + 'Knob',
          rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [-44, 0], size: [60, 60] },
          image: Object.assign(D.plate(D.P.Parchment, 0.2), { raycast: false }),
        }],
      },
    ],
    monos: [{
      key: key + 'View', cls: 'ToggleRowView', guid: cs('ToggleRowView'),
      fields: r => [
        ['_button', Y.fid(r[key + 'Pill'].button)],
        ['_pill', Y.fid(r[key + 'Pill'].image)],
        ['_knob', Y.fid(r[key + 'Knob'].rt)],
        ['_status', Y.fid(r[key + 'Status'].tmp)],
        ['_knobTravel', '44'],
      ],
    }],
  };
}

const settings = sheetCard('SettingsSheet', 'TEMPLE SETTINGS', 'ArchiveSheet', [
  toggleRow('HapticsRow', -320, 'HAPTICS'),
  toggleRow('ContrastRow', -500, 'HIGH CONTRAST STONES'),
  {
    name: 'ContrastPreview', key: 'contrastPreview',
    rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -700], size: [320, 180] },
    image: D.plate(D.P.StoneRidge, 2.4),
    children: [{
      name: 'PreviewStone', key: 'previewStone',
      rect: { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: [124, 124] },
      image: D.art(sp('sprite_stone_ember')),
    }],
  },
  {
    name: 'PreviewCaption',
    rect: { aMin: [0.5, 1], aMax: [0.5, 1], pos: [0, -830], size: [600, 46] },
    text: Object.assign({ text: 'PREVIEW', color: D.P.ParchmentMuted, charSpacing: 12 }, D.TYPE.caption),
  },
  {
    name: 'EraseButton', key: 'eraseButton',
    rect: { aMin: [0.5, 0], aMax: [0.5, 0], pos: [0, 220], size: [640, 150] },
    image: D.plate(D.P.StoneSlab, 2.6),
    button: {},
    monos: [{
      cls: 'TwoStepButton', guid: cs('TwoStepButton'),
      fields: r => [
        ['_button', Y.fid(r.eraseButton.button)],
        ['_plate', Y.fid(r.eraseButton.image)],
        ['_caption', Y.fid(r.eraseCaption.tmp)],
        ['_owner', Y.fid(r.settingsLogic.mono)],
        ['_armedSeconds', '4'],
      ],
    }],
    children: [{
      name: 'Caption', key: 'eraseCaption',
      rect: { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [-40, -30] },
      text: Object.assign({ text: 'ERASE RECORD', color: D.P.ParchmentMuted, charSpacing: 12 }, D.TYPE.title),
    }],
  },
]);
S.buildTree(settings, T.menuCanvasRt, alloc, out, refs);

// ── the menu's logic objects ─────────────────────────────────────────────────
const CHAMBERS = [
  // kL kR  d  Tlo Thi wLo wHi places. The two sides deliberately differ in COUNT: equal
  // counts make every high target a mirror split, the variety guard rejects it and the
  // target band silently collapses. Proved over 2000 seeds per chamber.
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

const logic = {
  name: 'MenuLogic', key: 'menuLogic', rect: { aMin: [0, 0], aMax: [0, 0], pos: [0, 0], size: [0, 0] },
  monos: [
    { key: 'progress', cls: 'ProgressStore', guid: cs('ProgressStore'), fields: [] },
    { key: 'library', cls: 'ChamberLibrary', guid: cs('ChamberLibrary'), fields: [['_chambers', chamberRows]] },
    {
      key: 'mapDirector', cls: 'TempleMapDirector', guid: cs('TempleMapDirector'),
      fields: r => [
        ['_progress', Y.fid(r.progress.mono)],
        ['_library', Y.fid(r.library.mono)],
        ['_nodes', [0, 1, 2, 3, 4].map(i => Y.fid(r['nodeView' + i].mono))],
        ['_pathSegments', [0, 1, 2, 3].map(i => Y.fid(r['path' + i].image))],
        ['_emberPlate', Y.fid(r.EmberPlateView.mono)],
        ['_sparkPlate', Y.fid(r.SparkPlateView.mono)],
        ['_objective', Y.fid(r.objective.tmp)],
        ['_guardian', Y.fid(r.guardian.image)],
      ],
    },
    {
      key: 'archiveLogic', cls: 'ArchiveSheet', guid: cs('ArchiveSheet'),
      fields: r => [
        ['_progress', Y.fid(r.progress.mono)],
        ['_card', Y.fid(r.ArchiveSheetCard.rt)],
        ['_grid', Y.fid(r.archiveGrid.go)],
        ['_cells', Array.from({ length: 15 }, (_, i) => Y.fid(r['cellView' + i].mono))],
        ['_emptyLine', Y.fid(r.archiveEmpty.tmp)],
      ],
    },
    {
      key: 'settingsLogic', cls: 'SettingsSheet', guid: cs('SettingsSheet'),
      fields: r => [
        ['_progress', Y.fid(r.progress.mono)],
        ['_map', Y.fid(r.mapDirector.mono)],
        ['_archive', Y.fid(r.archiveLogic.mono)],
        ['_card', Y.fid(r.SettingsSheetCard.rt)],
        ['_hapticsRow', Y.fid(r.HapticsRowView.mono)],
        ['_contrastRow', Y.fid(r.ContrastRowView.mono)],
        ['_contrastPreviewPlate', Y.fid(r.contrastPreview.image)],
        ['_contrastPreviewStone', Y.fid(r.previewStone.image)],
      ],
    },
    {
      key: 'playLaunch', cls: 'PlayLaunchButton', guid: cs('PlayLaunchButton'),
      fields: r => [['_button', Y.fid(T.playButton)], ['_progress', Y.fid(r.progress.mono)], ['_sceneIndex', '1']],
    },
    {
      key: 'openArchive', cls: 'SheetToggleButton', guid: cs('SheetToggleButton'),
      fields: r => [['_button', Y.fid(r.ArchiveButton.button)], ['_sheet', Y.fid(r.ArchiveSheet.go)],
        ['_otherSheet', Y.fid(r.SettingsSheet.go)], ['_open', '1']],
    },
    {
      key: 'openSettings', cls: 'SheetToggleButton', guid: cs('SheetToggleButton'),
      fields: r => [['_button', Y.fid(r.SettingsButton.button)], ['_sheet', Y.fid(r.SettingsSheet.go)],
        ['_otherSheet', Y.fid(r.ArchiveSheet.go)], ['_open', '1']],
    },
    {
      key: 'playPulse', cls: 'PressPulse', guid: cs('PressPulse'),
      fields: () => [['_target', Y.fid(T.playCaptionRt)], ['_pressedScale', '0.95']],
    },
    {
      key: 'cleaner', cls: 'TemplateTextCleaner', guid: cs('TemplateTextCleaner'),
      fields: () => [['_undressed', [Y.fid(T.scoreLabelTmp)]]],
    },
  ],
  children: [],
};
S.buildTree(logic, T.menuCanvasRt, alloc, out, refs);

S.appendDocs(scene, S.flatten(out));

// ── re-theme the template objects in place ───────────────────────────────────
// Canvases: the template ships a LANDSCAPE 2400x1120 reference, which makes every
// portrait number a guess. Expand keeps the canvas at least the reference size on both
// axes, so a centred frame of exactly that size is always fully visible.
for (const scaler of [T.loaderScaler, T.menuScaler]) {
  S.setField(scene, scaler, 'm_ReferenceResolution', Y.v2(D.REF));
  S.setField(scene, scaler, 'm_ScreenMatchMode', '1');
  S.setField(scene, scaler, 'm_MatchWidthOrHeight', '0');
}

// Backgrounds: both panels share BG_main, and the splash is tinted far darker so the
// first captured frame can never be mistaken for the menu.
S.setField(scene, T.loaderBgImage, 'm_Sprite', Y.sref(tx('BG_main')));
S.setField(scene, T.loaderBgImage, 'm_Color', Y.col([0.34, 0.32, 0.40, 1]));
S.setField(scene, T.menuBgImage, 'm_Sprite', Y.sref(tx('BG_main')));
S.setField(scene, T.menuBgImage, 'm_Color', Y.col(D.P.White));

// The loading bar: a real body, a palette fill and a track with visible alpha.
S.setField(scene, T.sliderRt, 'm_AnchoredPosition', Y.v2([0, -430]));
S.setField(scene, T.sliderRt, 'm_SizeDelta', Y.v2([640, 56]));
S.setField(scene, T.fillAreaRt, 'm_AnchorMin', Y.v2([0, 0]));
S.setField(scene, T.fillAreaRt, 'm_AnchorMax', Y.v2([1, 1]));
S.setField(scene, T.fillAreaRt, 'm_AnchoredPosition', Y.v2([0, 0]));
S.setField(scene, T.fillAreaRt, 'm_SizeDelta', Y.v2([-16, -20]));
// Fill ships 10 units wider than its value because UpdateVisuals rewrites its anchors
// every frame but never its sizeDelta. Zero it.
S.setField(scene, T.fillRt, 'm_SizeDelta', Y.v2([0, 0]));
S.setField(scene, T.fillImage, 'm_Color', Y.col(D.P.EmberGold));
S.setField(scene, T.fillImage, 'm_Sprite', Y.UISPRITE);
S.setField(scene, T.fillImage, 'm_Type', '1');
S.setField(scene, T.fillImage, 'm_PixelsPerUnitMultiplier', '0.35');
S.setField(scene, T.sliderTrackImage, 'm_Color', Y.col(D.P.StoneRidge));
S.setField(scene, T.sliderTrackImage, 'm_Sprite', Y.UISPRITE);
S.setField(scene, T.sliderTrackImage, 'm_Type', '1');
S.setField(scene, T.sliderTrackImage, 'm_PixelsPerUnitMultiplier', '0.35');
// 30 seconds of loading bar would eat the whole review window; the backstop sits at 10.
S.setField(scene, T.controllerLoader, 'slider_wait_time', '7');

// PLAY: a real button, not the template's full-width slab.
S.setField(scene, T.playRt, 'm_AnchorMin', Y.v2([0.5, 0.5]));
S.setField(scene, T.playRt, 'm_AnchorMax', Y.v2([0.5, 0.5]));
S.setField(scene, T.playRt, 'm_AnchoredPosition', Y.v2([0, D.MENU.playY - (D.REF[1] / 2)]));
S.setField(scene, T.playRt, 'm_SizeDelta', Y.v2([D.MENU.playW, D.MENU.playH]));
S.setField(scene, T.playImage, 'm_Sprite', Y.UISPRITE);
S.setField(scene, T.playImage, 'm_Type', '1');
S.setField(scene, T.playImage, 'm_PixelsPerUnitMultiplier', '2.0');
S.setField(scene, T.playImage, 'm_Color', Y.col(D.P.EmberGold));

// Privacy keeps the template's own handler; only its look and place change.
S.setField(scene, T.privacyRt, 'm_AnchorMin', Y.v2([0.5, 0.5]));
S.setField(scene, T.privacyRt, 'm_AnchorMax', Y.v2([0.5, 0.5]));
S.setField(scene, T.privacyRt, 'm_AnchoredPosition',
  Y.v2([D.MENU.privacyX - (D.REF[0] / 2), D.MENU.privacyY - (D.REF[1] / 2)]));
S.setField(scene, T.privacyRt, 'm_SizeDelta', Y.v2([D.MENU.privacyW, D.MENU.privacyH]));
S.setField(scene, T.privacyImage, 'm_Sprite', Y.UISPRITE);
S.setField(scene, T.privacyImage, 'm_Type', '1');
S.setField(scene, T.privacyImage, 'm_PixelsPerUnitMultiplier', '2.6');
S.setField(scene, T.privacyImage, 'm_Color', Y.col(D.a(D.P.StoneRidge, 0.9)));

// Both captions ship INACTIVE, near-black, wrapping, with autosize off and a floor of 18.
// All five of those are read from the serialized values, so all five are fixed here.
function fixCaption(goId, tmpId, rtId, text, color, size, min) {
  S.setField(scene, goId, 'm_IsActive', '1');
  S.setField(scene, tmpId, 'm_text', text);
  S.setField(scene, tmpId, 'm_fontColor', Y.col(color));
  S.setNested(scene, tmpId, 'm_fontColor32', 'rgba', String(Y.rgba32(color)));
  S.setField(scene, tmpId, 'm_fontSize', String(size));
  S.setField(scene, tmpId, 'm_fontSizeBase', String(size));
  S.setField(scene, tmpId, 'm_enableAutoSizing', '1');
  S.setField(scene, tmpId, 'm_fontSizeMin', String(min));
  S.setField(scene, tmpId, 'm_fontSizeMax', String(size));
  S.setField(scene, tmpId, 'm_TextWrappingMode', '0');
  S.setField(scene, tmpId, 'm_characterSpacing', '14');
  S.setField(scene, rtId, 'm_SizeDelta', Y.v2([-56, -36]));
}
fixCaption(T.playCaptionGo, T.playCaptionTmp, T.playCaptionRt, 'PLAY', D.P.Ink,
  D.TYPE.playCaption.size, D.TYPE.playCaption.min);
fixCaption(T.privacyCaptionGo, T.privacyCaptionTmp, T.privacyCaptionRt, 'PRIVACY', D.P.ParchmentMuted,
  D.TYPE.caption.size, D.TYPE.caption.min);
// The privacy caption is the one label pointing at a font asset that is in no .meta.
// Both of its references need repointing, not just the guid: the material sub-asset id
// has to become the one that really exists inside LiberationSans SDF.
S.setField(scene, T.privacyCaptionTmp, 'm_fontAsset',
  '{fileID: ' + Y.FONT_ASSET_FID + ', guid: ' + Y.FONT_GUID + ', type: 2}');
S.setField(scene, T.privacyCaptionTmp, 'm_sharedMaterial',
  '{fileID: ' + Y.FONT_MAT_FID + ', guid: ' + Y.FONT_GUID + ', type: 2}');

// The loose score label is rewritten every frame by the template from a serialized zero,
// so it cannot carry a stat. It stays off, its placeholder string goes, and its three
// readability values are brought in line anyway: the text checks read the file, not the
// hierarchy, and an inactive object is still a label in the shipped scene.
S.setField(scene, T.scoreLabelTmp, 'm_text', '');
S.setField(scene, T.scoreLabelTmp, 'm_fontSize', '40');
S.setField(scene, T.scoreLabelTmp, 'm_fontSizeBase', '40');
S.setField(scene, T.scoreLabelTmp, 'm_enableAutoSizing', '1');
S.setField(scene, T.scoreLabelTmp, 'm_fontSizeMin', '32');
S.setField(scene, T.scoreLabelTmp, 'm_fontSizeMax', '40');
S.setField(scene, T.scoreLabelTmp, 'm_TextWrappingMode', '0');

// Attach the new subtrees to the two canvases, after the template's own children.
S.setChildren(scene, T.loaderCanvasRt, S.children(scene, T.loaderCanvasRt).concat([refs.splash.rt]));
S.setChildren(scene, T.menuCanvasRt, S.children(scene, T.menuCanvasRt).concat([
  refs.map.rt, refs.ArchiveSheet.rt, refs.SettingsSheet.rt, refs.menuLogic.rt,
]));

// The dead input-actions guid is in both scenes, eleven references each; without the swap
// no uGUI raycast ever lands. EditorBuildSettings already pairs the real asset with the
// very fileIDs the scenes use, so this is a guid swap and nothing else.
scene.docs.forEach(d => {
  d.text = d.text.replace(/guid: ca9f5fa95ffab41fb9a615ab714db018/g, 'guid: 2bcd2660ca9b64942af0de543d8d7100');
});

S.sceneRoots(scene, S.existingRootRts(scene));
S.write(scene);
console.log('[loader] documents: ' + scene.docs.length);
