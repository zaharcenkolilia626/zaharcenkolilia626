using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One HUD plate: an accent pip alone in its gutter, then a value and a caption in a
/// text column that never shares an x-band with the pip (rule C.25). The geometry is
/// authored in the scene; this component only fills and tints it.
/// </summary>
public sealed class _0x7bc16e8b : MonoBehaviour
{
    private void Start()
    {
        this._0x093464a8 = this._pip == null ? _0x5789e692.EmberGold : this._pip.color;
    }

    public void _0xe032b406()
    {
        this._0xfe25e448 = 1f;
    }

    [SerializeField]
    private TextMeshProUGUI _value;
    /// <summary>A zero still renders - the number is information here - but the pip dims to read as spent.</summary>
    public void _0x9a544b68(string _0xb2ba7ab5, bool _0xd8286340)
    {
        _0x6c3fbecd.SetText(this._value, _0xb2ba7ab5, this._0x093464a8);
        if (this._pip != null)
        {
            this._pip.color = _0x5789e692.WithAlpha(this._0x093464a8, _0xd8286340 ? 0.35f : 1f);
        }
    }

    public void _0xb4955710(string _0x1cc2c076, Color _0x7dcd5d0d)
    {
        this._0x093464a8 = _0x7dcd5d0d;
        if (this._pip != null)
        {
            this._pip.color = _0x7dcd5d0d;
        }

        if (this._plate != null)
        {
            this._plate.color = _0x5789e692.StoneSlab;
        }

        _0x6c3fbecd.SetText(this._label, _0x1cc2c076, _0x5789e692.ParchmentMuted);
        _0x6c3fbecd.SetText(this._value, _0xd5bf3186._0xd279970c(new byte[1] { 94 }, 110), _0x7dcd5d0d);
    }

    [SerializeField]
    private Image _plate;
    private Color _0x093464a8 = Color.white;
    [SerializeField]
    private TextMeshProUGUI _label;
    private void Update()
    {
        if (this._0xfe25e448 <= 0f)
        {
            return;
        }

        this._0xfe25e448 = Mathf.Max(0f, this._0xfe25e448 - (Time.unscaledDeltaTime * 4.5f));
        if (this._value == null)
        {
            return;
        }

        float _0xe565c650 = 1f + (_0x5b45110f.Punch(1f - this._0xfe25e448) * 0.22f);
        this._value.rectTransform.localScale = new Vector3(_0xe565c650, _0xe565c650, 1f);
    }

    private float _0xfe25e448;
    [SerializeField]
    private Image _pip;
}

internal static class _0xd5bf3186
{
    internal static string _0xd279970c(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}