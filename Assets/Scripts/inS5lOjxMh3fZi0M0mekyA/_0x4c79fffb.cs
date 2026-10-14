using UnityEngine;

/// <summary>
/// Every board coordinate of the game scene, derived once from the canvas frame rather
/// than typed in at each call site. The frame is a fixed 1242x2688 rectangle pinned to
/// the canvas centre, so these numbers mean the same thing on every device shape.
/// </summary>
public static class _0x4c79fffb
{
    /// <summary>Beam tilt in degrees; positive means the right pan carries more.</summary>
    public static float Tilt(int _0x5c1029ce, int _0x092a2431, int _0x803f2f5f)
    {
        if (_0x803f2f5f <= 0)
        {
            return 0f;
        }

        float _0xa2ab4449 = (_0x092a2431 - _0x5c1029ce) / (float)_0x803f2f5f;
        return Mathf.Clamp(_0xa2ab4449, -1f, 1f) * MaxTiltDegrees;
    }

    public const int PanColumns = 3;
    public const float FrameWidth = 1242f;
    /// <summary>Centre of tray slot <paramref name = "index"/>, filled row by row from the top row.</summary>
    public static Vector2 TraySlot(int _0xc279751e, int _0x7f6ee137)
    {
        int _0x4c8d458e = TrayColumns(_0x7f6ee137);
        int _0xe6040c44 = _0xc279751e / _0x4c8d458e;
        int _0x56935069 = _0xc279751e % _0x4c8d458e;
        float _0x64ed457a = TrayPitch(_0x7f6ee137);
        float _0xba3d8f50 = BoardCentreX + ((_0x56935069 - ((_0x4c8d458e - 1) * 0.5f)) * _0x64ed457a);
        float _0x346bd665 = _0xe6040c44 == 0 ? TrayRowTopY : TrayRowBottomY;
        return new Vector2(_0xba3d8f50, _0x346bd665);
    }

    public const float BeamPivotY = 1800f;
    public const float FrameHeight = 2688f;
    public static float TrayPitch(int _0x4e8b309a)
    {
        return TraySpan / TrayColumns(_0x4e8b309a);
    }

    public const float PanSize = 300f;
    public const float BeamHalfSpan = 340f;
    /// <summary>Slot <paramref name = "index"/> inside a pan, in pan-local coordinates.</summary>
    public static Vector2 PanSlot(int _0x3b08aee1)
    {
        int _0x7348ab00 = Mathf.Clamp(_0x3b08aee1, 0, (PanColumns * PanRows) - 1);
        int _0xf3ffad44 = _0x7348ab00 / PanColumns;
        int _0xead7692f = _0x7348ab00 % PanColumns;
        float _0xdf39be63 = (_0xead7692f - ((PanColumns - 1) * 0.5f)) * PanSlotPitch;
        float _0x8e0ad899 = 14f + ((_0xf3ffad44 == 0 ? 1 : -1) * (PanSlotPitch * 0.52f));
        return new Vector2(_0xdf39be63, _0x8e0ad899);
    }

    public const float TraySpan = 1100f;
    public const int PanRows = 2;
    public const float TrayRowTopY = 980f;
    /// <summary>Where the chain anchors on the beam once it has turned.</summary>
    public static Vector2 BeamEnd(float _0xd3b9bbe1, bool _0x811c152f)
    {
        float _0x3aaf2652 = -_0xd3b9bbe1 * Mathf.Deg2Rad;
        float _0xfcacb1b7 = _0x811c152f ? -PanOffsetX : PanOffsetX;
        float _0x7b4d4879 = BoardCentreX + (_0xfcacb1b7 * Mathf.Cos(_0x3aaf2652));
        float _0xa4a59985 = BeamPivotY + (_0xfcacb1b7 * Mathf.Sin(_0x3aaf2652));
        return new Vector2(_0x7b4d4879, _0xa4a59985);
    }

    public static float PanCentreX(bool _0x1b0a9a36)
    {
        return _0x1b0a9a36 ? (BoardCentreX - PanOffsetX) : (BoardCentreX + PanOffsetX);
    }

    public const float PanRestY = 1480f;
    public static int TrayColumns(int _0x2febc303)
    {
        return Mathf.Max(1, Mathf.CeilToInt(_0x2febc303 / 2f));
    }

    public const float BoardCentreX = 621f;
    public static float TrayStoneSize(int _0x248e1740)
    {
        return Mathf.Min(TrayStoneCap, TrayPitch(_0x248e1740) * 0.84f);
    }

    public const float TrayStoneCap = 200f;
    public static int _0xf11a6f5c
    {
        get
        {
            return PanColumns * PanRows;
        }
    }

    public const float MaxTiltDegrees = 18f;
    public const float PanSlotPitch = 96f;
    public const float PanStoneSize = 92f;
    public const float PanOffsetX = 320f;
    public const float TrayRowBottomY = 760f;
    /// <summary>The pan heights that follow a given tilt, so one mover owns both.</summary>
    public static float PanHeight(float _0x8221d12b, bool _0x80797589)
    {
        float _0xbdc8e0c0 = Mathf.Sin(_0x8221d12b * Mathf.Deg2Rad) * PanOffsetX;
        return PanRestY + (_0x80797589 ? _0xbdc8e0c0 : -_0xbdc8e0c0);
    }
}