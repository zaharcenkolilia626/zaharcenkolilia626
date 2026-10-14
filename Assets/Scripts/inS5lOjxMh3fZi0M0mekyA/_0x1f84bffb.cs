using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The moving half of the scale. The beam's turn is the single source of truth: the
/// pans and the chain links are derived from it in the same frame, so nothing fights
/// anything else for the same rect.
/// </summary>
public sealed class _0x1f84bffb : MonoBehaviour
{
    private float _0x26eacee9 = 1f;
    public _0x42a31ba7 _0xc0741c6b
    {
        get
        {
            return this._leftPan;
        }
    }

    private float _0xd4b2447a;
    private float _0x767c9e44;
    private void _0xedbf6d27()
    {
        float _0x06fdfa8e = this._0xd4b2447a > 0f ? _0x5b45110f.Shake(1f - this._0xd4b2447a, 7f) * 10f : 0f;
        if (this._beam != null)
        {
            this._beam.localEulerAngles = new Vector3(0f, 0f, -this._0x2f4c30c6);
            this._beam.anchoredPosition = new Vector2(_0x4c79fffb.BoardCentreX + _0x06fdfa8e, _0x4c79fffb.BeamPivotY);
        }

        float _0x5916eda3 = _0x4c79fffb.PanHeight(this._0x2f4c30c6, true);
        float _0x5b8728a9 = _0x4c79fffb.PanHeight(this._0x2f4c30c6, false);
        if (this._leftPan != null)
        {
            this._leftPan._0xf44dc616(_0x5916eda3);
        }

        if (this._rightPan != null)
        {
            this._rightPan._0xf44dc616(_0x5b8728a9);
        }

        this._0xeaf49ceb(this._leftChain, _0x4c79fffb.BeamEnd(this._0x2f4c30c6, true), new Vector2(_0x4c79fffb.PanCentreX(true), _0x5916eda3));
        this._0xeaf49ceb(this._rightChain, _0x4c79fffb.BeamEnd(this._0x2f4c30c6, false), new Vector2(_0x4c79fffb.PanCentreX(false), _0x5b8728a9));
    }

    public void _0x253a3445(float _0x66c28a2a, bool _0x01c729a1)
    {
        if (_0x01c729a1)
        {
            this._0x2f4c30c6 = _0x66c28a2a;
            this._0x8a5fdb7c = _0x66c28a2a;
            this._0x767c9e44 = _0x66c28a2a;
            this._0x26eacee9 = 1f;
            this._0xedbf6d27();
            return;
        }

        this._0x8a5fdb7c = this._0x2f4c30c6;
        this._0x767c9e44 = _0x66c28a2a;
        this._0x26eacee9 = 0f;
    }

    private float _0x2f4c30c6;
    [SerializeField]
    private _0x42a31ba7 _rightPan;
    /// <summary>Discrete links spaced along the gap, so no sprite is ever stretched.</summary>
    private void _0xeaf49ceb(RectTransform[] _0x8d1636bb, Vector2 _0x78a60688, Vector2 _0xe14405d2)
    {
        if (_0x8d1636bb == null || _0x8d1636bb.Length == 0)
        {
            return;
        }

        Vector2 _0x60bd5dd3 = new Vector2(_0xe14405d2.x, _0xe14405d2.y + (_0x4c79fffb.PanSize * 0.42f));
        for (int _0x5f887da9 = 0; _0x5f887da9 < _0x8d1636bb.Length; _0x5f887da9++)
        {
            if (_0x8d1636bb[_0x5f887da9] == null)
            {
                continue;
            }

            float _0x97a47d20 = (_0x5f887da9 + 0.5f) / _0x8d1636bb.Length;
            _0x8d1636bb[_0x5f887da9].anchoredPosition = _0x5b45110f.Lerp(_0x78a60688, _0x60bd5dd3, _0x97a47d20);
        }
    }

    [SerializeField]
    private RectTransform _beam;
    [SerializeField]
    private RectTransform[] _leftChain = new RectTransform[0];
    public float _0x084358bb
    {
        get
        {
            return this._0x2f4c30c6;
        }
    }

    [SerializeField]
    private _0x42a31ba7 _leftPan;
    public void _0xb5b6573b()
    {
        this._0xd4b2447a = 1f;
    }

    [SerializeField]
    private RectTransform[] _rightChain = new RectTransform[0];
    private float _0x8a5fdb7c;
    private void Update()
    {
        if (this._0x26eacee9 < 1f)
        {
            this._0x26eacee9 = Mathf.Min(1f, this._0x26eacee9 + (Time.deltaTime / 0.35f));
            this._0x2f4c30c6 = Mathf.LerpUnclamped(this._0x8a5fdb7c, this._0x767c9e44, _0x5b45110f.OutBack(this._0x26eacee9));
        }

        if (this._0xd4b2447a > 0f)
        {
            this._0xd4b2447a = Mathf.Max(0f, this._0xd4b2447a - (Time.deltaTime / 0.25f));
        }

        this._0xedbf6d27();
    }

    public _0x42a31ba7 _0x22740ead
    {
        get
        {
            return this._rightPan;
        }
    }

    public void _0x1b2ca227(_0xc001135e _0xdd98dfc3)
    {
        if (this._leftPan != null)
        {
            this._leftPan._0x522480c6(_0xdd98dfc3);
        }

        if (this._rightPan != null)
        {
            this._rightPan._0x522480c6(_0xdd98dfc3);
        }

        this._0x253a3445(0f, true);
    }
}