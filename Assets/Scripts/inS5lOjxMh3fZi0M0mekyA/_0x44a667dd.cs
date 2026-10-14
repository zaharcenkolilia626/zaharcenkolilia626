using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Press feedback for any button: the rect dips on press and springs back on release.
/// Driven by hand from Update, because this project ships no tween library.
/// </summary>
public sealed class _0x44a667dd : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private void Reset()
    {
        this._target = this.transform as RectTransform;
    }

    public void OnPointerUp(PointerEventData _0x086e1157)
    {
        this._0x7364fe4e = false;
    }

    private void Start()
    {
        if (this._target == null)
        {
            this._target = this.transform as RectTransform;
        }
    }

    private void OnDisable()
    {
        this._0x7364fe4e = false;
        this._0x4e60551d = 1f;
        if (this._target != null)
        {
            this._target.localScale = Vector3.one;
        }
    }

    public void OnPointerDown(PointerEventData _0xd5cd1abb)
    {
        this._0x7364fe4e = true;
    }

    [SerializeField]
    private float _pressedScale = 0.94f;
    [SerializeField]
    private RectTransform _target;
    private float _0x4e60551d = 1f;
    private void Update()
    {
        if (this._target == null)
        {
            return;
        }

        float _0xcbdf96a2 = this._0x7364fe4e ? 0f : 1f;
        float _0x114c82e2 = this._0x7364fe4e ? 14f : 9f;
        this._0x4e60551d = Mathf.MoveTowards(this._0x4e60551d, _0xcbdf96a2, Time.unscaledDeltaTime * _0x114c82e2);
        float _0x143235ca = this._0x7364fe4e ? this._0x4e60551d : _0x5b45110f.OutBack(this._0x4e60551d);
        float _0x6dcc9b8e = Mathf.LerpUnclamped(this._pressedScale, 1f, _0x143235ca);
        this._target.localScale = new Vector3(_0x6dcc9b8e, _0x6dcc9b8e, 1f);
    }

    private bool _0x7364fe4e;
}