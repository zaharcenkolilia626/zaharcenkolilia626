using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// One pan of the scale: its own rect, the halo that invites a drop, and the running
/// sum beneath it. The pan never moves sideways, so its tap target is where the review
/// scenario expects it on every device.
/// </summary>
public sealed class _0x42a31ba7 : MonoBehaviour, IPointerClickHandler
{
    /// <summary>A stone's resting place on this pan, in frame coordinates.</summary>
    public Vector2 _0x8f94692a(int _0x704e1f32)
    {
        return this._0x71f85a2b + _0x4c79fffb.PanSlot(_0x704e1f32);
    }

    public Vector2 _0x71f85a2b
    {
        get
        {
            return this._rect == null ? Vector2.zero : this._rect.anchoredPosition;
        }
    }

    [SerializeField]
    private Image _glow;
    public int _0xd67c11db
    {
        get
        {
            return this._isLeft ? _0x0e03f787.PlaceLeft : _0x0e03f787.PlaceRight;
        }
    }

    public void _0x522480c6(_0xc001135e _0x843cfe2e)
    {
        this._0xfa44406d = _0x843cfe2e;
        this._0xa7e60659(false);
        _0x6c3fbecd.SetText(this._sum, _0x3064ca99._0xe51de56d(new byte[1] { 140 }, 188), _0x5789e692.EmberGold);
    }

    [SerializeField]
    private RectTransform _rect;
    private float _0xb698d5a8;
    public void OnPointerClick(PointerEventData _0x9f50ad8e)
    {
        if (this._0xfa44406d != null)
        {
            this._0xfa44406d._0xe2d95b47(this);
        }
    }

    public void _0xf44dc616(float _0x9ee4734a)
    {
        if (this._rect != null)
        {
            this._rect.anchoredPosition = new Vector2(_0x4c79fffb.PanCentreX(this._isLeft), _0x9ee4734a);
        }

        if (this._sumRect != null)
        {
            this._sumRect.anchoredPosition = new Vector2(_0x4c79fffb.PanCentreX(this._isLeft), _0x9ee4734a - 240f);
        }
    }

    [SerializeField]
    private TextMeshProUGUI _sum;
    [SerializeField]
    private Image _dish;
    public bool _0xd1f82a0e
    {
        get
        {
            return this._isLeft;
        }
    }

    public void _0xa7e60659(bool _0xa544c18d)
    {
        this._0xaeb33701 = _0xa544c18d;
        if (this._glow != null)
        {
            this._glow.color = _0x5789e692.WithAlpha(_0x5789e692.EmberGoldSoft, _0xa544c18d ? 0.35f : 0f);
        }
    }

    public void _0x93957177()
    {
        this._0xb698d5a8 = 0f;
        this._0xaeb33701 = false;
        if (this._glow != null)
        {
            this._glow.color = _0x5789e692.WithAlpha(_0x5789e692.EmberGoldSoft, 0.9f);
        }
    }

    [SerializeField]
    private bool _isLeft = true;
    private _0xc001135e _0xfa44406d;
    private void Update()
    {
        if (!this._0xaeb33701 || this._glow == null)
        {
            return;
        }

        this._0xb698d5a8 += Time.deltaTime;
        float _0x5b20779b = _0x5b45110f.InOutSine(Mathf.PingPong(this._0xb698d5a8 / 0.9f, 1f));
        this._glow.color = _0x5789e692.WithAlpha(_0x5789e692.EmberGoldSoft, Mathf.Lerp(0.22f, 0.45f, _0x5b20779b));
    }

    private bool _0xaeb33701;
    public void _0xe3faf145(int _0x255360b1)
    {
        _0x6c3fbecd.SetText(this._sum, _0x255360b1.ToString(), _0x5789e692.EmberGold);
    }

    [SerializeField]
    private RectTransform _sumRect;
    public void _0xa950b7a2(bool _0xc64af840)
    {
        if (this._dish != null)
        {
            this._dish.color = _0x5789e692.WithAlpha(Color.white, _0xc64af840 ? 0.55f : 1f);
        }
    }
}

internal static class _0x3064ca99
{
    internal static string _0xe51de56d(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}