using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The ember archive: fifteen slots over the catalogued weights, with a real empty
/// state when nothing has been recorded yet. A list that can be empty never shows a
/// blank card.
/// </summary>
public sealed class _0x518fbd0a : MonoBehaviour
{
    [SerializeField]
    private _0x7ea45645[] _cells = new _0x7ea45645[0];
    [SerializeField]
    private RectTransform _card;
    private float _0x16c8c0a0;
    private void _0x2e57c6a6()
    {
        if (this._progress == null)
        {
            return;
        }

        List<int> _0x6b9ef9a0 = this._progress._0xc99256b2();
        bool _0x5113f879 = _0x6b9ef9a0.Count == 0;
        if (this._grid != null)
        {
            this._grid.SetActive(!_0x5113f879);
        }

        if (this._emptyLine != null)
        {
            this._emptyLine.gameObject.SetActive(_0x5113f879);
            if (_0x5113f879)
            {
                _0x6c3fbecd.SetText(this._emptyLine, _0xb60c8ba3._0x4b1de6b2(new byte[60] { 32, 33, 78, 61, 58, 33, 32, 43, 61, 78, 45, 47, 58, 47, 34, 33, 41, 59, 43, 42, 78, 55, 43, 58, 100, 45, 34, 43, 47, 60, 78, 47, 78, 45, 38, 47, 35, 44, 43, 60, 78, 58, 33, 78, 61, 58, 47, 60, 58, 78, 58, 38, 43, 78, 60, 43, 45, 33, 60, 42 }, 110), _0x5789e692.ParchmentMuted);
            }
        }

        if (_0x5113f879)
        {
            return;
        }

        bool _0xe27955cf = this._progress._0x3a71d345;
        bool _0x498f47ab = true;
        for (int _0x44724ca5 = 0; _0x44724ca5 < _0x9e4438d2.ChamberCount; _0x44724ca5++)
        {
            if (!this._progress._0x5e678cd0(_0x44724ca5))
            {
                _0x498f47ab = false;
                break;
            }
        }

        int _0x12447654 = this._cells.Length - 1;
        for (int _0xf6349bbb = 0; _0xf6349bbb < this._cells.Length; _0xf6349bbb++)
        {
            if (this._cells[_0xf6349bbb] == null)
            {
                continue;
            }

            if (_0xf6349bbb == _0x12447654)
            {
                this._cells[_0xf6349bbb]._0x4aa1204e(_0x498f47ab);
            }
            else if (_0xf6349bbb < _0x6b9ef9a0.Count)
            {
                this._cells[_0xf6349bbb]._0x0703e013(_0x6b9ef9a0[_0xf6349bbb], _0xe27955cf);
            }
            else
            {
                this._cells[_0xf6349bbb]._0x6b5f0c02();
            }
        }
    }

    [SerializeField]
    private GameObject _grid;
    private void OnEnable()
    {
        this._0x16c8c0a0 = 0f;
        this._0x2e57c6a6();
    }

    [SerializeField]
    private _0x9e4438d2 _progress;
    [SerializeField]
    private TextMeshProUGUI _emptyLine;
    private void Update()
    {
        if (this._card == null)
        {
            return;
        }

        this._0x16c8c0a0 += Time.unscaledDeltaTime;
        float _0x9320b557 = Mathf.LerpUnclamped(0.94f, 1f, _0x5b45110f.OutBack(_0x5b45110f.Clamp01(this._0x16c8c0a0 / 0.22f)));
        this._card.localScale = new Vector3(_0x9320b557, _0x9320b557, 1f);
    }
}

internal static class _0xb60c8ba3
{
    internal static string _0x4b1de6b2(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}