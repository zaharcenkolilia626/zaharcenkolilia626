'use strict';
// Write the .cs.meta / .prefab.meta / .png.meta for everything codegen owns.
// The PNG arrives later; the guid written here is the guid that ships.
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const guids = JSON.parse(fs.readFileSync(path.join(__dirname, 'guids.json'), 'utf8'));

function scriptMeta(guid) {
  return 'fileFormatVersion: 2\nguid: ' + guid + '\nMonoImporter:\n' +
    '  externalObjects: {}\n  serializedVersion: 2\n  defaultReferences: []\n' +
    '  executionOrder: 0\n  icon: {instanceID: 0}\n  userData: \n' +
    '  assetBundleName: \n  assetBundleVariant: \n';
}

function prefabMeta(guid) {
  return 'fileFormatVersion: 2\nguid: ' + guid + '\nPrefabImporter:\n' +
    '  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n';
}

// Field names and order copied from a texture .meta this template family already ships.
// spriteID is left empty on purpose: Unity issues one per asset, and reusing another
// asset's value would hand every sprite the same id.
function textureMeta(guid, opts) {
  const border = opts.border || [0, 0, 0, 0];
  const alphaIsTransparency = opts.transparent ? 1 : 0;
  return 'fileFormatVersion: 2\nguid: ' + guid + '\nTextureImporter:\n' +
    '  internalIDToNameTable: []\n  externalObjects: {}\n  serializedVersion: 13\n' +
    '  mipmaps:\n    mipMapMode: 0\n    enableMipMap: 0\n    sRGBTexture: 1\n    linearTexture: 0\n' +
    '    fadeOut: 0\n    borderMipMap: 0\n    mipMapsPreserveCoverage: 0\n    alphaTestReferenceValue: 0.5\n' +
    '    mipMapFadeDistanceStart: 1\n    mipMapFadeDistanceEnd: 3\n' +
    '  bumpmap:\n    convertToNormalMap: 0\n    externalNormalMap: 0\n    heightScale: 0.25\n    normalMapFilter: 0\n' +
    '    flipGreenChannel: 0\n' +
    '  isReadable: 0\n  streamingMipmaps: 0\n  streamingMipmapsPriority: 0\n  vTOnly: 0\n' +
    '  ignoreMipmapLimit: 0\n  grayScaleToAlpha: 0\n  generateCubemap: 6\n  cubemapConvolution: 0\n' +
    '  seamlessCubemap: 0\n  textureFormat: 1\n  maxTextureSize: 2048\n' +
    '  textureSettings:\n    serializedVersion: 2\n    filterMode: 1\n    aniso: 1\n    mipBias: 0\n' +
    '    wrapU: 1\n    wrapV: 1\n    wrapW: 1\n' +
    '  nPOTScale: 0\n  lightmap: 0\n  compressionQuality: 50\n' +
    '  spriteMode: 1\n  spriteExtrude: 1\n  spriteMeshType: 0\n  alignment: 0\n' +
    '  spritePivot: {x: 0.5, y: 0.5}\n  spritePixelsToUnits: 100\n' +
    '  spriteBorder: {x: ' + border[0] + ', y: ' + border[1] + ', z: ' + border[2] + ', w: ' + border[3] + '}\n' +
    '  spriteGenerateFallbackPhysicsShape: 0\n  alphaUsage: 1\n' +
    '  alphaIsTransparency: ' + alphaIsTransparency + '\n  spriteTessellationDetail: -1\n' +
    '  textureType: 8\n  textureShape: 1\n  singleChannelComponent: 0\n' +
    '  flipbookRows: 1\n  flipbookColumns: 1\n  maxTextureSizeSet: 0\n  compressionQualitySet: 0\n' +
    '  textureFormatSet: 0\n  ignorePngGamma: 0\n  applyGammaDecoding: 0\n  swizzle: 50462976\n' +
    '  cookieLightType: 0\n  platformSettings:\n  - serializedVersion: 4\n    buildTarget: DefaultTexturePlatform\n' +
    '    maxTextureSize: 2048\n    resizeAlgorithm: 0\n    textureFormat: -1\n    textureCompression: 1\n' +
    '    compressionQuality: 50\n    crunchedCompression: 0\n    allowsAlphaSplitting: 0\n' +
    '    overridden: 0\n    ignorePlatformSupport: 0\n    androidETC2FallbackOverride: 0\n    forceMaximumCompressionQuality_BC6H_BC7: 0\n' +
    '  - serializedVersion: 4\n    buildTarget: Android\n    maxTextureSize: 2048\n    resizeAlgorithm: 0\n' +
    '    textureFormat: -1\n    textureCompression: 1\n    compressionQuality: 50\n    crunchedCompression: 0\n' +
    '    allowsAlphaSplitting: 0\n    overridden: 0\n    ignorePlatformSupport: 0\n    androidETC2FallbackOverride: 0\n' +
    '    forceMaximumCompressionQuality_BC6H_BC7: 0\n' +
    '  spriteSheet:\n    serializedVersion: 2\n    sprites: []\n    outline: []\n    physicsShape: []\n' +
    '    bones: []\n    spriteID: \n    internalID: 0\n    vertices: []\n    indices: \n    edges: []\n    weights: []\n' +
    '    secondaryTextures: []\n  spritePackingTag: \n  pSDRemoveMatte: 0\n' +
    '  userData: \n  assetBundleName: \n  assetBundleVariant: \n';
}

function folderMeta(guid) {
  return 'fileFormatVersion: 2\nguid: ' + guid + '\nfolderAsset: yes\nDefaultImporter:\n' +
    '  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n';
}

const SPRITE_DIR = 'Assets/Art/Sprites';
const BG_DIR = 'Assets/Art/Textures/_BASE/Backgrounds';
// The two plates are the only sprites used nine-sliced on a UGUI Image.
const BORDERS = {};

let written = 0;
for (const [name, guid] of Object.entries(guids)) {
  const kind = name.slice(0, 3);
  const base = name.slice(3);
  let file = null;
  let body = null;
  if (kind === 'cs:') {
    file = path.join(ROOT, 'Assets/Scripts/Generated', base + '.cs.meta');
    body = scriptMeta(guid);
  } else if (kind === 'pf:') {
    file = path.join(ROOT, 'Assets/Prefabs/Generated', base + '.prefab.meta');
    body = prefabMeta(guid);
  } else if (kind === 'sp:') {
    file = path.join(ROOT, SPRITE_DIR, base + '.png.meta');
    body = textureMeta(guid, { transparent: true, border: BORDERS[base] });
  } else if (kind === 'tx:') {
    const dir = base === 'game_icon' ? 'Assets/Art/Textures' : BG_DIR;
    file = path.join(ROOT, dir, base + '.png.meta');
    body = textureMeta(guid, { transparent: base === 'game_icon' });
  }

  if (!file) continue;
  fs.mkdirSync(path.dirname(file), { recursive: true });
  fs.writeFileSync(file, body);
  written++;
}

// Unity wants a .meta for every folder codegen adds.
const crypto = require('crypto');
const folders = ['Assets/Art', 'Assets/Art/Sprites', 'Assets/Art/Textures',
  'Assets/Art/Textures/_BASE', BG_DIR, 'Assets/Prefabs', 'Assets/Prefabs/Generated', 'Assets/Scripts/Generated'];
const taken = new Set(Object.values(guids));
for (const folder of folders) {
  const metaPath = path.join(ROOT, folder + '.meta');
  if (fs.existsSync(metaPath)) continue;
  let g;
  do { g = crypto.randomBytes(16).toString('hex'); } while (taken.has(g));
  taken.add(g);
  fs.mkdirSync(path.join(ROOT, folder), { recursive: true });
  fs.writeFileSync(metaPath, folderMeta(g));
  written++;
}

console.log('metas written: ' + written);
