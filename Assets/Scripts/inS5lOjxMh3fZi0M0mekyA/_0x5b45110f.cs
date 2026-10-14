using UnityEngine;

/// <summary>
/// Easing curves and a tiny one-shot timeline helper. This project has no tween
/// library, so every animation is advanced by hand from Update with these curves.
/// Pure arithmetic: nothing here holds state or touches a scene object.
/// </summary>
public static class _0x5b45110f
{
    public static float OutCubic(float _0xfb6bb583)
    {
        float _0xfb19ba8b = 1f - Clamp01(_0xfb6bb583);
        return 1f - (_0xfb19ba8b * _0xfb19ba8b * _0xfb19ba8b);
    }

    public static Vector2 Lerp(Vector2 _0xcc774e67, Vector2 _0xb3786c00, float _0x161fc748)
    {
        return new Vector2(_0xcc774e67.x + ((_0xb3786c00.x - _0xcc774e67.x) * _0x161fc748), _0xcc774e67.y + ((_0xb3786c00.y - _0xcc774e67.y) * _0x161fc748));
    }

    /// <summary>A damped shake offset in the range -1 .. 1.</summary>
    public static float Shake(float _0x1f50a728, float _0x6894bd65)
    {
        float _0x28ca5963 = Clamp01(_0x1f50a728);
        return Mathf.Sin(_0x28ca5963 * _0x6894bd65 * Mathf.PI * 2f) * (1f - _0x28ca5963);
    }

    public static float InOutSine(float _0xf7663e3a)
    {
        return 0.5f - (0.5f * Mathf.Cos(Mathf.PI * Clamp01(_0xf7663e3a)));
    }

    public static float OutBack(float _0xcbf0a0ea)
    {
        const float _0x548a90a8 = 1.70158f;
        float _0x68cfd121 = Clamp01(_0xcbf0a0ea) - 1f;
        return 1f + (_0x68cfd121 * _0x68cfd121 * (((_0x548a90a8 + 1f) * _0x68cfd121) + _0x548a90a8));
    }

    /// <summary>A 0 - 1 - 0 punch envelope, for a tap that must read in a still frame.</summary>
    public static float Punch(float _0xd94eec25)
    {
        float _0xcb57b873 = Clamp01(_0xd94eec25);
        return Mathf.Sin(Mathf.PI * _0xcb57b873) * (1f - _0xcb57b873);
    }

    public static float Clamp01(float _0xa3676cd9)
    {
        return _0xa3676cd9 < 0f ? 0f : (_0xa3676cd9 > 1f ? 1f : _0xa3676cd9);
    }

    public static float InQuad(float _0x96c881f7)
    {
        float _0x9500a872 = Clamp01(_0x96c881f7);
        return _0x9500a872 * _0x9500a872;
    }
}