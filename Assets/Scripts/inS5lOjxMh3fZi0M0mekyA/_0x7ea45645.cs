using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One archive slot. The stone glyph lives alone in the indicator gutter and the two
/// lines sit in their own column to the right of it, so nothing overlaps (rule C.25).
/// </summary>
public sealed class _0x7ea45645 : MonoBehaviour
{
    public void _0x0703e013(int _0x604fd622, bool _0x329313bd)
    {
        if (this._glyph != null)
        {
            this._glyph.sprite = this._emberSprite;
            this._glyph.color = Color.white;
        }

        if (this._plate != null)
        {
            this._plate.color = _0x329313bd ? _0x5789e692.ObsidianDeep : _0x5789e692.StoneSlab;
        }

        _0x6c3fbecd.SetText(this._weight, _0x604fd622.ToString(), _0x5789e692.EmberGold);
        _0x6c3fbecd.SetText(this._caption, _0xb95d9796._0x3227b287(new byte[7] { 203, 195, 199, 213, 211, 212, 195 }, 134), _0x5789e692.ParchmentMuted);
    }

    [SerializeField]
    private TextMeshProUGUI _caption;
    [SerializeField]
    private Image _glyph;
    [SerializeField]
    private TextMeshProUGUI _weight;
    public void _0x6b5f0c02()
    {
        if (this._glyph != null)
        {
            this._glyph.sprite = this._coldSprite;
            this._glyph.color = _0x5789e692.WithAlpha(Color.white, 0.5f);
        }

        if (this._plate != null)
        {
            this._plate.color = _0x5789e692.WithAlpha(_0x5789e692.StoneSlab, 0.75f);
        }

        _0x6c3fbecd.SetText(this._weight, _0xb95d9796._0x3227b287(new byte[1] { 73 }, 100), _0x5789e692.ParchmentMuted);
        _0x6c3fbecd.SetText(this._caption, _0xb95d9796._0x3227b287(new byte[6] { 246, 224, 228, 233, 224, 225 }, 165), _0x5789e692.WithAlpha(_0x5789e692.ParchmentMuted, 0.65f));
    }

    public void _0x4aa1204e(bool _0x851343f0)
    {
        if (this._glyph != null)
        {
            this._glyph.sprite = _0x851343f0 ? this._emberSprite : this._coldSprite;
            this._glyph.color = _0x5789e692.WithAlpha(Color.white, _0x851343f0 ? 1f : 0.5f);
        }

        if (this._plate != null)
        {
            this._plate.color = _0x851343f0 ? _0x5789e692.StoneRidge : _0x5789e692.WithAlpha(_0x5789e692.StoneSlab, 0.75f);
        }

        _0x6c3fbecd.SetText(this._weight, _0xb95d9796._0x3227b287(new byte[3] { 59, 53, 41 }, 112), _0x851343f0 ? _0x5789e692.EmberGoldSoft : _0x5789e692.ParchmentMuted);
        _0x6c3fbecd.SetText(this._caption, _0x851343f0 ? _0xb95d9796._0x3227b287(new byte[6] { 233, 237, 254, 226, 233, 232 }, 172) : _0xb95d9796._0x3227b287(new byte[8] { 213, 216, 216, 180, 210, 221, 194, 209 }, 148), _0x5789e692.ParchmentMuted);
    }

    [SerializeField]
    private Sprite _emberSprite;
    [SerializeField]
    private Image _plate;
    [SerializeField]
    private Sprite _coldSprite;
}

internal static class _0xb95d9796
{
    internal static string _0x3227b287(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}