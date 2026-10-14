using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens or closes an overlay sheet through a serialized reference to its container.
/// Used by the two menu buttons, by each sheet's close cross and by its scrim.
/// </summary>
public sealed class _0x3136b799 : MonoBehaviour
{
    private void _0xbfcc730d()
    {
        if (this._sheet == null)
        {
            return;
        }

        if (this._open && this._otherSheet != null)
        {
            this._otherSheet.SetActive(false);
        }

        this._sheet.SetActive(this._open);
    }

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0xbfcc730d());
        }
    }

    [SerializeField]
    private GameObject _sheet;
    [SerializeField]
    private GameObject _otherSheet;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private bool _open = true;
}