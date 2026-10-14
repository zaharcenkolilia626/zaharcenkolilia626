using TMPro;
using UnityEngine;

/// <summary>
/// The game scene's own readouts - three plates, the chamber title, the target chip and
/// the always-on control hint. Every one of them is a scene object of ours; nothing is
/// hung on a template slot, so switching the template's leftovers off breaks nothing.
/// </summary>
public sealed class _0xbb555dc0 : MonoBehaviour
{
    [SerializeField]
    private _0x7bc16e8b _emberPlate;
    public void _0x3fee813e(int _0xc69bc62d, int _0xf704d4fb)
    {
        _0x6c3fbecd.SetText(this._chamberTitle, _0x3af6620d._0x4698009f(new byte[8] { 22, 29, 20, 24, 23, 16, 7, 117 }, 85) + _0xd30f434d.Numeral(_0xc69bc62d), _0x5789e692.Parchment);
        _0x6c3fbecd.SetText(this._targetChip, _0x3af6620d._0x4698009f(new byte[7] { 116, 97, 114, 103, 101, 116, 0 }, 32) + _0xf704d4fb, _0x5789e692.Parchment);
    }

    private static readonly string RestingHint = _0x3af6620d._0x4698009f(new byte[71] { 206, 219, 202, 186, 219, 186, 201, 206, 213, 212, 223, 186, 186, 183, 186, 186, 206, 219, 202, 186, 219, 186, 202, 219, 212, 186, 206, 213, 186, 201, 223, 206, 186, 211, 206, 186, 186, 183, 186, 186, 206, 219, 202, 186, 219, 186, 201, 223, 206, 186, 201, 206, 213, 212, 223, 186, 206, 213, 186, 206, 219, 209, 223, 186, 211, 206, 186, 216, 219, 217, 209 }, 154);
    public void _0x938213cb()
    {
        if (this._sparkPlate != null)
        {
            this._sparkPlate._0xe032b406();
        }
    }

    [SerializeField]
    private TextMeshProUGUI _hint;
    [SerializeField]
    private TextMeshProUGUI _chamberTitle;
    [SerializeField]
    private _0x7bc16e8b _placesPlate;
    [SerializeField]
    private TextMeshProUGUI _targetChip;
    private bool _0x22f022d5;
    [SerializeField]
    private _0x7bc16e8b _sparkPlate;
    private float _0xbf157921;
    public void _0x7cf6a560(int _0xe3d9cd6c, int _0x06115a3f, int _0x20f771c9)
    {
        if (this._placesPlate != null)
        {
            this._placesPlate._0x9a544b68(_0xe3d9cd6c.ToString(), _0xe3d9cd6c <= 0);
        }

        if (this._sparkPlate != null)
        {
            this._sparkPlate._0x9a544b68(_0x06115a3f.ToString(), _0x06115a3f <= 0);
        }

        if (this._emberPlate != null)
        {
            this._emberPlate._0x9a544b68(_0x20f771c9.ToString(), _0x20f771c9 <= 0);
        }
    }

    private void Start()
    {
        if (this._placesPlate != null)
        {
            this._placesPlate._0xb4955710(_0x3af6620d._0x4698009f(new byte[6] { 173, 177, 188, 190, 184, 174 }, 253), _0x5789e692.EmberGold);
        }

        if (this._sparkPlate != null)
        {
            this._sparkPlate._0xb4955710(_0x3af6620d._0x4698009f(new byte[6] { 221, 222, 207, 220, 197, 221 }, 142), _0x5789e692.SkyLapis);
        }

        if (this._emberPlate != null)
        {
            this._emberPlate._0xb4955710(_0x3af6620d._0x4698009f(new byte[6] { 204, 196, 203, 204, 219, 218 }, 137), _0x5789e692.Parchment);
        }

        _0x6c3fbecd.SetText(this._hint, RestingHint, _0x5789e692.ParchmentMuted);
    }

    public void _0x9bf5b7e1()
    {
        if (this._placesPlate != null)
        {
            this._placesPlate._0xe032b406();
        }
    }

    /// <summary>A temporary line in place of the hint, for an action that could not happen.</summary>
    public void _0xadd2c807(string _0x2ae40835)
    {
        this._0x22f022d5 = true;
        this._0xbf157921 = 0f;
        _0x6c3fbecd.SetText(this._hint, _0x2ae40835, _0x5789e692.SkyLapis);
    }

    private void Update()
    {
        if (!this._0x22f022d5)
        {
            return;
        }

        this._0xbf157921 += Time.unscaledDeltaTime;
        if (this._0xbf157921 < 2.5f)
        {
            return;
        }

        this._0x22f022d5 = false;
        _0x6c3fbecd.SetText(this._hint, RestingHint, _0x5789e692.ParchmentMuted);
    }
}

internal static class _0x3af6620d
{
    internal static string _0x4698009f(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}