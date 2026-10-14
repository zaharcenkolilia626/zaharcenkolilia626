using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Dresses and routes the three result cards - cleared, cold and held - in this game's
/// own words. Pause is written to the same standard as the other two: it is the card the
/// player opens most often and the one the review capture lands on.
/// </summary>
public sealed class _0x1f9a8ba2 : MonoBehaviour
{
    private void _0xbe1b28c2(int _0xb423d030)
    {
        Time.timeScale = 1f;
        if (this._progress != null && _0xb423d030 == this._gameSceneIndex)
        {
            this._progress._0xa00752ac(this._progress._0xbd59124c);
        }

        SceneManager.LoadScene(_0xb423d030);
    }

    public void _0x37f07999(int _0x81b896db, int _0xb7f25350, int _0x748dc99a)
    {
        if (this._loseCard == null)
        {
            return;
        }

        this._loseCard._0xcca5f03d(_0xd57d745d._0x9d5f09b3(new byte[20] { 242, 238, 227, 134, 245, 229, 231, 234, 227, 245, 134, 241, 227, 232, 242, 134, 229, 233, 234, 226 }, 166), _0xd57d745d._0x9d5f09b3(new byte[18] { 22, 23, 120, 8, 20, 25, 27, 29, 21, 29, 22, 12, 11, 120, 20, 29, 30, 12 }, 88), _0xd57d745d._0x9d5f09b3(new byte[7] { 234, 255, 236, 249, 251, 234, 158 }, 190) + _0x81b896db + _0xd57d745d._0x9d5f09b3(new byte[10] { 246, 246, 251, 246, 246, 154, 147, 144, 130, 246 }, 214) + _0xb7f25350 + _0xd57d745d._0x9d5f09b3(new byte[11] { 232, 232, 229, 232, 232, 154, 129, 143, 128, 156, 232 }, 200) + _0x748dc99a, _0xd57d745d._0x9d5f09b3(new byte[9] { 175, 169, 162, 219, 186, 188, 186, 178, 181 }, 251), _0xd57d745d._0x9d5f09b3(new byte[10] { 87, 70, 78, 83, 79, 70, 35, 78, 66, 83 }, 3));
    }

    public const int PopLose = 8;
    private void _0x2a1bba88()
    {
        if (this._director != null)
        {
            this._director._0xc1889a37();
        }
    }

    public void _0x5b5506da(int _0xc1e684f5, int _0xc338e8cb)
    {
        if (_0xc1e684f5 == PopPause)
        {
            Time.timeScale = 1f;
            if (this._pauseCard != null)
            {
                this._pauseCard._0x3c5fe0fc();
            }

            if (_0xc338e8cb == _0xcbbacd03.SlotSecondary)
            {
                this._0xbe1b28c2(this._menuSceneIndex);
            }

            return;
        }

        if (_0xc1e684f5 == PopWin)
        {
            if (_0xc338e8cb == _0xcbbacd03.SlotClose)
            {
                this._winCard._0x3c5fe0fc();
                return;
            }

            if (_0xc338e8cb == _0xcbbacd03.SlotPrimary && !this._0x049ec6e7)
            {
                if (this._progress != null)
                {
                    this._progress._0xbd59124c = this._progress._0xbd59124c + 1;
                }

                this._0xbe1b28c2(this._gameSceneIndex);
                return;
            }

            if (_0xc338e8cb == _0xcbbacd03.SlotPrimary)
            {
                this._0xbe1b28c2(this._menuSceneIndex);
                return;
            }

            this._0xbe1b28c2(this._gameSceneIndex);
            return;
        }

        if (_0xc338e8cb == _0xcbbacd03.SlotClose)
        {
            this._loseCard._0x3c5fe0fc();
            return;
        }

        this._0xbe1b28c2(_0xc338e8cb == _0xcbbacd03.SlotPrimary ? this._gameSceneIndex : this._menuSceneIndex);
    }

    [SerializeField]
    private _0x9e4438d2 _progress;
    public const int PopPause = 6;
    public void _0xed73e46b(int _0x52eb33ba, int _0xaebb4cc9, int _0xe59c0d52, int _0xcf0bb917)
    {
        this._0x049ec6e7 = _0xcf0bb917 >= (_0x9e4438d2.ChamberCount - 1);
        if (this._winCard == null)
        {
            return;
        }

        this._winCard._0xcca5f03d(_0xd57d745d._0x9d5f09b3(new byte[14] { 59, 48, 57, 53, 58, 61, 42, 88, 43, 61, 57, 52, 61, 60 }, 120), _0xd57d745d._0x9d5f09b3(new byte[7] { 61, 40, 59, 46, 44, 61, 73 }, 105) + _0x52eb33ba + _0xd57d745d._0x9d5f09b3(new byte[18] { 188, 212, 217, 208, 216, 188, 211, 210, 188, 222, 211, 200, 212, 188, 204, 221, 210, 207 }, 156), _0xd57d745d._0x9d5f09b3(new byte[1] { 151 }, 188) + _0xaebb4cc9 + _0xd57d745d._0x9d5f09b3(new byte[12] { 113, 20, 28, 19, 20, 3, 2, 113, 113, 124, 113, 113 }, 81) + _0xe59c0d52 + _0xd57d745d._0x9d5f09b3(new byte[18] { 79, 60, 59, 32, 33, 42, 60, 79, 44, 46, 59, 46, 35, 32, 40, 58, 42, 43 }, 111), this._0x049ec6e7 ? _0xd57d745d._0x9d5f09b3(new byte[10] { 115, 98, 106, 119, 107, 98, 7, 106, 102, 119 }, 39) : _0xd57d745d._0x9d5f09b3(new byte[12] { 193, 202, 215, 219, 175, 204, 199, 206, 194, 205, 202, 221 }, 143), _0xd57d745d._0x9d5f09b3(new byte[10] { 59, 39, 42, 50, 75, 42, 44, 42, 34, 37 }, 107));
    }

    [SerializeField]
    private int _menuSceneIndex;
    [SerializeField]
    private _0xcbbacd03 _pauseCard;
    [SerializeField]
    private _0xc001135e _director;
    [SerializeField]
    private int _gameSceneIndex = 1;
    [SerializeField]
    private _0xcbbacd03 _loseCard;
    private void Start()
    {
        if (this._winCard != null)
        {
            this._winCard._0x16841ce9(this, PopWin);
        }

        if (this._loseCard != null)
        {
            this._loseCard._0x16841ce9(this, PopLose);
        }

        if (this._pauseCard != null)
        {
            this._pauseCard._0x16841ce9(this, PopPause);
        }

        if (this._pauseTrigger != null)
        {
            this._pauseTrigger.onClick.AddListener(() => this._0x2a1bba88());
        }
    }

    [SerializeField]
    private Button _pauseTrigger;
    [SerializeField]
    private _0xcbbacd03 _winCard;
    public void _0x5d2666bd(int _0x69e9c620, int _0xafad49b9, int _0xc83cb479, int _0x2be4e2df)
    {
        if (this._pauseCard == null)
        {
            return;
        }

        Time.timeScale = 0f;
        this._pauseCard._0xcca5f03d(_0xd57d745d._0x9d5f09b3(new byte[11] { 242, 226, 224, 237, 228, 242, 129, 233, 228, 237, 229 }, 161), _0xd57d745d._0x9d5f09b3(new byte[7] { 91, 78, 93, 72, 74, 91, 47 }, 15) + _0x69e9c620 + _0xd57d745d._0x9d5f09b3(new byte[5] { 235, 235, 230, 235, 235 }, 203) + _0xafad49b9 + _0xd57d745d._0x9d5f09b3(new byte[4] { 128, 239, 230, 128 }, 160) + _0xc83cb479 + _0xd57d745d._0x9d5f09b3(new byte[11] { 204, 191, 184, 163, 162, 169, 191, 204, 191, 169, 184 }, 236), _0xd57d745d._0x9d5f09b3(new byte[7] { 219, 216, 201, 218, 195, 219, 168 }, 136) + _0x2be4e2df, _0xd57d745d._0x9d5f09b3(new byte[6] { 226, 245, 227, 229, 253, 245 }, 176), _0xd57d745d._0x9d5f09b3(new byte[10] { 147, 130, 138, 151, 139, 130, 231, 138, 134, 151 }, 199));
    }

    private bool _0x049ec6e7;
    public const int PopWin = 7;
}

internal static class _0xd57d745d
{
    internal static string _0x9d5f09b3(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}