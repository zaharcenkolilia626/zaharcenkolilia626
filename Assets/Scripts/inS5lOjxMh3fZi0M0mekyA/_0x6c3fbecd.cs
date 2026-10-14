using TMPro;
using UnityEngine;

/// <summary>
/// Every rule C.10 obligation for a label, in one place: word wrap off, hand-placed
/// line breaks, autosize always on with a real floor, and an own outlined material
/// instance for anything the contrast stage cannot see in a file.
/// </summary>
public static class _0x6c3fbecd
{
    /// <summary>Assign text and keep the C.10 contract. Line breaks come from the caller, never from the engine.</summary>
    public static void SetText(TMP_Text _0xf46c0ac0, string _0x41882091)
    {
        if (_0xf46c0ac0 == null)
        {
            return;
        }

        _0xf46c0ac0.textWrappingMode = TextWrappingModes.NoWrap;
        _0xf46c0ac0.overflowMode = TextOverflowModes.Overflow;
        _0xf46c0ac0.enableAutoSizing = true;
        if (_0xf46c0ac0.fontSizeMin < UiFontFloor)
        {
            _0xf46c0ac0.fontSizeMin = UiFontFloor;
        }

        _0xf46c0ac0.text = _0x41882091 == null ? string.Empty : _0x41882091;
    }

    public const float UiFontFloor = 32f;
    /// <summary>Size, floor and face in one call, for labels built from code.</summary>
    public static void Style(TMP_Text _0x27b521dd, float _0xea23e8c2, float _0x2d82099a, Color _0xbab3df30)
    {
        if (_0x27b521dd == null)
        {
            return;
        }

        _0x27b521dd.fontSize = _0xea23e8c2;
        _0x27b521dd.fontSizeMax = _0xea23e8c2;
        _0x27b521dd.fontSizeMin = Mathf.Max(_0x2d82099a, UiFontFloor);
        _0x27b521dd.color = _0xbab3df30;
        _0x27b521dd.textWrappingMode = TextWrappingModes.NoWrap;
        _0x27b521dd.overflowMode = TextOverflowModes.Overflow;
        _0x27b521dd.enableAutoSizing = true;
    }

    public static void SetText(TMP_Text _0x55f42dfa, string _0xb8cd8c8e, Color _0x6ab7fef9)
    {
        SetText(_0x55f42dfa, _0xb8cd8c8e);
        if (_0x55f42dfa != null)
        {
            _0x55f42dfa.color = _0x6ab7fef9;
        }
    }

    /// <summary>
    /// Give a label its own material instance with an outline that contrasts with its face.
    /// Reading fontMaterial creates the instance; the shared asset is left alone.
    /// </summary>
    public static void ApplyOutline(TMP_Text _0x0b71e20f, float _0xcc30826a)
    {
        if (_0x0b71e20f == null)
        {
            return;
        }

        Color _0xd6a17822 = _0x0b71e20f.color;
        float _0x35b1fcb2 = (0.299f * _0xd6a17822.r) + (0.587f * _0xd6a17822.g) + (0.114f * _0xd6a17822.b);
        Material _0xffe01863 = _0x0b71e20f.fontMaterial;
        if (_0xffe01863 == null)
        {
            return;
        }

        _0xffe01863.EnableKeyword(_0x84d04ebb._0x677e00e0(new byte[10] { 169, 179, 178, 170, 175, 168, 163, 185, 169, 168 }, 230));
        _0xffe01863.SetColor(ShaderUtilities.ID_OutlineColor, _0x35b1fcb2 < 0.5f ? _0x5789e692.Parchment : _0x5789e692.Ink);
        _0xffe01863.SetFloat(ShaderUtilities.ID_OutlineWidth, Mathf.Clamp(_0xcc30826a, 0.10f, 0.30f));
    }
}

internal static class _0x84d04ebb
{
    internal static string _0x677e00e0(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}