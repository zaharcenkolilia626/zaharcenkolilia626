using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game loop: select a stone, set it on a pan, take it back, spend a spark, reset.
/// Nothing here runs on a clock. The only thing that ends a run is the placement budget,
/// and the only two things that spend it are a player tap on a pan and a player tap on
/// RESET - so an untouched board stays on screen indefinitely.
/// </summary>
public sealed class _0xc001135e : MonoBehaviour
{
    public void _0x12bc4fe1(_0x954188ea _0xe8a07def)
    {
        if (this._0x28166bc4)
        {
            return;
        }

        _0x0e03f787 _0xb2563d1f = this._0xb1533cf8();
        if (this._0xb7ccc701 <= 0 || _0xb2563d1f == null)
        {
            if (_0xe8a07def != null)
            {
                _0xe8a07def._0xbd572d5a();
            }

            if (this._hud != null)
            {
                this._hud._0xadd2c807(this._0xb7ccc701 <= 0 ? _0x17886886._0xa8107fb5(new byte[43] { 173, 174, 191, 172, 181, 173, 222, 173, 174, 187, 176, 170, 222, 211, 222, 189, 178, 187, 191, 172, 222, 191, 222, 189, 182, 191, 179, 188, 187, 172, 222, 170, 177, 222, 187, 191, 172, 176, 222, 179, 177, 172, 187 }, 254) : _0x17886886._0xa8107fb5(new byte[35] { 177, 162, 177, 166, 173, 212, 167, 187, 184, 161, 160, 189, 187, 186, 212, 167, 160, 187, 186, 177, 212, 189, 167, 212, 181, 184, 166, 177, 181, 176, 173, 212, 167, 177, 160 }, 244));
            }

            return;
        }

        this._0xb7ccc701--;
        if (this._progress != null)
        {
            this._progress._0x81cc3fbd = this._0xb7ccc701;
        }

        this._0x2c8318aa();
        this._0xa888886b(_0xb2563d1f);
        if (this._hud != null)
        {
            this._hud._0x938213cb();
        }

        this._0xd8c93227();
    }

    private void _0x2c8318aa()
    {
        if (this._0x8f6e2ad2 == null)
        {
            return;
        }

        this._0x8f6e2ad2._0x0a8fa1b0(false);
        this._0x8f6e2ad2._0xc96b743b(_0x4c79fffb.TraySlot(this._0x8f6e2ad2._0x62722c74, this._0x4937cc44.Tray.Length), false);
        this._0x8f6e2ad2 = null;
        this._0x2b22e7ac(false);
    }

    [SerializeField]
    private _0x954188ea _resetButton;
    private Vector2 _0x3e0fd390;
    private Vector2 _0x8a18e1fb;
    private int _0x96a0ea58;
    private void _0xdcf09d20(_0x0e03f787 _0xd7ac42b6)
    {
        // Taking a stone back is free: only setting one down costs the budget.
        _0xd7ac42b6._0xa59abd50(this._0xc575aa79);
        _0xd7ac42b6._0xcb658c62(_0x0e03f787.PlaceTray, _0xd7ac42b6._0x62722c74, _0x4c79fffb.TraySlot(_0xd7ac42b6._0x62722c74, this._0x4937cc44.Tray.Length), false);
        this._0x80550f4f(_0x0e03f787.PlaceLeft);
        this._0x80550f4f(_0x0e03f787.PlaceRight);
        this._0x3a7aceed(false);
    }

    private void _0x2b22e7ac(bool _0x483a4d5d)
    {
        if (this._board == null)
        {
            return;
        }

        this._board._0xc0741c6b._0xa7e60659(_0x483a4d5d);
        this._board._0x22740ead._0xa7e60659(_0x483a4d5d);
    }

    /// <summary>The hint oracle: the stored split, so a spark can never point at a wrong pan.</summary>
    private _0x0e03f787 _0xb1533cf8()
    {
        for (int _0xf6d1166e = 0; _0xf6d1166e < this._0xc393944f.Count; _0xf6d1166e++)
        {
            if (this._0xc393944f[_0xf6d1166e]._0x1251fa69 != 0 && this._0xc393944f[_0xf6d1166e]._0x0c33239f == _0x0e03f787.PlaceTray)
            {
                return this._0xc393944f[_0xf6d1166e];
            }
        }

        return null;
    }

    private _0xfadc6772._0x3ce7a5d2 _0x4937cc44;
    public void _0xc1889a37()
    {
        if (this._pops == null || this._0x4937cc44 == null)
        {
            return;
        }

        int _0x7b8b3b8e = 0;
        for (int _0x08ce46cd = 0; _0x08ce46cd < this._0xc393944f.Count; _0x08ce46cd++)
        {
            if (this._0xc393944f[_0x08ce46cd]._0x0c33239f != _0x0e03f787.PlaceTray)
            {
                _0x7b8b3b8e++;
            }
        }

        this._pops._0x5d2666bd(this._0x4937cc44.Target, _0x7b8b3b8e, this._0xc393944f.Count, this._0xb7ccc701);
    }

    /// <summary>Runs after the board has turned, so a resting stone never lags its pan by a frame.</summary>
    private void LateUpdate()
    {
        if (this._board == null || this._board._0xc0741c6b == null || this._board._0x22740ead == null)
        {
            return;
        }

        for (int _0x436f43c4 = 0; _0x436f43c4 < this._0xc393944f.Count; _0x436f43c4++)
        {
            _0x0e03f787 _0x9409faa3 = this._0xc393944f[_0x436f43c4];
            if (_0x9409faa3._0x0c33239f == _0x0e03f787.PlaceTray)
            {
                continue;
            }

            _0x42a31ba7 _0x00fee8b2 = _0x9409faa3._0x0c33239f == _0x0e03f787.PlaceLeft ? this._board._0xc0741c6b : this._board._0x22740ead;
            _0x9409faa3._0xe24ca8a9(_0x00fee8b2._0x8f94692a(_0x9409faa3._0x39bcc8e7));
        }
    }

    [SerializeField]
    private _0xbb555dc0 _hud;
    private void _0xa888886b(_0x0e03f787 _0x7d256574)
    {
        this._0x233df34a = _0x7d256574;
        this._0xd55da12a = 0f;
        if (this._sparkMote == null || this._sparkButton == null)
        {
            this._0xa116c022();
            return;
        }

        this._0xf42d16c8 = this._0x3e0fd390;
        this._0x8a18e1fb = _0x4c79fffb.TraySlot(_0x7d256574._0x62722c74, this._0x4937cc44.Tray.Length);
        this._sparkMote.gameObject.SetActive(true);
        this._sparkMote.anchoredPosition = this._0xf42d16c8;
    }

    /// <summary>Stones are built in Awake so every view that reads them is already populated.</summary>
    private void _0x41ab89b9()
    {
        if (this._stonePrefab == null || this._stoneLayer == null || this._0x4937cc44 == null)
        {
            return;
        }

        bool _0x15424451 = this._progress != null && this._progress._0x3a71d345;
        for (int _0x84bd76bc = 0; _0x84bd76bc < this._0x4937cc44.Tray.Length; _0x84bd76bc++)
        {
            _0x0e03f787 _0x3488d7dc = Instantiate(this._stonePrefab, this._stoneLayer);
            _0x3488d7dc._0xc096511f(this, this._0x4937cc44.Tray[_0x84bd76bc], this._0x4937cc44.SolutionSide[_0x84bd76bc], _0x84bd76bc, this._0xc575aa79);
            _0x3488d7dc._0xf81ab635(_0x15424451);
            _0x3488d7dc._0xcb658c62(_0x0e03f787.PlaceTray, _0x84bd76bc, _0x4c79fffb.TraySlot(_0x84bd76bc, this._0x4937cc44.Tray.Length), true);
            this._0xc393944f.Add(_0x3488d7dc);
        }
    }

    private float _0x668829b6 = -1f;
    private _0x0e03f787 _0x233df34a;
    public void _0x6e9f8dcb()
    {
        if (this._0x28166bc4)
        {
            return;
        }

        this._0x2c8318aa();
        bool _0x6ff0b058 = false;
        for (int _0xd766bd9b = 0; _0xd766bd9b < this._0xc393944f.Count; _0xd766bd9b++)
        {
            if (this._0xc393944f[_0xd766bd9b]._0x0c33239f == _0x0e03f787.PlaceTray)
            {
                continue;
            }

            _0x6ff0b058 = true;
            this._0xc393944f[_0xd766bd9b]._0xa59abd50(this._0xc575aa79);
            this._0xc393944f[_0xd766bd9b]._0xcb658c62(_0x0e03f787.PlaceTray, this._0xc393944f[_0xd766bd9b]._0x62722c74, _0x4c79fffb.TraySlot(this._0xc393944f[_0xd766bd9b]._0x62722c74, this._0x4937cc44.Tray.Length), false);
        }

        if (!_0x6ff0b058)
        {
            if (this._resetButton != null)
            {
                this._resetButton._0xbd572d5a();
            }

            if (this._hud != null)
            {
                this._hud._0xadd2c807(_0x17886886._0xa8107fb5(new byte[34] { 71, 84, 71, 80, 91, 34, 81, 86, 77, 76, 71, 34, 75, 81, 34, 67, 78, 80, 71, 67, 70, 91, 34, 75, 76, 34, 86, 74, 71, 34, 86, 80, 67, 91 }, 2));
            }

            return;
        }

        this._0x1a8babf2();
        this._0x3a7aceed(false);
    }

    private int _0xc575aa79;
    private void _0x1a8babf2()
    {
        this._0x96a0ea58 = Mathf.Max(0, this._0x96a0ea58 - 1);
        if (this._hud != null)
        {
            this._hud._0x9bf5b7e1();
        }

        this._0xd8c93227();
    }

    private void _0x04329cbe()
    {
        this._0xd55da12a = -1f;
        if (this._sparkMote != null)
        {
            this._sparkMote.gameObject.SetActive(false);
        }
    }

    private bool _0x52764e06;
    [SerializeField]
    private _0xd30f434d _library;
    private void Update()
    {
        if (this._0xd55da12a >= 0f && this._sparkMote != null)
        {
            this._0xd55da12a += Time.deltaTime;
            float _0x16fbcd58 = _0x5b45110f.Clamp01(this._0xd55da12a / 0.4f);
            this._sparkMote.anchoredPosition = _0x5b45110f.Lerp(this._0xf42d16c8, this._0x8a18e1fb, _0x5b45110f.OutCubic(_0x16fbcd58));
            if (_0x16fbcd58 >= 1f)
            {
                this._0xa116c022();
            }
        }

        if (this._0x668829b6 > 0f)
        {
            this._0x668829b6 -= Time.deltaTime;
            if (this._0x668829b6 <= 0f)
            {
                this._0x668829b6 = -1f;
                this._0xccf1930c();
            }
        }
    }

    public void _0x11afd01e(_0x0e03f787 _0xd1968e2f)
    {
        if (this._0x28166bc4 || _0xd1968e2f == null)
        {
            return;
        }

        if (_0xd1968e2f._0x0c33239f != _0x0e03f787.PlaceTray)
        {
            this._0xdcf09d20(_0xd1968e2f);
            return;
        }

        if (this._0x8f6e2ad2 == _0xd1968e2f)
        {
            this._0x2c8318aa();
            return;
        }

        this._0x2c8318aa();
        this._0x8f6e2ad2 = _0xd1968e2f;
        _0xd1968e2f._0x0a8fa1b0(true);
        _0xd1968e2f._0xc96b743b(_0x4c79fffb.TraySlot(_0xd1968e2f._0x62722c74, this._0x4937cc44.Tray.Length) + new Vector2(0f, 60f), false);
        this._0x2b22e7ac(true);
    }

    private void _0xb97a0dda()
    {
        if (this._0x28166bc4 || this._0x4937cc44 == null)
        {
            return;
        }

        int _0x4e1f8a50 = this._0x823b829c(_0x0e03f787.PlaceLeft);
        int _0xa35c7ac1 = this._0x823b829c(_0x0e03f787.PlaceRight);
        if (_0x4e1f8a50 == this._0x4937cc44.Target && _0xa35c7ac1 == this._0x4937cc44.Target)
        {
            this._0x28166bc4 = true;
            this._0x52764e06 = true;
            this._0x668829b6 = 0.9f;
            this._board._0xb5b6573b();
            this._board._0xc0741c6b._0x93957177();
            this._board._0x22740ead._0x93957177();
            return;
        }

        if (this._0x96a0ea58 <= 0)
        {
            this._0x28166bc4 = true;
            this._0x52764e06 = false;
            this._0x668829b6 = 0.7f;
            this._board._0xc0741c6b._0xa950b7a2(true);
            this._board._0x22740ead._0xa950b7a2(true);
            this._board._0x253a3445(_0x4c79fffb.MaxTiltDegrees * (_0xa35c7ac1 >= _0x4e1f8a50 ? 1f : -1f), false);
        }
    }

    private void Awake()
    {
        if (this._progress == null || this._library == null)
        {
            return;
        }

        this._0x75b4af1c = this._progress._0xbd59124c;
        _0x0c0fdd1c _0xa8ac4702 = this._library._0x055a8058(this._0x75b4af1c);
        this._0x4937cc44 = this._0x8ab0371f._0xbb320790(this._0x75b4af1c, this._progress._0xbd15fbd6(this._0x75b4af1c), _0xa8ac4702);
        this._0x96a0ea58 = this._0x4937cc44.Placements;
        this._0xb7ccc701 = this._progress._0x81cc3fbd;
        this._0xc575aa79 = Mathf.RoundToInt(_0x4c79fffb.TrayStoneSize(this._0x4937cc44.Tray.Length));
        if (this._board != null)
        {
            this._board._0x1b2ca227(this);
        }

        this._0x41ab89b9();
    }

    private Vector2 _0xf42d16c8;
    private void _0x80550f4f(int _0x17e1b56b)
    {
        _0x42a31ba7 _0xf4c71851 = _0x17e1b56b == _0x0e03f787.PlaceLeft ? this._board._0xc0741c6b : this._board._0x22740ead;
        int _0xea033b74 = 0;
        for (int _0x69868729 = 0; _0x69868729 < this._0xc393944f.Count; _0x69868729++)
        {
            if (this._0xc393944f[_0x69868729]._0x0c33239f != _0x17e1b56b)
            {
                continue;
            }

            this._0xc393944f[_0x69868729]._0xcb658c62(_0x17e1b56b, _0xea033b74, _0xf4c71851._0x8f94692a(_0xea033b74), false);
            _0xea033b74++;
        }
    }

    private int _0xb7ccc701;
    private void _0xccf1930c()
    {
        int _0x5724d7d2 = this._0x823b829c(_0x0e03f787.PlaceLeft);
        int _0x48b50eb0 = this._0x823b829c(_0x0e03f787.PlaceRight);
        if (!this._0x52764e06)
        {
            if (this._pops != null)
            {
                this._pops._0x37f07999(this._0x4937cc44.Target, _0x5724d7d2, _0x48b50eb0);
            }

            return;
        }

        int _0xd2b10c5a = this._0x4937cc44.Target + (5 * this._0x96a0ea58);
        bool _0x6f7e86aa = this._progress != null && !this._progress._0x5e678cd0(this._0x75b4af1c);
        if (_0x6f7e86aa)
        {
            _0xd2b10c5a += 10;
        }

        int _0x6f3a2bbe = 0;
        if (this._progress != null)
        {
            List<int> _0xde1981b9 = new List<int>();
            for (int _0xb9e12963 = 0; _0xb9e12963 < this._0xc393944f.Count; _0xb9e12963++)
            {
                if (this._0xc393944f[_0xb9e12963]._0x0c33239f != _0x0e03f787.PlaceTray)
                {
                    _0xde1981b9.Add(this._0xc393944f[_0xb9e12963]._0x6707c8dd);
                }
            }

            _0x6f3a2bbe = _0xde1981b9.Count;
            this._progress._0x9c879244(_0xde1981b9.ToArray());
            this._progress._0x8e49a77c = this._progress._0x8e49a77c + _0xd2b10c5a;
            this._progress._0x81cc3fbd = this._0xb7ccc701 + 2;
            this._progress._0x3530f7b1(this._0x75b4af1c);
        }

        if (this._pops != null)
        {
            this._pops._0xed73e46b(this._0x4937cc44.Target, _0xd2b10c5a, _0x6f3a2bbe, this._0x75b4af1c);
        }
    }

    private int _0x823b829c(int _0x2f5db554)
    {
        int _0x091cbcb2 = 0;
        for (int _0xdfa0fa52 = 0; _0xdfa0fa52 < this._0xc393944f.Count; _0xdfa0fa52++)
        {
            if (this._0xc393944f[_0xdfa0fa52]._0x0c33239f == _0x2f5db554)
            {
                _0x091cbcb2 += this._0xc393944f[_0xdfa0fa52]._0x6707c8dd;
            }
        }

        return _0x091cbcb2;
    }

    [SerializeField]
    private _0x954188ea _sparkButton;
    private int _0x75b4af1c;
    private readonly _0xfadc6772 _0x8ab0371f = new _0xfadc6772();
    private void _0xa116c022()
    {
        this._0x04329cbe();
        _0x0e03f787 _0x35479d75 = this._0x233df34a;
        this._0x233df34a = null;
        if (_0x35479d75 == null || this._0x28166bc4)
        {
            return;
        }

        _0x35479d75._0x7d9f10fe();
        this._0x8f6e2ad2 = _0x35479d75;
        this._0x4cb228ac(_0x35479d75, _0x35479d75._0x1251fa69);
    }

    private void _0x3a7aceed(bool _0x34b729f9)
    {
        if (this._board == null || this._0x4937cc44 == null)
        {
            return;
        }

        int _0xaab36280 = this._0x823b829c(_0x0e03f787.PlaceLeft);
        int _0xf33a84dd = this._0x823b829c(_0x0e03f787.PlaceRight);
        this._board._0xc0741c6b._0xe3faf145(_0xaab36280);
        this._board._0x22740ead._0xe3faf145(_0xf33a84dd);
        this._board._0x253a3445(_0x4c79fffb.Tilt(_0xaab36280, _0xf33a84dd, this._0x4937cc44.Target), _0x34b729f9);
    }

    [SerializeField]
    private _0x1f9a8ba2 _pops;
    private void _0xd8c93227()
    {
        if (this._hud != null)
        {
            this._hud._0x7cf6a560(this._0x96a0ea58, this._0xb7ccc701, this._progress == null ? 0 : this._progress._0x8e49a77c);
        }

        if (this._sparkButton != null)
        {
            this._sparkButton._0x710a4349(_0x17886886._0xa8107fb5(new byte[7] { 233, 234, 251, 232, 241, 154, 154 }, 186) + this._0xb7ccc701);
        }

        if (this._resetButton != null)
        {
            this._resetButton._0x710a4349(_0x17886886._0xa8107fb5(new byte[5] { 13, 26, 12, 26, 11 }, 95));
        }
    }

    private _0x0e03f787 _0x8f6e2ad2;
    [SerializeField]
    private _0x0e03f787 _stonePrefab;
    private float _0xd55da12a = -1f;
    private int _0x0db24e89(int _0x297f7700)
    {
        int _0x49c27796 = 0;
        for (int _0x2e77da25 = 0; _0x2e77da25 < this._0xc393944f.Count; _0x2e77da25++)
        {
            if (this._0xc393944f[_0x2e77da25]._0x0c33239f == _0x297f7700)
            {
                _0x49c27796++;
            }
        }

        return _0x49c27796;
    }

    private void Start()
    {
        if (this._hud != null)
        {
            this._hud._0x3fee813e(this._0x75b4af1c, this._0x4937cc44 == null ? 0 : this._0x4937cc44.Target);
        }

        if (this._sparkMote != null)
        {
            this._0x3e0fd390 = this._sparkMote.anchoredPosition;
        }

        this._0xd8c93227();
        this._0x3a7aceed(true);
        this._0x04329cbe();
    }

    public void _0xe2d95b47(_0x42a31ba7 _0x082eae43)
    {
        if (this._0x28166bc4 || _0x082eae43 == null)
        {
            return;
        }

        if (this._0x8f6e2ad2 == null)
        {
            if (this._hud != null)
            {
                this._hud._0xadd2c807(_0x17886886._0xa8107fb5(new byte[34] { 13, 24, 9, 121, 24, 121, 10, 13, 22, 23, 28, 121, 31, 16, 11, 10, 13, 121, 116, 121, 13, 17, 28, 23, 121, 13, 24, 9, 121, 24, 121, 9, 24, 23 }, 89));
            }

            return;
        }

        if (this._0x0db24e89(_0x082eae43._0xd67c11db) >= _0x4c79fffb._0xf11a6f5c)
        {
            if (this._hud != null)
            {
                this._hud._0xadd2c807(_0x17886886._0xa8107fb5(new byte[42] { 85, 73, 64, 85, 33, 81, 64, 79, 33, 72, 82, 33, 71, 84, 77, 77, 33, 44, 33, 85, 64, 74, 68, 33, 64, 33, 82, 85, 78, 79, 68, 33, 67, 64, 66, 74, 33, 71, 72, 83, 82, 85 }, 1));
            }

            return;
        }

        this._0x4cb228ac(this._0x8f6e2ad2, _0x082eae43._0xd67c11db);
    }

    private void _0x4cb228ac(_0x0e03f787 _0xa500f728, int _0x583197d5)
    {
        int _0x661514d2 = this._0x0db24e89(_0x583197d5);
        _0x42a31ba7 _0xe856c438 = _0x583197d5 == _0x0e03f787.PlaceLeft ? this._board._0xc0741c6b : this._board._0x22740ead;
        _0xa500f728._0x0a8fa1b0(false);
        _0xa500f728._0xa59abd50(_0x4c79fffb.PanStoneSize);
        _0xa500f728._0xcb658c62(_0x583197d5, _0x661514d2, _0xe856c438._0x8f94692a(_0x661514d2), false);
        this._0x8f6e2ad2 = null;
        this._0x2b22e7ac(false);
        this._0x1a8babf2();
        this._0x3a7aceed(false);
        this._0xb97a0dda();
    }

    private readonly List<_0x0e03f787> _0xc393944f = new List<_0x0e03f787>();
    private bool _0x28166bc4;
    [SerializeField]
    private _0x9e4438d2 _progress;
    [SerializeField]
    private _0x1f84bffb _board;
    [SerializeField]
    private RectTransform _stoneLayer;
    [SerializeField]
    private RectTransform _sparkMote;
}

internal static class _0x17886886
{
    internal static string _0xa8107fb5(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}