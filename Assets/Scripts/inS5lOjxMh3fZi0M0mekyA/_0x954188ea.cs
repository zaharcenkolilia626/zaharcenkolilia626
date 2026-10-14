using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The two in-game actions, SPARK and RESET. A spark with an empty pool is not a dead
/// control: the plate shakes and flashes and the hint line explains itself, which is a
/// visible answer to the tap rather than silence.
/// </summary>
public sealed class _0x954188ea : MonoBehaviour
{
    public const int ActionReset = 1;
    private float _0x0df844a8;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private _0xc001135e _director;
    [SerializeField]
    private TextMeshProUGUI _caption;
    [SerializeField]
    private int _action;
    public void _0x710a4349(string _0x27b661d6)
    {
        _0x6c3fbecd.SetText(this._caption, _0x27b661d6, this._action == ActionSpark ? _0x5789e692.Parchment : _0x5789e692.ParchmentMuted);
    }

    private void Start()
    {
        if (this._rect != null)
        {
            this._0xf3546dcf = this._rect.anchoredPosition;
        }

        if (this._plate != null)
        {
            this._plate.color = this._restColor;
        }

        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0xfdd81ae2());
        }
    }

    private float _0x0db37f61;
    private void Update()
    {
        if (this._0x0db37f61 > 0f && this._rect != null)
        {
            this._0x0db37f61 = Mathf.Max(0f, this._0x0db37f61 - (Time.deltaTime / 0.3f));
            this._rect.anchoredPosition = this._0xf3546dcf + new Vector2(_0x5b45110f.Shake(1f - this._0x0db37f61, 9f) * 12f, 0f);
        }

        if (this._0x0df844a8 > 0f && this._plate != null)
        {
            this._0x0df844a8 = Mathf.Max(0f, this._0x0df844a8 - (Time.deltaTime / 0.45f));
            this._plate.color = Color.Lerp(this._restColor, _0x5789e692.DragonCrimson, _0x5b45110f.Punch(1f - this._0x0df844a8) * 2.2f);
        }
    }

    private void _0xfdd81ae2()
    {
        if (this._director == null)
        {
            return;
        }

        if (this._action == ActionSpark)
        {
            this._director._0x12bc4fe1(this);
        }
        else
        {
            this._director._0x6e9f8dcb();
        }
    }

    /// <summary>The refusal animation: a short shake plus a crimson flash on the fill only.</summary>
    public void _0xbd572d5a()
    {
        this._0x0db37f61 = 1f;
        this._0x0df844a8 = 1f;
    }

    private Vector2 _0xf3546dcf;
    [SerializeField]
    private Color _restColor = new Color(0.165f, 0.145f, 0.192f, 1f);
    [SerializeField]
    private Image _plate;
    public const int ActionSpark = 0;
    [SerializeField]
    private RectTransform _rect;
}