'use strict';
// Writes assets.json and screenshot-scenario.json.
//
// Every tapFrac is DERIVED from the rect the scene really holds, not estimated. Under the
// Expand scaler the canvas is the screen divided by min(sw/1242, sh/2688), so on the
// 1080x2400 capture device the canvas is exactly 1242x2760 and the centred 1242x2688 frame
// sits 36 units above the bottom. Targets anchored to the canvas edge (the top bar) are
// computed from that edge instead, which keeps them on target at other aspects too.
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const REF_W = 1242, REF_H = 2688;
const DEVICE = [1080, 2400];
const scale = Math.min(DEVICE[0] / REF_W, DEVICE[1] / REF_H);
const CANVAS_W = DEVICE[0] / scale;
const CANVAS_H = DEVICE[1] / scale;
const FRAME_X = (CANVAS_W - REF_W) / 2;
const FRAME_Y = (CANVAS_H - REF_H) / 2;

const round = n => Math.round(n * 1000) / 1000;
// A point inside the centred 1242x2688 frame, measured from its bottom-left corner.
const frame = (x, y) => [round((FRAME_X + x) / CANVAS_W), round(1 - ((FRAME_Y + y) / CANVAS_H))];
// A point pinned to the canvas's top edge: dx from the named side, dy below the top.
const fromTop = (side, dx, dy) => [
  round(side === 'right' ? 1 - (dx / CANVAS_W) : (side === 'centre' ? 0.5 : dx / CANVAS_W)),
  round(dy / CANVAS_H),
];

// ── board numbers, the same ones ChamberLayout.cs uses ───────────────────────
const CENTRE_X = 621;
const PAN_X = 320;
const PAN_REST_Y = 1480;
const TRAY_SPAN = 1100;
const TRAY_ROW_TOP = 980;
// Chamber I is what a fresh install plays, and its tray is exactly six stones.
const CHAMBER_ONE_STONES = 6;
const cols = Math.ceil(CHAMBER_ONE_STONES / 2);
const pitch = TRAY_SPAN / cols;
const traySlot = index => {
  const column = index % cols;
  return [CENTRE_X + ((column - ((cols - 1) * 0.5)) * pitch), TRAY_ROW_TOP];
};

const MENU_PLAY = [621, 790];

const scenario = {
  steps: [
    // The runner already blocks on the app's own loader before the FIRST capturing step,
    // so this frame is the menu and there is no splash shot to ask for. Carrying
    // waitLoader here only adds its own eight-second sleep on top.
    { wait: 2, shot: '01_TempleMap' },
    // A scene swap, so waitLoader - it settles on the frame CHANGING, which works even
    // though the game scene carries no loading bar of its own.
    { tapFrac: frame(MENU_PLAY[0], MENU_PLAY[1]), label: 'PLAY', waitLoader: true, shot: '02_BalancePuzzle' },
    { tapFrac: frame(traySlot(0)[0], traySlot(0)[1]), label: 'STONE_LEFT', wait: 2, shot: '03_StoneLifted' },
    { tapFrac: frame(CENTRE_X - PAN_X, PAN_REST_Y), label: 'LEFT_PAN', wait: 3, shot: '04_LeftPanLoaded' },
    { tapFrac: frame(traySlot(2)[0], traySlot(2)[1]), label: 'STONE_RIGHT', wait: 2, shot: '05_SecondStoneLifted' },
    { tapFrac: frame(CENTRE_X + PAN_X, PAN_REST_Y), label: 'RIGHT_PAN', wait: 3, shot: '06_BeamLeaning' },
    { tapFrac: frame(870, 390), label: 'SPARK', wait: 3, shot: '07_SparkHint' },
    { tapFrac: fromTop('right', 110, 194), label: 'PAUSE', wait: 3, shot: '08_ScalesHeld' },
  ],
};

fs.writeFileSync(path.join(ROOT, 'screenshot-scenario.json'), JSON.stringify(scenario, null, 2) + '\n');

// ── art manifest ─────────────────────────────────────────────────────────────
// One style for every entry, named in every prompt. No photographic vocabulary anywhere:
// that is what puts a stock photograph into a shipped menu. Facing is declared for every
// sprite that reads as directional, because without it neither generation nor the
// orientation check nor the auto-fix has anything to work from.
const STYLE = 'hand-painted storybook gouache, warm ember light on dark carved stone, visible brush texture, chunky readable silhouette';
const SPRITES = [
  ['sprite_emblem_scale', 'one single carved temple emblem: a balance beam with a glowing ember disc at its pivot, centred, isolated, crisp silhouette, no letters, no text', 'symmetric'],
  ['sprite_scale_post', 'one single ancient bronze scale column standing on a carved stone base, front view, centred, isolated', 'symmetric, vertical'],
  ['sprite_pivot_dragon', 'one single round bronze medallion with a carved dragon head facing forward, front view, centred, isolated', 'symmetric'],
  ['sprite_pan', 'one single shallow bronze offering pan with a scalloped rim, three-quarter front view, centred, isolated', 'symmetric'],
  ['sprite_stone_ember', 'one single rounded ember stone with a glowing amber core and a carved rune on its face, centred, isolated', 'symmetric'],
  ['sprite_stone_cold', 'one single rounded dark unlit stone with a faint carved rune, centred, isolated', 'symmetric'],
  ['sprite_spark', 'one single small amber flame wisp, centred, isolated', 'symmetric'],
  ['sprite_dragon_guardian', 'one single coiled stone-and-bronze guardian dragon, side view, facing RIGHT, centred, isolated', 'facing RIGHT'],
  ['sprite_chamber_node', 'one single round carved stone seal medallion with a bronze rim, front view, centred, isolated', 'symmetric'],
  ['sprite_rune_lock', 'one single crimson rune padlock glyph, front view, centred, isolated', 'symmetric'],
  ['sprite_pan_glow', 'one single soft round amber glow halo, centred, isolated, nothing inside it', 'symmetric'],
  ['sprite_icon_close', 'one single bronze cross glyph with thick strokes, centred, isolated', 'symmetric'],
  ['sprite_icon_back', 'one single bronze chevron arrow glyph pointing LEFT, centred, isolated', 'facing LEFT'],
  ['sprite_icon_pause', 'one single bronze pause glyph of two thick vertical bars, centred, isolated', 'symmetric'],
];

const assets = [];
for (const [name, core, facing] of SPRITES) {
  assets.push({
    name,
    type: 'sprite',
    transparent: true,
    target: 'Assets/Art/Sprites/' + name + '.png',
    facing,
    prompt: core + ', ' + STYLE,
  });
}
assets.push({
  name: 'BG_main',
  type: 'bg',
  transparent: false,
  target: 'Assets/Art/Textures/_BASE/Backgrounds/BG_main.png',
  prompt: 'vertical 19.5:9 portrait background of a dark temple sanctum, obsidian walls with carved dragon reliefs, ' +
    'a brazier pooling amber light along the bottom third, deliberately low contrast through the central band, ' +
    'no text, no UI elements, ' + STYLE,
});
assets.push({
  name: 'BG_game',
  type: 'bg',
  transparent: false,
  target: 'Assets/Art/Textures/_BASE/Backgrounds/BG_game.png',
  prompt: 'vertical 19.5:9 portrait background of a deeper temple chamber, darker, a soft vertical vignette, ' +
    'faint carved rune columns down the left and right edges, a clean low-contrast band through the middle, ' +
    'no text, no UI elements, ' + STYLE,
});

const manifest = {
  style: STYLE,
  icon: {
    // Built after the sprites, from this game's own art: one hero object, never a scene,
    // and no lettering of any kind.
    hero: 'sprite_stone_ember',
    target: 'Assets/Art/Textures/game_icon.png',
    prompt: 'a single glowing ember stone resting on an ancient bronze offering pan, dark temple chamber, ' +
      'warm amber rim light, one hero object, no text, no letters, ' + STYLE,
  },
  assets,
};
fs.writeFileSync(path.join(ROOT, 'assets.json'), JSON.stringify(manifest, null, 2) + '\n');

console.log('[manifests] assets.json: ' + assets.length + ' entries');
console.log('[manifests] canvas at ' + DEVICE.join('x') + ' = ' + CANVAS_W.toFixed(0) + 'x' + CANVAS_H.toFixed(0) +
  ', frame offset ' + FRAME_X.toFixed(0) + ',' + FRAME_Y.toFixed(0));
for (const s of scenario.steps) {
  if (s.tapFrac) console.log('  ' + s.label.padEnd(12) + ' -> [' + s.tapFrac.join(', ') + ']  ' + s.shot);
}
