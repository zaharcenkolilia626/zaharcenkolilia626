using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A destructive action that has to be confirmed. The first tap arms it - the caption
/// changes AND the plate turns crimson - and a second tap within the timeout commits.
/// The crimson is a fill, never a text colour.
/// </summary>
public sealed class _0xe1d6eedd : MonoBehaviour
{
    [SerializeField]
    private float _armedSeconds = 4f;
    private bool _0xcb90b081;
    private void OnDisable()
    {
        this._0x7d1cd91c();
    }

    private void Update()
    {
        if (!this._0xcb90b081)
        {
            return;
        }

        this._0x54b22981 += Time.unscaledDeltaTime;
        if (this._0x54b22981 >= this._armedSeconds)
        {
            this._0x7d1cd91c();
        }
    }

    private void _0x7d1cd91c()
    {
        this._0xcb90b081 = false;
        this._0x54b22981 = 0f;
        if (this._plate != null)
        {
            this._plate.color = _0x5789e692.StoneSlab;
        }

        _0x6c3fbecd.SetText(this._caption, _0x6bb1a89b._0x6b1d3738(new byte[12] { 153, 142, 157, 143, 153, 252, 142, 153, 159, 147, 142, 152 }, 220), _0x5789e692.ParchmentMuted);
    }

    private void _0x4776b725()
    {
        if (!this._0xcb90b081)
        {
            this._0xcb90b081 = true;
            this._0x54b22981 = 0f;
            if (this._plate != null)
            {
                this._plate.color = _0x5789e692.DragonCrimson;
            }

            _0x6c3fbecd.SetText(this._caption, _0x6bb1a89b._0x6b1d3738(new byte[18] { 180, 161, 176, 192, 161, 167, 161, 169, 174, 192, 180, 175, 192, 165, 178, 161, 179, 165 }, 224), _0x5789e692.Parchment);
            return;
        }

        this._0x7d1cd91c();
        if (this._owner != null)
        {
            this._owner._0xd3e25b1d();
        }
    }

    [SerializeField]
    private TextMeshProUGUI _caption;
    [SerializeField]
    private _0xeb480f05 _owner;
    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0x4776b725());
        }

        this._0x7d1cd91c();
    }

    [SerializeField]
    private Image _plate;
    private float _0x54b22981;
    [SerializeField]
    private Button _button;
}

internal static class _0x6bb1a89b
{
    internal static string _0x6b1d3738(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}