using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// One ember stone. It carries its weight, where it currently sits, and the eased
/// travel between those places. Every stone lives in one shared layer in frame
/// coordinates, so moving between the tray and a pan is a position change and never a
/// reparenting - which keeps its scale and draw order stable.
/// </summary>
public sealed class _0x0e03f787 : MonoBehaviour, IPointerClickHandler
{
    public void _0xa59abd50(float _0xc4134984)
    {
        this._0x1f667d8a = _0xc4134984;
        if (this._rect != null)
        {
            this._rect.sizeDelta = new Vector2(_0xc4134984, _0xc4134984);
        }
    }

    public void OnPointerClick(PointerEventData _0xc2edd213)
    {
        if (this._0x9ec3a3b7 != null)
        {
            this._0x9ec3a3b7._0x11afd01e(this);
        }
    }

    public const int PlaceRight = 1;
    private Vector2 _0x5b040957;
    public void _0xc96b743b(Vector2 _0xd802476a, bool _0x6b2df235)
    {
        if (this._rect == null)
        {
            return;
        }

        if (_0x6b2df235)
        {
            this._rect.anchoredPosition = _0xd802476a;
            this._0x4692879b = _0xd802476a;
            this._0x5b040957 = _0xd802476a;
            this._0x6d579478 = 1f;
            return;
        }

        this._0x4692879b = this._rect.anchoredPosition;
        this._0x5b040957 = _0xd802476a;
        this._0x6d579478 = 0f;
    }

    public void _0x7d9f10fe()
    {
        this._0xd09cd929 = 1f;
    }

    public const int PlaceTray = 0;
    [SerializeField]
    private TextMeshProUGUI _numeral;
    public int _0x0c33239f { get; private set; }

    [SerializeField]
    private Image _face;
    private bool _0x9bc96339;
    private void Update()
    {
        if (this._rect == null || this._body == null)
        {
            return;
        }

        if (this._0x6d579478 < 1f)
        {
            this._0x6d579478 = Mathf.Min(1f, this._0x6d579478 + (Time.deltaTime / this._0xf9af2507));
            this._rect.anchoredPosition = _0x5b45110f.Lerp(this._0x4692879b, this._0x5b040957, _0x5b45110f.OutCubic(this._0x6d579478));
            if (this._0x6d579478 >= 1f)
            {
                this._0x75a941a1 = 1f;
            }
        }

        float _0x43c47b59 = this._0x9bc96339 ? 1.12f : 1f;
        if (this._0x75a941a1 > 0f)
        {
            this._0x75a941a1 = Mathf.Max(0f, this._0x75a941a1 - (Time.deltaTime * 5.5f));
            _0x43c47b59 *= 1f + (_0x5b45110f.Punch(1f - this._0x75a941a1) * 0.14f);
        }

        this._body.localScale = new Vector3(_0x43c47b59, _0x43c47b59, 1f);
        if (this._0xd09cd929 > 0f)
        {
            this._0xd09cd929 = Mathf.Max(0f, this._0xd09cd929 - (Time.deltaTime * 4f));
            if (this._face != null)
            {
                this._face.color = Color.Lerp(Color.white, _0x5789e692.SkyLapis, _0x5b45110f.Punch(1f - this._0xd09cd929) * 2f);
            }
        }
    }

    public void _0xf81ab635(bool _0x714398cf)
    {
        if (this._face != null)
        {
            this._face.color = _0x714398cf ? _0x5789e692.EmberGoldSoft : Color.white;
        }
    }

    [SerializeField]
    private RectTransform _rect;
    private void Start()
    {
        // The numeral is the one label the contrast stage cannot see in any file, so it
        // gets its own outlined material instance here - after TMP has woken up.
        _0x6c3fbecd.ApplyOutline(this._numeral, 0.18f);
    }

    private float _0x1f667d8a = 160f;
    /// <summary>The tray slot this stone was dealt into. It never changes, so a stone always has a home.</summary>
    public int _0x62722c74 { get; private set; }

    private _0xc001135e _0x9ec3a3b7;
    public void _0xeae32249()
    {
        this._0x75a941a1 = 1f;
    }

    public int _0x1251fa69 { get; private set; }

    [SerializeField]
    private Image _ring;
    /// <summary>Keeps a resting stone glued to a slot that moved under it as the beam turned.</summary>
    public void _0xe24ca8a9(Vector2 _0xcac3fc71)
    {
        if (this._rect == null)
        {
            return;
        }

        if (this._0x6d579478 < 1f)
        {
            this._0x5b040957 = _0xcac3fc71;
            return;
        }

        this._rect.anchoredPosition = _0xcac3fc71;
        this._0x4692879b = _0xcac3fc71;
        this._0x5b040957 = _0xcac3fc71;
    }

    private Vector2 _0x4692879b;
    public int _0x6707c8dd { get; private set; }

    private float _0x6d579478 = 1f;
    public bool _0x53a57ef7
    {
        get
        {
            return this._0x6d579478 < 1f;
        }
    }

    public void _0xcb658c62(int _0xea88b5c5, int _0xe0ed2bd5, Vector2 _0xc47d916f, bool _0xe95d4eee)
    {
        this._0x0c33239f = _0xea88b5c5;
        this._0x39bcc8e7 = _0xe0ed2bd5;
        this._0xc96b743b(_0xc47d916f, _0xe95d4eee);
    }

    public void _0xc096511f(_0xc001135e _0x0d1d1d71, int _0xbd6cd032, int _0xcc23eabf, int _0x16f453b6, float _0xd0d7f18c)
    {
        this._0x9ec3a3b7 = _0x0d1d1d71;
        this._0x6707c8dd = _0xbd6cd032;
        this._0x1251fa69 = _0xcc23eabf;
        this._0x0c33239f = PlaceTray;
        this._0x62722c74 = _0x16f453b6;
        this._0x39bcc8e7 = _0x16f453b6;
        this._0x1f667d8a = _0xd0d7f18c;
        if (this._rect != null)
        {
            this._rect.sizeDelta = new Vector2(_0xd0d7f18c, _0xd0d7f18c);
        }

        _0x6c3fbecd.SetText(this._numeral, _0xbd6cd032.ToString(), _0x5789e692.Parchment);
        this._0x0a8fa1b0(false);
    }

    private float _0xd09cd929;
    [SerializeField]
    private RectTransform _body;
    private float _0x75a941a1;
    private float _0xf9af2507 = 0.28f;
    public const int PlaceLeft = -1;
    public void _0x0a8fa1b0(bool _0x80045e54)
    {
        this._0x9bc96339 = _0x80045e54;
        if (this._ring != null)
        {
            this._ring.gameObject.SetActive(_0x80045e54);
            this._ring.color = _0x5789e692.WithAlpha(_0x5789e692.EmberGoldSoft, 0.9f);
        }
    }

    public int _0x39bcc8e7 { get; private set; }
}