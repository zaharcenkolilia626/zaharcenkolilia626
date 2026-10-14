using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The menu screen. Owns the five chamber seals, the two HUD plates, the objective line
/// and the lit path between cleared seals. A node tap changes three visible things at
/// once - the seal punches, its rim brightens, and the objective rewrites - so the
/// action reads in a still review frame.
/// </summary>
public sealed class _0xbe1ea1f5 : MonoBehaviour
{
    private void _0x14aafc2f()
    {
        if (this._progress == null)
        {
            return;
        }

        int _0x76e1df00 = this._progress._0xbd59124c;
        if (!this._progress._0x31e57292(_0x76e1df00))
        {
            _0x76e1df00 = 0;
            this._progress._0xbd59124c = 0;
        }

        for (int _0x54652958 = 0; _0x54652958 < this._nodes.Length; _0x54652958++)
        {
            if (this._nodes[_0x54652958] == null)
            {
                continue;
            }

            this._nodes[_0x54652958]._0x758f4627(this._progress._0x31e57292(_0x54652958), this._progress._0x5e678cd0(_0x54652958), _0x54652958 == _0x76e1df00);
        }

        for (int _0x3ad420cb = 0; _0x3ad420cb < this._pathSegments.Length; _0x3ad420cb++)
        {
            if (this._pathSegments[_0x3ad420cb] == null)
            {
                continue;
            }

            bool _0xa9b0401d = this._progress._0x5e678cd0(_0x3ad420cb);
            this._pathSegments[_0x3ad420cb].color = _0xa9b0401d ? _0x5789e692.WithAlpha(_0x5789e692.EmberGold, 0.55f) : _0x5789e692.WithAlpha(_0x5789e692.StoneRidge, 0.85f);
        }

        if (this._emberPlate != null)
        {
            int _0x632e133f = this._progress._0x8e49a77c;
            this._emberPlate._0x9a544b68(_0x632e133f.ToString(), _0x632e133f <= 0);
        }

        if (this._sparkPlate != null)
        {
            int _0xdb995d92 = this._progress._0x81cc3fbd;
            this._sparkPlate._0x9a544b68(_0xdb995d92.ToString(), _0xdb995d92 <= 0);
        }

        this._0x8c1f7770(_0x76e1df00);
    }

    [SerializeField]
    private _0x9e4438d2 _progress;
    [SerializeField]
    private _0x7bc16e8b _emberPlate;
    [SerializeField]
    private TextMeshProUGUI _objective;
    [SerializeField]
    private _0xd30f434d _library;
    [SerializeField]
    private _0xde28c715[] _nodes = new _0xde28c715[0];
    private void OnEnable()
    {
        this._0x14aafc2f();
    }

    private void Update()
    {
        if (this._guardian == null)
        {
            return;
        }

        this._0x436496cd += Time.unscaledDeltaTime;
        float _0xa3129c09 = _0x5b45110f.InOutSine(Mathf.PingPong(this._0x436496cd / 4f, 1f));
        this._guardian.color = _0x5789e692.WithAlpha(Color.white, Mathf.Lerp(0.82f, 1f, _0xa3129c09));
    }

    [SerializeField]
    private Image[] _pathSegments = new Image[0];
    private void _0x8c1f7770(int _0x3c1fe96d)
    {
        if (this._objective == null || this._library == null)
        {
            return;
        }

        _0x0c0fdd1c _0xc288021a = this._library._0x055a8058(_0x3c1fe96d);
        string _0x19f01c06 = _0xc288021a.TargetLow == _0xc288021a.TargetHigh ? _0xc288021a.TargetLow.ToString() : _0xc288021a.TargetLow + _0x098af5c7._0x8ceec9ae(new byte[3] { 194, 207, 194 }, 226) + _0xc288021a.TargetHigh;
        _0x6c3fbecd.SetText(this._objective, _0x098af5c7._0x8ceec9ae(new byte[8] { 107, 96, 105, 101, 106, 109, 122, 8 }, 40) + _0xd30f434d.Numeral(_0x3c1fe96d) + _0x098af5c7._0x8ceec9ae(new byte[24] { 161, 161, 172, 161, 161, 204, 192, 213, 194, 201, 161, 195, 206, 213, 201, 161, 209, 192, 207, 210, 161, 192, 213, 161 }, 129) + _0x19f01c06, _0x5789e692.ParchmentMuted);
    }

    [SerializeField]
    private _0x7bc16e8b _sparkPlate;
    /// <summary>Re-reads the saved record and repaints every seal, plate and line.</summary>
    public void _0x45243191()
    {
        this._0x14aafc2f();
    }

    private void Awake()
    {
        for (int _0x3489fab5 = 0; _0x3489fab5 < this._nodes.Length; _0x3489fab5++)
        {
            if (this._nodes[_0x3489fab5] != null)
            {
                this._nodes[_0x3489fab5]._0xf09272d7(this, _0x3489fab5);
            }
        }

        if (this._emberPlate != null)
        {
            this._emberPlate._0xb4955710(_0x098af5c7._0x8ceec9ae(new byte[6] { 36, 44, 35, 36, 51, 50 }, 97), _0x5789e692.EmberGold);
        }

        if (this._sparkPlate != null)
        {
            this._sparkPlate._0xb4955710(_0x098af5c7._0x8ceec9ae(new byte[6] { 139, 136, 153, 138, 147, 139 }, 216), _0x5789e692.SkyLapis);
        }
    }

    private float _0x436496cd;
    [SerializeField]
    private Image _guardian;
    public void _0x3903a517(int _0x0f8094b8)
    {
        if (this._progress == null)
        {
            return;
        }

        this._progress._0xbd59124c = _0x0f8094b8;
        if (_0x0f8094b8 >= 0 && _0x0f8094b8 < this._nodes.Length && this._nodes[_0x0f8094b8] != null)
        {
            this._nodes[_0x0f8094b8]._0x1d1893e6();
        }

        this._0x14aafc2f();
    }
}

internal static class _0x098af5c7
{
    internal static string _0x8ceec9ae(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}