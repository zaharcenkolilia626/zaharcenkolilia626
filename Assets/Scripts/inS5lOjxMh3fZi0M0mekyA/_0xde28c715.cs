using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One carved seal on the temple path. Three visible states - locked, available and
/// cleared - plus a rim that brightens on selection and a punch on tap, so a single
/// still frame shows which chamber the player just chose.
/// </summary>
public sealed class _0xde28c715 : MonoBehaviour
{
    private void _0xffb0a653()
    {
        if (!this._0x1727580f || this._0x915df74f == null)
        {
            return;
        }

        this._0x915df74f._0x3903a517(this._0xaab13032);
    }

    [SerializeField]
    private TextMeshProUGUI _numeral;
    [SerializeField]
    private Image _rim;
    private void Update()
    {
        if (this._body == null)
        {
            return;
        }

        float _0x437c80f4 = 1f;
        if (this._0x63957222 && this._0x1727580f)
        {
            this._0x7253c9e8 += Time.unscaledDeltaTime;
            _0x437c80f4 *= 1f + (0.045f * _0x5b45110f.InOutSine(Mathf.PingPong(this._0x7253c9e8 / 1.6f, 1f)));
        }

        if (this._0xbc8ad618 > 0f)
        {
            this._0xbc8ad618 = Mathf.Max(0f, this._0xbc8ad618 - (Time.unscaledDeltaTime * 5.5f));
            _0x437c80f4 *= 1f + (_0x5b45110f.Punch(1f - this._0xbc8ad618) * 0.18f);
        }

        this._body.localScale = new Vector3(_0x437c80f4, _0x437c80f4, 1f);
    }

    public void _0xf09272d7(_0xbe1ea1f5 _0x3abe694a, int _0x99ff40a7)
    {
        this._0x915df74f = _0x3abe694a;
        this._0xaab13032 = _0x99ff40a7;
        _0x6c3fbecd.SetText(this._numeral, _0xd30f434d.Numeral(_0x99ff40a7), _0x5789e692.Parchment);
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0xffb0a653());
        }
    }

    [SerializeField]
    private Image _face;
    [SerializeField]
    private RectTransform _body;
    [SerializeField]
    private Button _button;
    public void _0x1d1893e6()
    {
        this._0xbc8ad618 = 1f;
    }

    private bool _0x1727580f;
    private bool _0x63957222;
    private float _0x7253c9e8;
    [SerializeField]
    private Image _lock;
    [SerializeField]
    private Image _clearedBadge;
    public void _0x758f4627(bool _0x679b7e49, bool _0xee0ebebd, bool _0x228c1cb8)
    {
        this._0x1727580f = _0x679b7e49;
        this._0x63957222 = _0x228c1cb8;
        if (this._face != null)
        {
            this._face.color = _0x679b7e49 ? Color.white : new Color(0.46f, 0.42f, 0.53f, 1f);
        }

        if (this._lock != null)
        {
            this._lock.gameObject.SetActive(!_0x679b7e49);
            this._lock.color = _0x5789e692.DragonCrimson;
        }

        if (this._clearedBadge != null)
        {
            this._clearedBadge.gameObject.SetActive(_0xee0ebebd);
        }

        if (this._numeral != null)
        {
            this._numeral.color = _0x679b7e49 ? _0x5789e692.Parchment : _0x5789e692.ParchmentMuted;
        }

        if (this._rim != null)
        {
            Color _0x0b5c38c6 = _0xee0ebebd ? _0x5789e692.EmberGold : (_0x679b7e49 ? _0x5789e692.EmberGoldSoft : _0x5789e692.StoneRidge);
            this._rim.color = _0x5789e692.WithAlpha(_0x0b5c38c6, _0x228c1cb8 ? 1f : (_0x679b7e49 ? 0.5f : 0.3f));
        }

        if (this._button != null)
        {
            this._button.interactable = _0x679b7e49;
        }

        if (!_0x228c1cb8)
        {
            this._0x7253c9e8 = 0f;
        }
    }

    private float _0xbc8ad618;
    private void OnDisable()
    {
        this._0xbc8ad618 = 0f;
        this._0x7253c9e8 = 0f;
        if (this._body != null)
        {
            this._body.localScale = Vector3.one;
        }
    }

    private int _0xaab13032;
    private _0xbe1ea1f5 _0x915df74f;
}