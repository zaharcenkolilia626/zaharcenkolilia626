using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistence for the five things that survive a session. Keys are stable literals that
/// no C# symbol feeds, so obfuscation cannot move them. A malformed archive key is the
/// only error surface this offline game has, and it resets that one key rather than throwing.
/// </summary>
public sealed class _0x9e4438d2 : MonoBehaviour
{
    public bool _0x3a71d345
    {
        get
        {
            return PlayerPrefs.GetInt(KeyContrast, 0) != 0;
        }

        set
        {
            PlayerPrefs.SetInt(KeyContrast, value ? 1 : 0);
        }
    }

    /// <summary>Chamber I is always open; every later one needs the chamber below it cleared.</summary>
    public bool _0x31e57292(int _0xfa798c47)
    {
        return _0xfa798c47 <= 0 || this._0x5e678cd0(_0xfa798c47 - 1);
    }

    private static readonly string KeyCleared = _0xf3d1fb0a._0x13cfbc0c(new byte[20] { 169, 191, 168, 227, 174, 165, 172, 160, 175, 168, 191, 190, 227, 174, 161, 168, 172, 191, 168, 169 }, 205);
    private static readonly string KeyContrast = _0xf3d1fb0a._0x13cfbc0c(new byte[16] { 117, 99, 116, 63, 126, 97, 101, 63, 114, 126, 127, 101, 99, 112, 98, 101 }, 17);
    public int _0x81cc3fbd
    {
        get
        {
            return Mathf.Max(0, PlayerPrefs.GetInt(KeySparks, StartingSparks));
        }

        set
        {
            PlayerPrefs.SetInt(KeySparks, Mathf.Max(0, value));
        }
    }

    /// <summary>True once the player has started or cleared anything, so a return to the
    /// menu can skip the branded wait instead of replaying it every time.</summary>
    public bool _0x874890e4
    {
        get
        {
            if (this._0xa7bfb78d != 0)
            {
                return true;
            }

            for (int _0x5369f9e8 = 0; _0x5369f9e8 < ChamberCount; _0x5369f9e8++)
            {
                if (this._0xbd15fbd6(_0x5369f9e8) > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void _0xa00752ac(int _0xde27ff26)
    {
        PlayerPrefs.SetInt(_0xf3d1fb0a._0x13cfbc0c(new byte[12] { 206, 216, 207, 132, 203, 222, 222, 207, 199, 218, 222, 132 }, 170) + _0xde27ff26, this._0xbd15fbd6(_0xde27ff26) + 1);
        PlayerPrefs.Save();
    }

    public void _0xf1b19d86()
    {
        PlayerPrefs.SetInt(KeyCleared, 0);
        PlayerPrefs.SetInt(KeySelected, 0);
        PlayerPrefs.SetInt(KeySparks, StartingSparks);
        PlayerPrefs.SetInt(KeyEmbers, 0);
        PlayerPrefs.SetString(KeyArchive, string.Empty);
        for (int _0x9c9a25fa = 0; _0x9c9a25fa < ChamberCount; _0x9c9a25fa++)
        {
            PlayerPrefs.SetInt(_0xf3d1fb0a._0x13cfbc0c(new byte[12] { 242, 228, 243, 184, 247, 226, 226, 243, 251, 230, 226, 184 }, 150) + _0x9c9a25fa, 0);
        }

        PlayerPrefs.Save();
    }

    public void _0x3530f7b1(int _0xc06364c4)
    {
        PlayerPrefs.SetInt(KeyCleared, this._0xa7bfb78d | (1 << _0xc06364c4));
        PlayerPrefs.Save();
    }

    public int _0x8e49a77c
    {
        get
        {
            return Mathf.Max(0, PlayerPrefs.GetInt(KeyEmbers, 0));
        }

        set
        {
            PlayerPrefs.SetInt(KeyEmbers, Mathf.Max(0, value));
        }
    }

    private static readonly string KeyHaptics = _0xf3d1fb0a._0x13cfbc0c(new byte[15] { 121, 111, 120, 51, 114, 109, 105, 51, 117, 124, 109, 105, 116, 126, 110 }, 29);
    private static readonly string KeyArchive = _0xf3d1fb0a._0x13cfbc0c(new byte[19] { 21, 3, 20, 95, 16, 3, 18, 25, 24, 7, 20, 95, 6, 20, 24, 22, 25, 5, 2 }, 113);
    public void _0x9c879244(int[] _0x16c6b665)
    {
        if (_0x16c6b665 == null)
        {
            return;
        }

        List<int> _0x98956ce5 = this._0xc99256b2();
        for (int _0x8899f17f = 0; _0x8899f17f < _0x16c6b665.Length; _0x8899f17f++)
        {
            if (_0x16c6b665[_0x8899f17f] > 0 && !_0x98956ce5.Contains(_0x16c6b665[_0x8899f17f]))
            {
                _0x98956ce5.Add(_0x16c6b665[_0x8899f17f]);
            }
        }

        _0x98956ce5.Sort();
        System.Text.StringBuilder _0x99033142 = new System.Text.StringBuilder();
        for (int _0xea737d48 = 0; _0xea737d48 < _0x98956ce5.Count; _0xea737d48++)
        {
            if (_0xea737d48 > 0)
            {
                _0x99033142.Append(',');
            }

            _0x99033142.Append(_0x98956ce5[_0xea737d48]);
        }

        PlayerPrefs.SetString(KeyArchive, _0x99033142.ToString());
        PlayerPrefs.Save();
    }

    public int _0xa7bfb78d
    {
        get
        {
            return PlayerPrefs.GetInt(KeyCleared, 0);
        }
    }

    private static readonly string KeySparks = _0xf3d1fb0a._0x13cfbc0c(new byte[10] { 74, 92, 75, 0, 93, 94, 79, 92, 69, 93 }, 46);
    private static readonly string KeyEmbers = _0xf3d1fb0a._0x13cfbc0c(new byte[10] { 113, 103, 112, 59, 112, 120, 119, 112, 103, 102 }, 21);
    public int _0xbd15fbd6(int _0x8e5df487)
    {
        return PlayerPrefs.GetInt(_0xf3d1fb0a._0x13cfbc0c(new byte[12] { 118, 96, 119, 60, 115, 102, 102, 119, 127, 98, 102, 60 }, 18) + _0x8e5df487, 0);
    }

    public int _0xbd59124c
    {
        get
        {
            return Mathf.Clamp(PlayerPrefs.GetInt(KeySelected, 0), 0, ChamberCount - 1);
        }

        set
        {
            PlayerPrefs.SetInt(KeySelected, Mathf.Clamp(value, 0, ChamberCount - 1));
        }
    }

    public bool _0x5e678cd0(int _0x9d344730)
    {
        return (this._0xa7bfb78d & (1 << _0x9d344730)) != 0;
    }

    private static readonly string KeySelected = _0xf3d1fb0a._0x13cfbc0c(new byte[20] { 168, 190, 169, 226, 175, 164, 173, 161, 174, 169, 190, 226, 191, 169, 160, 169, 175, 184, 169, 168 }, 204);
    public bool _0xdac3f8a9
    {
        get
        {
            return PlayerPrefs.GetInt(KeyHaptics, 1) != 0;
        }

        set
        {
            PlayerPrefs.SetInt(KeyHaptics, value ? 1 : 0);
        }
    }

    public const int ChamberCount = 5;
    /// <summary>Catalogued ember weights, ascending. A corrupt record resets to empty.</summary>
    public List<int> _0xc99256b2()
    {
        List<int> _0xef1b2113 = new List<int>();
        string _0x5954a22c = PlayerPrefs.GetString(KeyArchive, string.Empty);
        if (string.IsNullOrEmpty(_0x5954a22c))
        {
            return _0xef1b2113;
        }

        string[] _0xf85f61f3 = _0x5954a22c.Split(',');
        for (int _0xb74ffab6 = 0; _0xb74ffab6 < _0xf85f61f3.Length; _0xb74ffab6++)
        {
            int _0x4bcaf995;
            if (!int.TryParse(_0xf85f61f3[_0xb74ffab6], out _0x4bcaf995) || _0x4bcaf995 < 1 || _0x4bcaf995 > 99)
            {
                PlayerPrefs.SetString(KeyArchive, string.Empty);
                PlayerPrefs.Save();
                return new List<int>();
            }

            if (!_0xef1b2113.Contains(_0x4bcaf995))
            {
                _0xef1b2113.Add(_0x4bcaf995);
            }
        }

        _0xef1b2113.Sort();
        return _0xef1b2113;
    }

    public const int StartingSparks = 5;
}

internal static class _0xf3d1fb0a
{
    internal static string _0x13cfbc0c(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}