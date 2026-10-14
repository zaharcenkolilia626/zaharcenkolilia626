using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Three rows, and not one of them is audio: these games ship with no sound at all, so
/// a volume control would be a dead button. The slot it would have taken carries a real
/// accessibility setting whose effect is visible in the sheet itself.
/// </summary>
public sealed class _0xeb480f05 : MonoBehaviour
{
    [SerializeField]
    private _0xbe1ea1f5 _map;
    [SerializeField]
    private Image _contrastPreviewPlate;
    public const int SlotContrast = 1;
    public const int SlotHaptics = 0;
    [SerializeField]
    private _0x9e4438d2 _progress;
    [SerializeField]
    private _0xab5a85d0 _hapticsRow;
    [SerializeField]
    private RectTransform _card;
    private void Update()
    {
        if (this._card == null)
        {
            return;
        }

        this._0x7a164942 += Time.unscaledDeltaTime;
        float _0x0ab1a6e4 = Mathf.LerpUnclamped(0.94f, 1f, _0x5b45110f.OutBack(_0x5b45110f.Clamp01(this._0x7a164942 / 0.22f)));
        this._card.localScale = new Vector3(_0x0ab1a6e4, _0x0ab1a6e4, 1f);
    }

    private float _0x7a164942;
    [SerializeField]
    private _0xab5a85d0 _contrastRow;
    public void _0xd3e25b1d()
    {
        if (this._progress == null)
        {
            return;
        }

        this._progress._0xf1b19d86();
        this._0xdf60b033();
        if (this._map != null)
        {
            this._map._0x45243191();
        }

        if (this._archive != null && this._archive.gameObject.activeSelf)
        {
            this._archive.gameObject.SetActive(false);
            this._archive.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        this._0x7a164942 = 0f;
        this._0xdf60b033();
    }

    [SerializeField]
    private _0x518fbd0a _archive;
    private void Awake()
    {
        if (this._hapticsRow != null)
        {
            this._hapticsRow._0x1ea0ca30(this, SlotHaptics);
        }

        if (this._contrastRow != null)
        {
            this._contrastRow._0x1ea0ca30(this, SlotContrast);
        }
    }

    public void _0xb34023a1(int _0x574b032c)
    {
        if (this._progress == null)
        {
            return;
        }

        if (_0x574b032c == SlotHaptics)
        {
            this._progress._0xdac3f8a9 = !this._progress._0xdac3f8a9;
            if (this._progress._0xdac3f8a9)
            {
                Handheld.Vibrate();
            }
        }
        else if (_0x574b032c == SlotContrast)
        {
            this._progress._0x3a71d345 = !this._progress._0x3a71d345;
        }

        this._0xdf60b033();
    }

    [SerializeField]
    private Image _contrastPreviewStone;
    private void _0xdf60b033()
    {
        if (this._progress == null)
        {
            return;
        }

        if (this._hapticsRow != null)
        {
            this._hapticsRow._0x0409eb07(this._progress._0xdac3f8a9);
        }

        if (this._contrastRow != null)
        {
            this._contrastRow._0x0409eb07(this._progress._0x3a71d345);
        }

        bool _0x0ca62693 = this._progress._0x3a71d345;
        if (this._contrastPreviewPlate != null)
        {
            this._contrastPreviewPlate.color = _0x0ca62693 ? _0x5789e692.ObsidianDeep : _0x5789e692.StoneRidge;
        }

        if (this._contrastPreviewStone != null)
        {
            this._contrastPreviewStone.color = _0x0ca62693 ? _0x5789e692.EmberGoldSoft : Color.white;
        }
    }
}