using System.Collections.Generic;
using TMPro;
using UnityEngine;

// TmpContrastGuard.cs — staged into every Unity app by approve-pipeline-unity.sh
// (stage 5c3, rule C.14 in CLAUDE-unity.md). Do not edit the copy inside a project;
// edit scripts/lib/unity/TmpContrastGuard.cs.
//
// WHY: every TMP label gets an outline (C.10), and by default that outline is dark.
// A dark face colour on a dark outline merges into a smudge — the label is not
// readable on any backing (ANDROID-3627: PLAY drawn Deep #12151E on the #12151E
// outline read as a black blob). enforce-text-contrast.sh fixes colours SERIALISED
// in scenes/prefabs, but labels built at runtime from C# (UiKit.Cta, VaultUi.Caption,
// label.color = Palette.X ...) never reach a scene file, so that pass cannot see them.
//
// WHAT: after any TMP text is regenerated, compare its face colour with the outline
// colour of the material it actually renders with. Below WCAG 4.5:1 the face is
// blended toward white (dark outline) or black (light outline) until it reaches 7:1.
// Hue is kept; alpha is kept. A label whose outline was deliberately switched to a
// light colour (TextReadability-style per-label material) is measured against THAT
// outline, so intentionally dark text on a light rim is left alone. Labels without
// an outline are left alone too.
public sealed class _0x919a669b : MonoBehaviour
{
    private static float Ratio(Color _0xa316f6bd, Color _0xa271a8bd)
    {
        float _0x219c2ff9 = Luminance(_0xa316f6bd);
        float _0x1a5fa0cc = Luminance(_0xa271a8bd);
        return (Mathf.Max(_0x219c2ff9, _0x1a5fa0cc) + 0.05f) / (Mathf.Min(_0x219c2ff9, _0x1a5fa0cc) + 0.05f);
    }

    private readonly List<TMP_Text> _0x6aa2fb44 = new List<TMP_Text>();
    private readonly HashSet<TMP_Text> _0x1b7c9393 = new HashSet<TMP_Text>();
    // A lambda held in a field, never the bare method group: Plana renames the method
    // declaration but not a method-group reference (verify-unity-buttons.sh, CS0103).
    // The field keeps Add and Remove on the same delegate instance.
    private System.Action<Object> _0x7915c795;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Boot()
    {
        if (_0xed32543f != null)
            return;
        GameObject _0x06ef31f0 = new GameObject(_0x15d50400._0x31932ea3(new byte[16] { 211, 234, 247, 196, 232, 233, 243, 245, 230, 244, 243, 192, 242, 230, 245, 227 }, 135));
        _0x06ef31f0.hideFlags = HideFlags.HideInHierarchy;
        DontDestroyOnLoad(_0x06ef31f0);
        _0xed32543f = _0x06ef31f0.AddComponent<_0x919a669b>();
    }

    private const float TargetRatio = 7f;
    private static float Linear(float _0xd6878fab)
    {
        _0xd6878fab = Mathf.Clamp01(_0xd6878fab);
        return _0xd6878fab <= 0.03928f ? _0xd6878fab / 12.92f : Mathf.Pow((_0xd6878fab + 0.055f) / 1.055f, 2.4f);
    }

    private void OnEnable()
    {
        if (this._0x7915c795 == null)
            this._0x7915c795 = _0xe57c9dc2 => this._0x7b3105a8(_0xe57c9dc2);
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(this._0x7915c795);
    }

    private void OnDisable()
    {
        if (this._0x7915c795 != null)
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(this._0x7915c795);
    }

    private static _0x919a669b _0xed32543f;
    // The event fires from inside the canvas rebuild. Changing the colour right there
    // would re-dirty the graphic mid-rebuild, which Unity rejects — so queue it and
    // apply in LateUpdate, which runs before the next frame's rebuild.
    private void _0x7b3105a8(Object _0x0c369b56)
    {
        TMP_Text _0xd6c46404 = _0x0c369b56 as TMP_Text;
        if (_0xd6c46404 != null)
            this._0x1b7c9393.Add(_0xd6c46404);
    }

    private const float MinOutlineWidth = 0.01f;
    private void LateUpdate()
    {
        if (this._0x1b7c9393.Count == 0)
            return;
        this._0x6aa2fb44.Clear();
        this._0x6aa2fb44.AddRange(this._0x1b7c9393);
        this._0x1b7c9393.Clear();
        for (int _0x54022005 = 0; _0x54022005 < this._0x6aa2fb44.Count; _0x54022005++)
            Fix(this._0x6aa2fb44[_0x54022005]);
    }

    private const float MinRatio = 4.5f;
    private static void Fix(TMP_Text _0xd0e5c155)
    {
        if (_0xd0e5c155 == null || !_0xd0e5c155.isActiveAndEnabled)
            return;
        Material _0x6f80731b = _0xd0e5c155.fontSharedMaterial;
        if (_0x6f80731b == null || !_0x6f80731b.HasProperty(ShaderUtilities.ID_OutlineColor) || !_0x6f80731b.HasProperty(ShaderUtilities.ID_OutlineWidth))
            return;
        if (_0x6f80731b.GetFloat(ShaderUtilities.ID_OutlineWidth) < MinOutlineWidth)
            return;
        Color _0xab8ec26e = _0xd0e5c155.color;
        if (_0xab8ec26e.a <= 0f)
            return;
        Color _0x4965d1f9 = _0x6f80731b.GetColor(ShaderUtilities.ID_OutlineColor);
        if (Ratio(_0xab8ec26e, _0x4965d1f9) >= MinRatio)
            return;
        Color _0x68127936 = Luminance(_0x4965d1f9) < 0.5f ? Color.white : Color.black;
        Color _0xb080596c;
        if (Ratio(_0x68127936, _0x4965d1f9) < TargetRatio)
        {
            _0xb080596c = _0x68127936;
        }
        else
        {
            // Smallest blend that reaches the target: contrast grows monotonically
            // with t, so a short bisection keeps as much of the hue as possible.
            float _0xed202c35 = 0f;
            float _0x4abe5a0d = 1f;
            for (int _0x9849b8da = 0; _0x9849b8da < 20; _0x9849b8da++)
            {
                float _0x2bc54f14 = (_0xed202c35 + _0x4abe5a0d) * 0.5f;
                if (Ratio(Color.Lerp(_0xab8ec26e, _0x68127936, _0x2bc54f14), _0x4965d1f9) >= TargetRatio)
                    _0x4abe5a0d = _0x2bc54f14;
                else
                    _0xed202c35 = _0x2bc54f14;
            }

            _0xb080596c = Color.Lerp(_0xab8ec26e, _0x68127936, _0x4abe5a0d);
        }

        _0xb080596c.a = _0xab8ec26e.a;
        _0xd0e5c155.color = _0xb080596c;
    }

    // WCAG relative luminance of an sRGB colour, and the contrast ratio of two.
    private static float Luminance(Color _0xe31a8913)
    {
        return 0.2126f * Linear(_0xe31a8913.r) + 0.7152f * Linear(_0xe31a8913.g) + 0.0722f * Linear(_0xe31a8913.b);
    }
}

internal static class _0x15d50400
{
    internal static string _0x31932ea3(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}