using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A settings pill. Every tap moves the knob, repaints the pill and flips the status
/// word - three visible confirmations, none of which is a label change alone.
/// </summary>
public sealed class _0xab5a85d0 : MonoBehaviour
{
    public void _0x1ea0ca30(_0xeb480f05 _0x43f00824, int _0xd1ddb6d3)
    {
        this._0xe2224fff = _0x43f00824;
        this._0x172560ab = _0xd1ddb6d3;
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0x1d41b56c());
        }
    }

    [SerializeField]
    private TextMeshProUGUI _status;
    [SerializeField]
    private RectTransform _knob;
    private void _0x2a6009f5()
    {
        if (this._pill != null)
        {
            this._pill.color = Color.Lerp(_0x5789e692.StoneRidge, _0x5789e692.SkyLapis, this._0x3955c8c3);
        }

        if (this._knob != null)
        {
            this._knob.anchoredPosition = new Vector2(Mathf.Lerp(-this._knobTravel, this._knobTravel, this._0x3955c8c3), 0f);
        }

        _0x6c3fbecd.SetText(this._status, this._0x3e79539c ? _0x2f4352b6._0xbd0a5376(new byte[2] { 180, 181 }, 251) : _0x2f4352b6._0xbd0a5376(new byte[3] { 224, 233, 233 }, 175), this._0x3e79539c ? _0x5789e692.SkyLapis : _0x5789e692.ParchmentMuted);
    }

    private void Update()
    {
        float _0x809133e3 = this._0x3e79539c ? 1f : 0f;
        if (Mathf.Approximately(this._0x3955c8c3, _0x809133e3))
        {
            return;
        }

        this._0x3955c8c3 = Mathf.MoveTowards(this._0x3955c8c3, _0x809133e3, Time.unscaledDeltaTime / 0.18f);
        this._0x2a6009f5();
    }

    [SerializeField]
    private float _knobTravel = 44f;
    private bool _0x3e79539c;
    public void _0x0409eb07(bool _0xc34ff971)
    {
        this._0x3e79539c = _0xc34ff971;
        this._0x3955c8c3 = _0xc34ff971 ? 1f : 0f;
        this._0x2a6009f5();
    }

    private int _0x172560ab;
    private _0xeb480f05 _0xe2224fff;
    private void _0x1d41b56c()
    {
        if (this._0xe2224fff == null)
        {
            return;
        }

        this._0xe2224fff._0xb34023a1(this._0x172560ab);
    }

    [SerializeField]
    private Image _pill;
    private float _0x3955c8c3;
    [SerializeField]
    private Button _button;
}

internal static class _0x2f4352b6
{
    internal static string _0xbd0a5376(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}