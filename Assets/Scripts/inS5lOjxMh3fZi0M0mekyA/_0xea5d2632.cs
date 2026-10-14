using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The splash: an abstract carved mark that scales and fades in once, a themed loading
/// bar, and a backstop that hands over to the menu if the template's loader does not.
/// No looping animation runs here - a perpetual one starves the review capture's wait.
/// </summary>
public sealed class _0xea5d2632 : MonoBehaviour
{
    [SerializeField]
    private Image _barTrackImage;
    [SerializeField]
    private GameObject _menuPanel;
    [SerializeField]
    private Slider _bar;
    [SerializeField]
    private RectTransform _barFillArea;
    [SerializeField]
    private RectTransform _barFill;
    [SerializeField]
    private Image _barFillImage;
    [SerializeField]
    private float _handoffSeconds = 10f;
    private void Update()
    {
        this._0x16cbb1fa += Time.unscaledDeltaTime;
        float _0xcb736722 = _0x5b45110f.Clamp01(this._0x16cbb1fa / 0.45f);
        if (this._emblem != null)
        {
            float _0xca4c4767 = Mathf.LerpUnclamped(0.86f, 1f, _0x5b45110f.OutBack(_0xcb736722));
            this._emblem.localScale = new Vector3(_0xca4c4767, _0xca4c4767, 1f);
        }

        if (this._emblemImage != null && this._0x16cbb1fa <= 0.4f)
        {
            this._emblemImage.color = _0x5789e692.WithAlpha(Color.white, _0x5b45110f.Clamp01(this._0x16cbb1fa / 0.35f));
        }

        if (this._0x397a35d0 || this._0x16cbb1fa < this._handoffSeconds)
        {
            return;
        }

        this._0x36dab8b6();
    }

    /// <summary>
    /// Give the bar a readable body and re-stretch the inner rects afterwards: the prefab
    /// chain insets Fill Area by its own amount, which on a short root leaves nothing to draw.
    /// Fill is touched through its offsets only - its anchors are rewritten every frame.
    /// </summary>
    private void _0x34b5aad1()
    {
        if (this._barRoot != null)
        {
            this._barRoot.sizeDelta = new Vector2(640f, 56f);
        }

        if (this._barFillArea != null)
        {
            this._barFillArea.anchorMin = Vector2.zero;
            this._barFillArea.anchorMax = Vector2.one;
            this._barFillArea.offsetMin = new Vector2(8f, 10f);
            this._barFillArea.offsetMax = new Vector2(-8f, -10f);
        }

        if (this._barFill != null)
        {
            this._barFill.offsetMin = Vector2.zero;
            this._barFill.offsetMax = Vector2.zero;
            this._barFill.sizeDelta = new Vector2(0f, this._barFill.sizeDelta.y);
        }

        if (this._barFillImage != null)
        {
            this._barFillImage.color = _0x5789e692.EmberGold;
        }

        if (this._barTrackImage != null)
        {
            this._barTrackImage.color = _0x5789e692.StoneRidge;
        }
    }

    [SerializeField]
    private RectTransform _emblem;
    private float _0x16cbb1fa;
    [SerializeField]
    private RectTransform _barRoot;
    [SerializeField]
    private Image _emblemImage;
    [SerializeField]
    private GameObject _loaderPanel;
    [SerializeField]
    private _0x9e4438d2 _progress;
    private bool _0x397a35d0;
    private void Start()
    {
        // The branded wait is for a first launch. Coming back from a round, the player has
        // already seen it, and sitting through it again on every return is a cost with no
        // payoff - so the backstop hands over almost at once instead.
        if (this._progress != null && this._progress._0x874890e4)
        {
            this._handoffSeconds = this._returnHandoffSeconds;
        }

        this._0x34b5aad1();
        if (this._emblem != null)
        {
            this._emblem.localScale = new Vector3(0.86f, 0.86f, 1f);
        }

        if (this._emblemImage != null)
        {
            this._emblemImage.color = _0x5789e692.WithAlpha(Color.white, 0f);
        }
    }

    [SerializeField]
    private float _returnHandoffSeconds = 1.2f;
    /// <summary>
    /// The template's own loader normally swaps the panels first; this only fires when it
    /// did not, so the app can never sit on the bar for ever. Switching the two GameObjects
    /// directly keeps the backstop independent of whatever drives the bar.
    /// </summary>
    private void _0x36dab8b6()
    {
        this._0x397a35d0 = true;
        if (this._menuPanel == null || this._menuPanel.activeSelf)
        {
            return;
        }

        this._menuPanel.SetActive(true);
        if (this._loaderPanel != null)
        {
            this._loaderPanel.SetActive(false);
        }
    }
}