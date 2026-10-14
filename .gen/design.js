'use strict';
// The palette and the composition numbers, shared by both scene builders so the two
// screens cannot drift apart. Colours are 0..1 floats, matching EmberPalette.cs exactly.

const hex = h => {
  const n = parseInt(h.slice(1), 16);
  return [((n >> 16) & 255) / 255, ((n >> 8) & 255) / 255, (n & 255) / 255, 1];
};
const a = (c, alpha) => [c[0], c[1], c[2], alpha];

const P = {
  Obsidian: hex('#1D1A22'),
  ObsidianDeep: hex('#141118'),
  StoneSlab: hex('#2A2531'),
  StoneRidge: hex('#3A3342'),
  EmberGold: hex('#FFB703'),
  EmberGoldSoft: hex('#FFD56B'),
  DragonCrimson: hex('#D1495B'),
  CrimsonDeep: hex('#8E2F3C'),
  SkyLapis: hex('#4EA8DE'),
  LapisDeep: hex('#2B6B92'),
  Parchment: hex('#FFF4E6'),
  ParchmentMuted: hex('#C9BCA9'),
  Ink: hex('#171320'),
  White: [1, 1, 1, 1],
};

// Canvas reference. Expand keeps the canvas at least this big on both axes, so a
// centred frame of exactly this size is always fully on screen.
const REF = [1242, 2688];
const FRAME = { aMin: [0.5, 0.5], aMax: [0.5, 0.5], pos: [0, 0], size: REF };
const STRETCH = { aMin: [0, 0], aMax: [1, 1], pos: [0, 0], size: [0, 0] };

// A child of the frame, positioned from the frame's bottom-left corner.
const at = (x, y, w, h, extra) => Object.assign({
  aMin: [0, 0], aMax: [0, 0], pivot: [0.5, 0.5], pos: [x, y], size: [w, h],
}, extra || {});

// A child positioned from the canvas centre (used where the template already anchors there).
const mid = (x, y, w, h) => ({ aMin: [0.5, 0.5], aMax: [0.5, 0.5], pivot: [0.5, 0.5], pos: [x, y], size: [w, h] });

// Type 1 = Sliced. The builtin UI plate is a real nine-slice, and
// m_PixelsPerUnitMultiplier is the corner-radius lever: lower is rounder.
const plate = (color, ppu, sprite) => ({ sprite: sprite || 'UISPRITE', type: 1, color, ppu: ppu == null ? 3.2 : ppu });
// A content sprite always lives in a SQUARE rect with preserveAspect, so the generated
// 512x512 PNG can never be drawn as an ellipse.
const art = (guid, color) => ({ sprite: guid, type: 0, preserveAspect: true, color: color || P.White, raycast: false });

const TYPE = {
  popHeader: { size: 96, min: 56 },
  playCaption: { size: 76, min: 52 },
  sheetHeader: { size: 64, min: 46 },
  nodeNumeral: { size: 64, min: 44 },
  statValue: { size: 56, min: 40 },
  popBody: { size: 54, min: 40 },
  title: { size: 52, min: 38 },
  chip: { size: 46, min: 34 },
  objective: { size: 44, min: 34 },
  popExtra: { size: 42, min: 34 },
  hint: { size: 38, min: 32 },
  caption: { size: 34, min: 32 },
};

// Menu composition, y measured from the bottom of the 2688 frame.
const MENU = {
  statY: 2500, statW: 480, statH: 150, statLeftX: 330, statRightX: 912,
  nodeX: [420, 822, 420, 822, 420],
  nodeY: [1270, 1520, 1770, 2020, 2270],
  nodeSize: 196,
  objectiveY: 1090, objectiveW: 1000, objectiveH: 64,
  playX: 621, playY: 790, playW: 760, playH: 168,
  secondaryY: 560, secondaryW: 480, secondaryH: 144,
  archiveX: 330, settingsX: 912,
  privacyX: 880, privacyY: 330, privacyW: 560, privacyH: 104,
  guardianX: 230, guardianY: 250, guardianSize: 340,
};

// Card chrome shared by the two menu sheets and the three result cards.
const CARD = { w: 1040, h: 1760, frameInset: 12 };

// Stat plate interior: the pip lives alone in its gutter and the text column starts
// clear of it, so the two never share an x-band (rule C.25).
const PLATE = {
  gutterCentre: 62, gutterWidth: 42,
  textLeft: 62 + 21 + 46,     // 129
  textWidth: 220,             // 129 + 220 = 349 <= 372 - 16 on the game plates
};

module.exports = { P, a, hex, REF, FRAME, STRETCH, at, mid, plate, art, TYPE, MENU, CARD, PLATE };
