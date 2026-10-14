using UnityEngine;

/// <summary>
/// The difficulty table, serialized in the scene rather than compiled in, plus the
/// one place that knows the Roman numeral of a chamber. Feasibility of each row is
/// checked at startup and a broken row falls back to a known-good envelope.
/// </summary>
public sealed class _0xd30f434d : MonoBehaviour
{
    private static _0x0c0fdd1c SafeDefault()
    {
        _0x0c0fdd1c _0x12a9ee17 = new _0x0c0fdd1c();
        _0x12a9ee17.LeftCount = 2;
        _0x12a9ee17.RightCount = 2;
        _0x12a9ee17.DecoyCount = 2;
        _0x12a9ee17.TargetLow = 12;
        _0x12a9ee17.TargetHigh = 18;
        _0x12a9ee17.WeightLow = 3;
        _0x12a9ee17.WeightHigh = 9;
        _0x12a9ee17.Placements = 10;
        return _0x12a9ee17;
    }

    private static readonly string[] _0xf3508d45 = new string[]
    {
        _0xc7b46fa7._0x2cb503c3(new byte[1] { 19 }, 90),
        _0xc7b46fa7._0x2cb503c3(new byte[2] { 29, 29 }, 84),
        _0xc7b46fa7._0x2cb503c3(new byte[3] { 11, 11, 11 }, 66),
        _0xc7b46fa7._0x2cb503c3(new byte[2] { 162, 189 }, 235),
        _0xc7b46fa7._0x2cb503c3(new byte[1] { 135 }, 209)
    };
    public _0x0c0fdd1c _0x055a8058(int _0xad4520fc)
    {
        if (this._chambers == null || this._chambers.Length == 0)
        {
            return SafeDefault();
        }

        int _0x6517740d = Mathf.Clamp(_0xad4520fc, 0, this._chambers.Length - 1);
        _0x0c0fdd1c _0xbf3c3e54 = this._chambers[_0x6517740d];
        if (_0xbf3c3e54 == null || !_0xbf3c3e54._0x99a1c546())
        {
            return SafeDefault();
        }

        return _0xbf3c3e54;
    }

    public static string Numeral(int _0x4d3f1651)
    {
        if (_0x4d3f1651 < 0 || _0x4d3f1651 >= _0xf3508d45.Length)
        {
            return _0xc7b46fa7._0x2cb503c3(new byte[1] { 111 }, 38);
        }

        return _0xf3508d45[_0x4d3f1651];
    }

    public int _0x3c2939d7
    {
        get
        {
            return this._chambers == null ? 0 : this._chambers.Length;
        }
    }

    [SerializeField]
    private _0x0c0fdd1c[] _chambers = new _0x0c0fdd1c[0];
}

internal static class _0xc7b46fa7
{
    internal static string _0x2cb503c3(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}