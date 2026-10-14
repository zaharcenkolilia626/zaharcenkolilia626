'use strict';
// Emitters for Unity 6 serialized documents. One function per document type.
// Every emitter returns a full "--- !u!N &id\n<Type>:\n..." block.

const UGUI = {
  image:     'fe87c0e1cc204ed48ad3b37840f39efc',
  button:    '4e29b1a8efbd4b44bb3f3716e73f07ff',
  scaler:    '0cd44c1031e13a943bb63640046fad76',
  raycaster: 'dc42784cf147c0c48a680349fa168899',
  tmp:       'f4688fdb7df04437aeb418b961361dc5',
};
const FONT_GUID = '8f586378b4e144a9851e7b34d9b748ee';
const FONT_ASSET_FID = 11400000;
const FONT_MAT_FID = 2180264;
// Builtin UI plates (verified): 10905 UISprite, 10907 Background, 10913 Knob.
const BUILTIN = '0000000000000000f000000000000000';
const UISPRITE = '{fileID: 10905, guid: ' + BUILTIN + ', type: 0}';
const UIPANEL = '{fileID: 10907, guid: ' + BUILTIN + ', type: 0}';

const f = n => {
  const v = Number(n);
  if (!isFinite(v)) throw new Error('non-finite number: ' + n);
  return String(Math.round(v * 1e6) / 1e6);
};
const v2 = p => '{x: ' + f(p[0]) + ', y: ' + f(p[1]) + '}';
const col = c => '{r: ' + f(c[0]) + ', g: ' + f(c[1]) + ', b: ' + f(c[2]) + ', a: ' + f(c.length > 3 ? c[3] : 1) + '}';
const rgba32 = c => {
  const b = x => Math.max(0, Math.min(255, Math.round(x * 255)));
  const a = c.length > 3 ? c[3] : 1;
  return (b(c[0]) | (b(c[1]) << 8) | (b(c[2]) << 16) | (b(a) << 24)) >>> 0;
};
const sref = s => (s ? '{fileID: 21300000, guid: ' + s + ', type: 3}' : '{fileID: 0}');
const fid = i => '{fileID: ' + (i == null ? 0 : i) + '}';

const HEAD = (cls, id, type) =>
  '--- !u!' + cls + ' &' + id + '\n' + type + ':\n  m_ObjectHideFlags: 0\n' +
  '  m_CorrespondingSourceObject: {fileID: 0}\n  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n';

function gameObject(id, o) {
  const comps = (o.components || []).map(c => '  - component: {fileID: ' + c + '}').join('\n');
  return HEAD(1, id, 'GameObject') +
    '  serializedVersion: 6\n  m_Component:\n' + comps + '\n' +
    '  m_Layer: ' + (o.layer == null ? 5 : o.layer) + '\n  m_Name: ' + o.name + '\n  m_TagString: Untagged\n' +
    '  m_Icon: {fileID: 0}\n  m_NavMeshLayer: 0\n  m_StaticEditorFlags: 0\n' +
    '  m_IsActive: ' + (o.active === false ? 0 : 1) + '\n';
}

function rectTransform(id, o) {
  if (!Array.isArray(o.children)) throw new Error('rectTransform ' + id + ': children must be a materialised array');
  const kids = o.children.length ? '\n' + o.children.map(c => '  - {fileID: ' + c + '}').join('\n') + '\n' : ' []\n';
  const sc = o.scale || [1, 1, 1];
  const rotZ = o.rotZ || 0;
  const hr = Math.PI * rotZ / 360;
  const q = '{x: -0, y: -0, z: ' + f(Math.sin(hr)) + ', w: ' + f(Math.cos(hr)) + '}';
  return HEAD(224, id, 'RectTransform') +
    '  m_GameObject: {fileID: ' + o.go + '}\n' +
    '  m_LocalRotation: ' + (rotZ ? q : '{x: -0, y: -0, z: -0, w: 1}') + '\n' +
    '  m_LocalPosition: {x: 0, y: 0, z: 0}\n' +
    '  m_LocalScale: {x: ' + f(sc[0]) + ', y: ' + f(sc[1]) + ', z: ' + f(sc[2]) + '}\n' +
    '  m_ConstrainProportionsScale: 0\n' +
    '  m_Children:' + kids +
    '  m_Father: {fileID: ' + (o.father == null ? 0 : o.father) + '}\n' +
    '  m_LocalEulerAnglesHint: {x: 0, y: 0, z: ' + f(rotZ) + '}\n' +
    '  m_AnchorMin: ' + v2(o.aMin || [0.5, 0.5]) + '\n' +
    '  m_AnchorMax: ' + v2(o.aMax || [0.5, 0.5]) + '\n' +
    '  m_AnchoredPosition: ' + v2(o.pos || [0, 0]) + '\n' +
    '  m_SizeDelta: ' + v2(o.size || [100, 100]) + '\n' +
    '  m_Pivot: ' + v2(o.pivot || [0.5, 0.5]) + '\n';
}

function canvasRenderer(id, o) {
  return HEAD(222, id, 'CanvasRenderer') + '  m_GameObject: {fileID: ' + o.go + '}\n  m_CullTransparentMesh: 1\n';
}

function canvasGroup(id, o) {
  return HEAD(225, id, 'CanvasGroup') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n' +
    '  m_Alpha: ' + f(o.alpha == null ? 1 : o.alpha) + '\n' +
    '  m_Interactable: ' + (o.interactable === false ? 0 : 1) + '\n' +
    '  m_BlocksRaycasts: ' + (o.blocksRaycasts === false ? 0 : 1) + '\n  m_IgnoreParentGroups: 0\n';
}

// sprite: a 32-hex guid of a texture we own, the literal 'UISPRITE' / 'UIPANEL', or null.
function image(id, o) {
  let sp = '{fileID: 0}';
  if (o.sprite === 'UISPRITE') sp = UISPRITE;
  else if (o.sprite === 'UIPANEL') sp = UIPANEL;
  else if (o.sprite) sp = sref(o.sprite);
  return HEAD(114, id, 'MonoBehaviour') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n' +
    '  m_Script: {fileID: 11500000, guid: ' + UGUI.image + ', type: 3}\n  m_Name: \n' +
    '  m_EditorClassIdentifier: UnityEngine.UI::UnityEngine.UI.Image\n' +
    '  m_Material: {fileID: 0}\n  m_Color: ' + col(o.color || [1, 1, 1, 1]) + '\n' +
    '  m_RaycastTarget: ' + (o.raycast === false ? 0 : 1) + '\n' +
    '  m_RaycastPadding: {x: 0, y: 0, z: 0, w: 0}\n  m_Maskable: 1\n' +
    '  m_OnCullStateChanged:\n    m_PersistentCalls:\n      m_Calls: []\n' +
    '  m_Sprite: ' + sp + '\n  m_Type: ' + (o.type == null ? 0 : o.type) + '\n' +
    '  m_PreserveAspect: ' + (o.preserveAspect ? 1 : 0) + '\n  m_FillCenter: 1\n' +
    '  m_FillMethod: 4\n  m_FillAmount: 1\n  m_FillClockwise: 1\n  m_FillOrigin: 0\n' +
    '  m_UseSpriteMesh: 0\n  m_PixelsPerUnitMultiplier: ' + f(o.ppu == null ? 1 : o.ppu) + '\n';
}

// Horizontal alignment: 1 Left, 2 Centre, 4 Right.  Vertical: 256 Top, 512 Middle, 1024 Bottom.
function tmpText(id, o) {
  const text = String(o.text == null ? '' : o.text);
  if (/[\r\n]/.test(text)) throw new Error('tmpText ' + id + ': a real newline cannot be authored into m_text - set it from code');
  if (/[^\x20-\x7E]/.test(text)) throw new Error('tmpText ' + id + ': the generated TMP atlas is ASCII 32..126 only: ' + JSON.stringify(text));
  const c = o.color || [1, 1, 1, 1];
  const size = o.size == null ? 40 : o.size;
  const min = o.min == null ? 32 : o.min;
  if (min < 32) throw new Error('tmpText ' + id + ': fontSizeMin ' + min + ' is under the project floor of 32');
  return HEAD(114, id, 'MonoBehaviour') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n' +
    '  m_Script: {fileID: 11500000, guid: ' + UGUI.tmp + ', type: 3}\n  m_Name: \n' +
    '  m_EditorClassIdentifier: Unity.TextMeshPro::TMPro.TextMeshProUGUI\n' +
    '  m_Material: {fileID: 0}\n  m_Color: {r: 1, g: 1, b: 1, a: 1}\n' +
    '  m_RaycastTarget: ' + (o.raycast ? 1 : 0) + '\n  m_RaycastPadding: {x: 0, y: 0, z: 0, w: 0}\n  m_Maskable: 1\n' +
    '  m_OnCullStateChanged:\n    m_PersistentCalls:\n      m_Calls: []\n' +
    '  m_text: ' + text + '\n  m_isRightToLeft: 0\n' +
    '  m_fontAsset: {fileID: ' + FONT_ASSET_FID + ', guid: ' + FONT_GUID + ', type: 2}\n' +
    '  m_sharedMaterial: {fileID: ' + FONT_MAT_FID + ', guid: ' + FONT_GUID + ', type: 2}\n' +
    '  m_fontSharedMaterials: []\n  m_fontMaterial: {fileID: 0}\n  m_fontMaterials: []\n' +
    '  m_fontColor32:\n    serializedVersion: 2\n    rgba: ' + rgba32(c) + '\n' +
    '  m_fontColor: ' + col(c) + '\n  m_enableVertexGradient: 0\n  m_colorMode: 3\n' +
    '  m_fontColorGradient:\n    topLeft: {r: 1, g: 1, b: 1, a: 1}\n    topRight: {r: 1, g: 1, b: 1, a: 1}\n' +
    '    bottomLeft: {r: 1, g: 1, b: 1, a: 1}\n    bottomRight: {r: 1, g: 1, b: 1, a: 1}\n' +
    '  m_fontColorGradientPreset: {fileID: 0}\n  m_spriteAsset: {fileID: 0}\n  m_tintAllSprites: 0\n' +
    '  m_StyleSheet: {fileID: 0}\n  m_TextStyleHashCode: -1183493901\n  m_overrideHtmlColors: 0\n' +
    '  m_faceColor:\n    serializedVersion: 2\n    rgba: 4294967295\n' +
    '  m_fontSize: ' + f(size) + '\n  m_fontSizeBase: ' + f(size) + '\n  m_fontWeight: 400\n' +
    '  m_enableAutoSizing: 1\n  m_fontSizeMin: ' + f(min) + '\n  m_fontSizeMax: ' + f(o.max == null ? size : o.max) + '\n' +
    '  m_fontStyle: ' + (o.style == null ? 0 : o.style) + '\n' +
    '  m_HorizontalAlignment: ' + (o.hAlign == null ? 2 : o.hAlign) + '\n' +
    '  m_VerticalAlignment: ' + (o.vAlign == null ? 512 : o.vAlign) + '\n' +
    '  m_textAlignment: 65535\n  m_characterSpacing: ' + f(o.charSpacing || 0) + '\n' +
    '  m_characterHorizontalScale: 1\n  m_wordSpacing: 0\n  m_lineSpacing: ' + f(o.lineSpacing || 0) + '\n' +
    '  m_lineSpacingMax: 0\n  m_paragraphSpacing: 0\n  m_charWidthMaxAdj: 0\n' +
    '  m_TextWrappingMode: 0\n  m_wordWrappingRatios: 0.4\n  m_overflowMode: 0\n' +
    '  m_linkedTextComponent: {fileID: 0}\n  parentLinkedComponent: {fileID: 0}\n' +
    '  m_enableKerning: 0\n  m_ActiveFontFeatures: 6e72656bu\n  m_enableExtraPadding: 0\n' +
    '  checkPaddingRequired: 0\n  m_isRichText: 1\n  m_EmojiFallbackSupport: 1\n  m_parseCtrlCharacters: 1\n' +
    '  m_isOrthographic: 1\n  m_isCullingEnabled: 0\n  m_horizontalMapping: 0\n  m_verticalMapping: 0\n' +
    '  m_uvLineOffset: 0\n  m_geometrySortingOrder: 0\n  m_IsTextObjectScaleStatic: 0\n' +
    '  m_VertexBufferAutoSizeReduction: 0\n  m_useMaxVisibleDescender: 1\n  m_pageToDisplay: 1\n' +
    '  m_margin: {x: 0, y: 0, z: 0, w: 0}\n  m_isUsingLegacyAnimationComponent: 0\n' +
    '  m_isVolumetricText: 0\n  m_hasFontAssetChanged: 0\n  m_baseMaterial: {fileID: 0}\n' +
    '  m_maskOffset: {x: 0, y: 0, z: 0, w: 0}\n';
}

function button(id, o) {
  return HEAD(114, id, 'MonoBehaviour') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n' +
    '  m_Script: {fileID: 11500000, guid: ' + UGUI.button + ', type: 3}\n  m_Name: \n' +
    '  m_EditorClassIdentifier: UnityEngine.UI::UnityEngine.UI.Button\n' +
    '  m_Navigation:\n    m_Mode: 3\n    m_WrapAround: 0\n    m_SelectOnUp: {fileID: 0}\n' +
    '    m_SelectOnDown: {fileID: 0}\n    m_SelectOnLeft: {fileID: 0}\n    m_SelectOnRight: {fileID: 0}\n' +
    '  m_Transition: 1\n  m_Colors:\n    m_NormalColor: {r: 1, g: 1, b: 1, a: 1}\n' +
    '    m_HighlightedColor: {r: 0.88, g: 0.88, b: 0.88, a: 1}\n' +
    '    m_PressedColor: {r: 0.72, g: 0.72, b: 0.72, a: 1}\n' +
    '    m_SelectedColor: {r: 1, g: 1, b: 1, a: 1}\n' +
    '    m_DisabledColor: {r: 0.5, g: 0.5, b: 0.5, a: 0.5}\n    m_ColorMultiplier: 1\n    m_FadeDuration: 0.1\n' +
    '  m_SpriteState:\n    m_HighlightedSprite: {fileID: 0}\n    m_PressedSprite: {fileID: 0}\n' +
    '    m_SelectedSprite: {fileID: 0}\n    m_DisabledSprite: {fileID: 0}\n' +
    '  m_AnimationTriggers:\n    m_NormalTrigger: Normal\n    m_HighlightedTrigger: Highlighted\n' +
    '    m_PressedTrigger: Pressed\n    m_SelectedTrigger: Selected\n    m_DisabledTrigger: Disabled\n' +
    '  m_Interactable: ' + (o.interactable === false ? 0 : 1) + '\n  m_TargetGraphic: ' + fid(o.target) + '\n' +
    '  m_OnClick:\n    m_PersistentCalls:\n      m_Calls: []\n';
}

function canvas(id, o) {
  return HEAD(223, id, 'Canvas') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  serializedVersion: 3\n' +
    '  m_RenderMode: 0\n  m_Camera: {fileID: 0}\n  m_PlaneDistance: 100\n  m_PixelPerfect: 0\n' +
    '  m_ReceivesEvents: 1\n  m_OverrideSorting: 0\n  m_OverridePixelPerfect: 0\n' +
    '  m_SortingBucketNormalizedSize: 0\n  m_VertexColorAlwaysGammaSpace: 0\n  m_UseReflectionProbes: 0\n' +
    '  m_AdditionalShaderChannelsFlag: 0\n  m_UpdateRectTransformForStandalone: 0\n' +
    '  m_SortingLayerID: 0\n  m_SortingOrder: ' + (o.sortingOrder == null ? 0 : o.sortingOrder) + '\n  m_TargetDisplay: 0\n';
}

function canvasScaler(id, o) {
  const r = o.ref || [1242, 2688];
  return HEAD(114, id, 'MonoBehaviour') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n' +
    '  m_Script: {fileID: 11500000, guid: ' + UGUI.scaler + ', type: 3}\n  m_Name: \n' +
    '  m_EditorClassIdentifier: UnityEngine.UI::UnityEngine.UI.CanvasScaler\n' +
    '  m_UiScaleMode: 1\n  m_ReferencePixelsPerUnit: 100\n  m_ScaleFactor: 1\n' +
    '  m_ReferenceResolution: ' + v2(r) + '\n  m_ScreenMatchMode: ' + (o.screenMatch == null ? 1 : o.screenMatch) + '\n' +
    '  m_MatchWidthOrHeight: ' + f(o.match || 0) + '\n  m_PhysicalUnit: 3\n  m_FallbackScreenDPI: 96\n' +
    '  m_DefaultSpriteDPI: 96\n  m_DynamicPixelsPerUnit: 1\n  m_PresetInfoIsWorld: 0\n';
}

function raycaster(id, o) {
  return HEAD(114, id, 'MonoBehaviour') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n' +
    '  m_Script: {fileID: 11500000, guid: ' + UGUI.raycaster + ', type: 3}\n  m_Name: \n' +
    '  m_EditorClassIdentifier: UnityEngine.UI::UnityEngine.UI.GraphicRaycaster\n' +
    '  m_IgnoreReversedGraphics: 1\n  m_BlockingObjects: 0\n' +
    '  m_BlockingMask:\n    serializedVersion: 2\n    m_Bits: 4294967295\n';
}

// fields: ['name', 'already-rendered value'] pairs; an array value becomes a YAML sequence.
function mono(id, o) {
  const body = (o.fields || []).map(([k, val]) =>
    Array.isArray(val)
      ? '  ' + k + ':' + (val.length ? '\n' + val.map(x => '  - ' + x).join('\n') : ' []')
      : '  ' + k + ': ' + val).join('\n');
  return HEAD(114, id, 'MonoBehaviour') +
    '  m_GameObject: {fileID: ' + o.go + '}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n' +
    '  m_Script: {fileID: 11500000, guid: ' + o.guid + ', type: 3}\n  m_Name: \n' +
    '  m_EditorClassIdentifier: Assembly-CSharp::' + o.cls + '\n' + (body ? body + '\n' : '');
}

module.exports = {
  gameObject, rectTransform, canvasRenderer, canvasGroup, image, tmpText, button,
  canvas, canvasScaler, raycaster, mono,
  f, v2, col, rgba32, sref, fid,
  UGUI, FONT_GUID, FONT_ASSET_FID, FONT_MAT_FID, BUILTIN, UISPRITE, UIPANEL,
};
