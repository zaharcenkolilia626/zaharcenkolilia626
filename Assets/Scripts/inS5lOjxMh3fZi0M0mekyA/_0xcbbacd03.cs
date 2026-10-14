using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One result card. Its buttons are told apart by the SLOT they were serialized into -
/// close, primary, secondary - never by walking the children, which is how a close cross
/// ends up wearing an action's caption. The cross carries an icon and no text at all.
/// </summary>
public sealed class _0xcbbacd03 : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _header;
    private float _0x3ac943e2 = -1f;
    [SerializeField]
    private Image _primaryPlate;
    [SerializeField]
    private Image _cardFrame;
    private void _0xa6e56984()
    {
        if (this._cardPlate != null)
        {
            this._cardPlate.color = _0x5789e692.StoneSlab;
        }

        if (this._cardFrame != null)
        {
            this._cardFrame.color = _0x5789e692.WithAlpha(_0x5789e692.EmberGold, 0.85f);
        }

        if (this._primaryPlate != null)
        {
            this._primaryPlate.color = _0x5789e692.EmberGold;
        }

        if (this._secondaryPlate != null)
        {
            this._secondaryPlate.color = _0x5789e692.StoneRidge;
        }

        // The template leaves a close Image with no sprite, which Unity draws as a white
        // slab. The cross comes from our own serialized sprite on the Image in the scene.
        if (this._closeIcon != null)
        {
            this._closeIcon.color = _0x5789e692.ParchmentMuted;
        }
    }

    private void _0xd68f69d9(int _0x0588d5a4)
    {
        if (this._0xc4f7d77b != null)
        {
            this._0xc4f7d77b._0x5b5506da(this._0x6ad53f04, _0x0588d5a4);
        }
    }

    public void _0x16841ce9(_0x1f9a8ba2 _0x13d93bb5, int _0xd49f5ea8)
    {
        this._0xc4f7d77b = _0x13d93bb5;
        this._0x6ad53f04 = _0xd49f5ea8;
        if (this._closeButton != null)
        {
            this._closeButton.onClick.AddListener(() => this._0xd68f69d9(SlotClose));
        }

        if (this._primaryButton != null)
        {
            this._primaryButton.onClick.AddListener(() => this._0xd68f69d9(SlotPrimary));
        }

        if (this._secondaryButton != null)
        {
            this._secondaryButton.onClick.AddListener(() => this._0xd68f69d9(SlotSecondary));
        }

        this._0xa6e56984();
        this._0x3c5fe0fc();
    }

    [SerializeField]
    private Image _secondaryPlate;
    public const int SlotPrimary = 1;
    public const int SlotClose = 0;
    public void _0xcca5f03d(string _0x13e32bcb, string _0xa051da8d, string _0x6a3491e2, string _0x5470b0f7, string _0xb58c90f9)
    {
        _0x6c3fbecd.SetText(this._header, _0x13e32bcb, _0x5789e692.EmberGold);
        _0x6c3fbecd.SetText(this._main, _0xa051da8d, _0x5789e692.Parchment);
        _0x6c3fbecd.SetText(this._additional, _0x6a3491e2, _0x5789e692.ParchmentMuted);
        _0x6c3fbecd.SetText(this._primaryCaption, _0x5470b0f7, _0x5789e692.Ink);
        _0x6c3fbecd.SetText(this._secondaryCaption, _0xb58c90f9, _0x5789e692.ParchmentMuted);
        // The primary plate is gold, so its caption is the one face that must be read
        // against a light fill. Give that label its own light outline rather than a dark one.
        if (this._primaryCaption != null)
        {
            this._primaryCaption.color = _0x5789e692.Ink;
            _0x6c3fbecd.ApplyOutline(this._primaryCaption, 0.16f);
        }

        if (this._root != null)
        {
            this._root.SetActive(true);
        }

        this._0x3ac943e2 = 0f;
    }

    [SerializeField]
    private TextMeshProUGUI _main;
    [SerializeField]
    private Image _closeIcon;
    [SerializeField]
    private TextMeshProUGUI _additional;
    [SerializeField]
    private TextMeshProUGUI _secondaryCaption;
    [SerializeField]
    private GameObject _root;
    public const int SlotSecondary = 2;
    [SerializeField]
    private Button _secondaryButton;
    [SerializeField]
    private Image _cardPlate;
    [SerializeField]
    private RectTransform _card;
    [SerializeField]
    private Button _primaryButton;
    [SerializeField]
    private TextMeshProUGUI _primaryCaption;
    public bool _0xe2a20b27
    {
        get
        {
            return this._root != null && this._root.activeSelf;
        }
    }

    private _0x1f9a8ba2 _0xc4f7d77b;
    [SerializeField]
    private Button _closeButton;
    public void _0x3c5fe0fc()
    {
        this._0x3ac943e2 = -1f;
        if (this._root != null)
        {
            this._root.SetActive(false);
        }
    }

    private int _0x6ad53f04;
    private void Update()
    {
        if (this._0x3ac943e2 < 0f || this._card == null)
        {
            return;
        }

        this._0x3ac943e2 += Time.unscaledDeltaTime;
        float _0xa703b5d0 = Mathf.LerpUnclamped(0.94f, 1f, _0x5b45110f.OutBack(_0x5b45110f.Clamp01(this._0x3ac943e2 / 0.22f)));
        this._card.localScale = new Vector3(_0xa703b5d0, _0xa703b5d0, 1f);
    }
}