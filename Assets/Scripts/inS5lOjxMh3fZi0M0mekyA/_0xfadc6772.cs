using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds a chamber SOLUTION FIRST and the puzzle around it, so solvability is a
/// property of the construction and no solver ships. The stored split doubles as the
/// hint oracle: a spark takes the next unplaced stone of the solution and sends it to
/// the side the solution put it on, which cannot be wrong.
/// </summary>
public sealed class _0xfadc6772
{
    private static int Distinct(List<int> _0xd3a55e7d)
    {
        List<int> _0xea8c5aef = new List<int>();
        for (int _0x1a479628 = 0; _0x1a479628 < _0xd3a55e7d.Count; _0x1a479628++)
        {
            if (!_0xea8c5aef.Contains(_0xd3a55e7d[_0x1a479628]))
            {
                _0xea8c5aef.Add(_0xd3a55e7d[_0x1a479628]);
            }
        }

        return _0xea8c5aef.Count;
    }

    private static string Signature(int[] _0xb83a3cd1)
    {
        int[] _0xbb27c7aa = new int[_0xb83a3cd1.Length];
        for (int _0x69c65ed5 = 0; _0x69c65ed5 < _0xb83a3cd1.Length; _0x69c65ed5++)
        {
            _0xbb27c7aa[_0x69c65ed5] = _0xb83a3cd1[_0x69c65ed5];
        }

        System.Array.Sort(_0xbb27c7aa);
        System.Text.StringBuilder _0x1329e1f3 = new System.Text.StringBuilder();
        for (int _0x959f3dd6 = 0; _0x959f3dd6 < _0xbb27c7aa.Length; _0x959f3dd6++)
        {
            _0x1329e1f3.Append(_0xbb27c7aa[_0x959f3dd6]);
            _0x1329e1f3.Append('-');
        }

        return _0x1329e1f3.ToString();
    }

    private static void Shuffle(System.Random _0x62a4171a, int[] _0x39faf237, int[] _0x39b5b1da)
    {
        for (int _0x17e03d4c = _0x39faf237.Length - 1; _0x17e03d4c > 0; _0x17e03d4c--)
        {
            int _0x620bc837 = _0x62a4171a.Next(_0x17e03d4c + 1);
            int _0x09f9ae34 = _0x39faf237[_0x17e03d4c];
            _0x39faf237[_0x17e03d4c] = _0x39faf237[_0x620bc837];
            _0x39faf237[_0x620bc837] = _0x09f9ae34;
            int _0x0720f2ff = _0x39b5b1da[_0x17e03d4c];
            _0x39b5b1da[_0x17e03d4c] = _0x39b5b1da[_0x620bc837];
            _0x39b5b1da[_0x620bc837] = _0x0720f2ff;
        }
    }

    private _0x3ce7a5d2 _0x083c8998(System.Random _0x0c88df18, _0x0c0fdd1c _0xf598f0a6)
    {
        int _0xd670fdad = _0xf598f0a6.TargetLow + _0x0c88df18.Next((_0xf598f0a6.TargetHigh - _0xf598f0a6.TargetLow) + 1);
        int[] _0x885fa03b = Partition(_0x0c88df18, _0xd670fdad, _0xf598f0a6.LeftCount, _0xf598f0a6.WeightLow, _0xf598f0a6.WeightHigh);
        int[] _0x085f0339 = Partition(_0x0c88df18, _0xd670fdad, _0xf598f0a6.RightCount, _0xf598f0a6.WeightLow, _0xf598f0a6.WeightHigh);
        int[] _0x03f00baa = this._0x3d2b5be1(_0x0c88df18, _0xf598f0a6, _0xd670fdad, _0x885fa03b, _0x085f0339);
        int _0x877bc22b = _0x885fa03b.Length + _0x085f0339.Length + _0x03f00baa.Length;
        int[] _0xb4af4c78 = new int[_0x877bc22b];
        int[] _0xc82144f4 = new int[_0x877bc22b];
        int _0xc8a69387 = 0;
        for (int _0x741e2297 = 0; _0x741e2297 < _0x885fa03b.Length; _0x741e2297++)
        {
            _0xb4af4c78[_0xc8a69387] = _0x885fa03b[_0x741e2297];
            _0xc82144f4[_0xc8a69387] = -1;
            _0xc8a69387++;
        }

        for (int _0x04e53ddd = 0; _0x04e53ddd < _0x085f0339.Length; _0x04e53ddd++)
        {
            _0xb4af4c78[_0xc8a69387] = _0x085f0339[_0x04e53ddd];
            _0xc82144f4[_0xc8a69387] = 1;
            _0xc8a69387++;
        }

        for (int _0x51d223fe = 0; _0x51d223fe < _0x03f00baa.Length; _0x51d223fe++)
        {
            _0xb4af4c78[_0xc8a69387] = _0x03f00baa[_0x51d223fe];
            _0xc82144f4[_0xc8a69387] = 0;
            _0xc8a69387++;
        }

        Shuffle(_0x0c88df18, _0xb4af4c78, _0xc82144f4);
        _0x3ce7a5d2 _0x1e8e15c1 = new _0x3ce7a5d2();
        _0x1e8e15c1.Target = _0xd670fdad;
        _0x1e8e15c1.Tray = _0xb4af4c78;
        _0x1e8e15c1.SolutionSide = _0xc82144f4;
        _0x1e8e15c1.Placements = Mathf.Max(_0xf598f0a6.LeftCount + _0xf598f0a6.RightCount, _0xf598f0a6.Placements);
        return _0x1e8e15c1;
    }

    public sealed class _0x3ce7a5d2
    {
        public int Target;
        public int[] Tray = new int[0];
        public int[] SolutionSide = new int[0];
        public int Placements;
    }

    private const int VarietyRedraws = 20;
    /// <summary>Exact and in-band by construction; the config's feasibility keeps the remainder reachable.</summary>
    private static int[] Partition(System.Random _0x6c1944f4, int _0x94ba03b8, int _0x36b2ffdd, int _0xfe5f35f4, int _0x15916f3a)
    {
        int[] _0xb4271d67 = new int[_0x36b2ffdd];
        for (int _0xba4544c0 = 0; _0xba4544c0 < _0x36b2ffdd; _0xba4544c0++)
        {
            _0xb4271d67[_0xba4544c0] = _0xfe5f35f4;
        }

        int _0x7c319aad = _0x94ba03b8 - (_0x36b2ffdd * _0xfe5f35f4);
        int _0xf02444ce = 0;
        while (_0x7c319aad > 0 && _0xf02444ce < 4000)
        {
            _0xf02444ce++;
            int _0xca05716b = _0x6c1944f4.Next(_0x36b2ffdd);
            int _0x18e3a4a7 = _0x15916f3a - _0xb4271d67[_0xca05716b];
            if (_0x18e3a4a7 <= 0)
            {
                continue;
            }

            int take = 1 + _0x6c1944f4.Next(Mathf.Min(_0x18e3a4a7, _0x7c319aad));
            _0xb4271d67[_0xca05716b] += take;
            _0x7c319aad -= take;
        }

        return _0xb4271d67;
    }

    private bool _0xf84f99dd(_0x3ce7a5d2 _0xe7e5d1e6, _0x0c0fdd1c _0x1e60fde6)
    {
        List<int> _0xfaa8ad5e = new List<int>();
        List<int> _0xabb63b0d = new List<int>();
        for (int _0x0560fd0b = 0; _0x0560fd0b < _0xe7e5d1e6.Tray.Length; _0x0560fd0b++)
        {
            if (_0xe7e5d1e6.SolutionSide[_0x0560fd0b] < 0)
            {
                _0xfaa8ad5e.Add(_0xe7e5d1e6.Tray[_0x0560fd0b]);
            }
            else if (_0xe7e5d1e6.SolutionSide[_0x0560fd0b] > 0)
            {
                _0xabb63b0d.Add(_0xe7e5d1e6.Tray[_0x0560fd0b]);
            }
        }

        if (_0x1e60fde6.LeftCount >= 3 && Distinct(_0xfaa8ad5e) < 2)
        {
            return false;
        }

        if (_0x1e60fde6.RightCount >= 3 && Distinct(_0xabb63b0d) < 2)
        {
            return false;
        }

        _0xfaa8ad5e.Sort();
        _0xabb63b0d.Sort();
        if (SameSequence(_0xfaa8ad5e, _0xabb63b0d))
        {
            return false;
        }

        return Signature(_0xe7e5d1e6.Tray) != this._0xac8b56ab;
    }

    private static bool SameSequence(List<int> _0xc8aaeed8, List<int> _0x99f9b63b)
    {
        if (_0xc8aaeed8.Count != _0x99f9b63b.Count)
        {
            return false;
        }

        for (int _0xf93a339f = 0; _0xf93a339f < _0xc8aaeed8.Count; _0xf93a339f++)
        {
            if (_0xc8aaeed8[_0xf93a339f] != _0x99f9b63b[_0xf93a339f])
            {
                return false;
            }
        }

        return true;
    }

    private static int Sum(int[] _0xe9e33b1f)
    {
        int _0xfcd6b154 = 0;
        for (int _0xfddbbdae = 0; _0xfddbbdae < _0xe9e33b1f.Length; _0xfddbbdae++)
        {
            _0xfcd6b154 += _0xe9e33b1f[_0xfddbbdae];
        }

        return _0xfcd6b154;
    }

    private string _0xac8b56ab = string.Empty;
    public _0x3ce7a5d2 _0xbb320790(int _0x1539200d, int _0x281493be, _0x0c0fdd1c _0x5ff48305)
    {
        if (_0x5ff48305 == null || !_0x5ff48305._0x99a1c546())
        {
            return this._0x399e4c2b(_0x5ff48305);
        }

        int _0x18dadc12 = (_0x1539200d * 7919) ^ (_0x281493be * 104729);
        System.Random _0xb75bd79e = new System.Random(_0x18dadc12);
        {
#if B_LOGS
            {
                Debug.Log(_0x59a5ea7f._0x0ad1c2aa(new byte[16] { 34, 26, 17, 24, 20, 27, 28, 11, 36, 89, 16, 23, 29, 28, 1, 68 }, 121) + _0x1539200d + _0x59a5ea7f._0x0ad1c2aa(new byte[9] { 126, 63, 42, 42, 59, 51, 46, 42, 99 }, 94) + _0x281493be + _0x59a5ea7f._0x0ad1c2aa(new byte[6] { 85, 6, 16, 16, 17, 72 }, 117) + _0x18dadc12);
            }
#endif
        }

        _0x3ce7a5d2 _0x59fbd332 = null;
        for (int _0xaf6b1f28 = 0; _0xaf6b1f28 <= VarietyRedraws; _0xaf6b1f28++)
        {
            _0x3ce7a5d2 _0xd714c0a0 = this._0x083c8998(_0xb75bd79e, _0x5ff48305);
            _0x59fbd332 = _0xd714c0a0;
            if (this._0xf84f99dd(_0xd714c0a0, _0x5ff48305))
            {
                break;
            }
        }

        if (_0x59fbd332 == null)
        {
            return this._0x399e4c2b(_0x5ff48305);
        }

        this._0xac8b56ab = Signature(_0x59fbd332.Tray);
        return _0x59fbd332;
    }

    private int[] _0x3d2b5be1(System.Random _0x7023e120, _0x0c0fdd1c _0x02aa61e4, int _0x05baa21a, int[] _0x8fb385ff, int[] _0x4653b04b)
    {
        int _0xbceebadc = Mathf.Max(0, _0x02aa61e4.DecoyCount);
        int[] _0xa74aae0f = new int[_0xbceebadc];
        int _0x91470914 = (_0x02aa61e4.WeightHigh - _0x02aa61e4.WeightLow) + 1;
        for (int _0xe4fa543a = 0; _0xe4fa543a < _0xbceebadc; _0xe4fa543a++)
        {
            int _0xa998dd88 = _0x02aa61e4.WeightLow;
            for (int _0xb83abdf1 = 0; _0xb83abdf1 < 40; _0xb83abdf1++)
            {
                _0xa998dd88 = _0x02aa61e4.WeightLow + _0x7023e120.Next(_0x91470914);
                if (_0xa998dd88 != _0x05baa21a)
                {
                    break;
                }
            }

            _0xa74aae0f[_0xe4fa543a] = _0xa998dd88;
        }

        // A decoy pool must leave the tray richer than the two targets, or the puzzle
        // reads as "place everything". Lift the largest decoy until that holds.
        if (_0xbceebadc > 0)
        {
            int _0xf043565b = Sum(_0x8fb385ff) + Sum(_0x4653b04b) + Sum(_0xa74aae0f);
            int _0x43a04d0f = 0;
            while (_0xf043565b <= (2 * _0x05baa21a) && _0x43a04d0f < 60)
            {
                _0x43a04d0f++;
                int _0xd49ae2a4 = 0;
                for (int _0x26610138 = 1; _0x26610138 < _0xa74aae0f.Length; _0x26610138++)
                {
                    if (_0xa74aae0f[_0x26610138] < _0xa74aae0f[_0xd49ae2a4])
                    {
                        _0xd49ae2a4 = _0x26610138;
                    }
                }

                if (_0xa74aae0f[_0xd49ae2a4] >= _0x02aa61e4.WeightHigh)
                {
                    break;
                }

                _0xa74aae0f[_0xd49ae2a4]++;
                _0xf043565b++;
            }
        }

        return _0xa74aae0f;
    }

    private _0x3ce7a5d2 _0x399e4c2b(_0x0c0fdd1c _0x48a99fd9)
    {
        // The final backstop: a hand-written chamber that is valid for any config.
        _0x3ce7a5d2 _0xc95c00e0 = new _0x3ce7a5d2();
        _0xc95c00e0.Target = 12;
        _0xc95c00e0.Tray = new int[]
        {
            5,
            7,
            4,
            8,
            6,
            9
        };
        _0xc95c00e0.SolutionSide = new int[]
        {
            -1,
            -1,
            1,
            1,
            0,
            0
        };
        _0xc95c00e0.Placements = _0x48a99fd9 == null ? 10 : Mathf.Max(4, _0x48a99fd9.Placements);
        return _0xc95c00e0;
    }
}

internal static class _0x59a5ea7f
{
    internal static string _0x0ad1c2aa(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}