using UnityEngine;

/// <summary>
/// The whole colour vocabulary of the app, as independent float literals.
/// Never parsed from a hex string: an encrypted string literal in a static
/// initializer can be reordered into a dead type initializer after obfuscation.
/// Every field below stands on its own and depends on nothing.
/// </summary>
public static class _0x5789e692
{
    public static readonly Color StoneSlab = new Color(0.165f, 0.145f, 0.192f, 1f);
    public static readonly Color EmberGold = new Color(1f, 0.718f, 0.012f, 1f);
    public static readonly Color DragonCrimson = new Color(0.820f, 0.286f, 0.357f, 1f);
    // The five faces allowed for text (rule C.14: each contrasts 7:1 or better with Ink).
    public static readonly Color Parchment = new Color(1f, 0.957f, 0.902f, 1f);
    public static readonly Color LapisDeep = new Color(0.169f, 0.420f, 0.573f, 1f);
    public static readonly Color Ink = new Color(0.090f, 0.075f, 0.125f, 1f);
    public static Color Blend(Color _0x69818502, Color _0x4fa8df25, float _0x4aa4e4b5)
    {
        return Color.Lerp(_0x69818502, _0x4fa8df25, Mathf.Clamp01(_0x4aa4e4b5));
    }

    public static Color WithAlpha(Color _0x93d41e47, float _0x382a4dd3)
    {
        return new Color(_0x93d41e47.r, _0x93d41e47.g, _0x93d41e47.b, Mathf.Clamp01(_0x382a4dd3));
    }

    public static readonly Color Obsidian = new Color(0.114f, 0.102f, 0.133f, 1f);
    public static readonly Color CrimsonDeep = new Color(0.557f, 0.184f, 0.235f, 1f);
    public static readonly Color StoneRidge = new Color(0.227f, 0.200f, 0.259f, 1f);
    public static readonly Color ParchmentMuted = new Color(0.788f, 0.737f, 0.663f, 1f);
    public static readonly Color SkyLapis = new Color(0.306f, 0.659f, 0.871f, 1f);
    public static readonly Color EmberGoldSoft = new Color(1f, 0.835f, 0.420f, 1f);
    public static readonly Color ObsidianDeep = new Color(0.078f, 0.067f, 0.094f, 1f);
}