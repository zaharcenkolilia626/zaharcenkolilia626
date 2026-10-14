using System;
using UnityEngine;

public class _0x6a3f051f
{
    private void _0xf8bb4844<T>(Action<T> _0xa83fa578, T _0x23fc6d5c)
    {
        if (_0x4834039c || _0xa83fa578 == null)
            return;
        try
        {
            _0xa83fa578(_0x23fc6d5c);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[18] { 239, 221, 218, 238, 209, 221, 207, 152, 219, 217, 212, 212, 218, 217, 219, 211, 130, 152 }, 184) + exception.Message);
        }
    }

    private void _0x7d026a51(string _0x6e8c1bad, string _0x732f4895)
    {
        _0xf8bb4844(PopupPageFinished, _0x6e8c1bad ?? string.Empty, _0x732f4895 ?? string.Empty);
    }

    public void _0xf1ae865b(string _0x477b5e22, string _0xb4e74560)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[17] { 132, 146, 131, 167, 152, 135, 130, 135, 162, 132, 146, 133, 182, 144, 146, 153, 131 }, 247), _0x477b5e22 ?? string.Empty, _0xb4e74560 ?? string.Empty);
    }

    public void _0x0e80d863(string _0x95f51b70)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[4] { 158, 141, 154, 151 }, 251), _0x95f51b70 ?? string.Empty);
    }

    public void _0xe0d5754f(string _0x964f6f0c)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[10] { 208, 223, 220, 192, 214, 227, 220, 195, 198, 195 }, 179), _0x964f6f0c ?? string.Empty);
    }

    public void _0x2f6580a7()
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[6] { 138, 130, 175, 140, 142, 134 }, 237));
    }

    public bool _0x7bde7cb2
    {
        get
        {
            return _0xa7a7537e(_0xe20ad544._0x59d74307(new byte[9] { 250, 248, 247, 222, 246, 219, 248, 250, 242 }, 153));
        }
    }

    public void _0xa44a7a95()
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[4] { 175, 168, 179, 172 }, 220));
    }

    private void _0x1553ba10(int _0x5e9c1f27)
    {
        _0xf8bb4844(ProgressChanged, Mathf.Clamp(_0x5e9c1f27, 0, 100) / 100f);
    }

    public bool _0xb52774be
    {
        get
        {
            return _0x80b99a93 != null && !_0x4834039c;
        }
    }

    private void _0x8a356fb7(int _0xad73be7e, string _0x7f4fded1)
    {
        _0xf8bb4844(PageFinished, _0xad73be7e, _0x7f4fded1 ?? string.Empty);
    }

    private void _0x573e2427(string _0xbeeba97f)
    {
        _0xf8bb4844(PageStarted, _0xbeeba97f ?? string.Empty);
    }

    private string _0x0f0014da(string _0xbeaa0402)
    {
        if (_0x80b99a93 == null || _0x4834039c)
            return string.Empty;
        try
        {
            string _0x760c5275 = _0x80b99a93.Call<string>(_0xbeaa0402);
            return _0x760c5275 ?? string.Empty;
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[8] { 113, 67, 68, 112, 79, 67, 81, 6 }, 38) + _0xbeaa0402 + _0xe20ad544._0x59d74307(new byte[2] { 13, 23 }, 55) + exception.Message);
            return string.Empty;
        }
    }

    public void _0x3a9e0e0c(bool _0x07e2fb28)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[20] { 193, 215, 198, 230, 218, 219, 192, 214, 226, 211, 192, 198, 203, 241, 221, 221, 217, 219, 215, 193 }, 178), _0x07e2fb28);
    }

    private void _0x2d60757b(string _0x1609e4bb)
    {
        _0xf8bb4844(PageError, _0x1609e4bb ?? string.Empty);
    }

    public void _0xe583ec72(bool _0xce486d7f)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[13] { 140, 131, 132, 131, 153, 130, 169, 139, 154, 158, 159, 152, 143 }, 234), _0xce486d7f ? 1 : 0);
    }

    public _0x6a3f051f()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            _0x9d524c7a();
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[23] { 217, 235, 236, 216, 231, 235, 249, 174, 239, 250, 250, 239, 237, 230, 174, 232, 239, 231, 226, 235, 234, 180, 174 }, 142) + exception.Message);
        }
#endif
    }

    private void _0x3737e8f2(string _0xf6fc5e24)
    {
        _0xf8bb4844(PopupOpened, _0xf6fc5e24 ?? string.Empty);
    }

    public event Action<string, string> PopupPageStarted;
    public event Action<int, string> PageFinished;
    public Func<string, int> AllowCapture;
    public string _0x93bea148()
    {
        return _0x0f0014da(_0xe20ad544._0x59d74307(new byte[7] { 90, 89, 91, 83, 109, 74, 84 }, 56));
    }

    public void _0xda2e39e2(int _0xbe18cd96, int _0xf8825d02, int _0xef2ab974, int _0x4398d115, int _0x4d915407, int _0x40166d59)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[10] { 159, 137, 152, 161, 141, 158, 139, 133, 130, 159 }, 236), _0xbe18cd96, _0xf8825d02, _0xef2ab974, _0x4398d115, _0x4d915407, _0x40166d59);
    }

    private void _0xe25141d3(string _0x9dbbf50e, string _0xe5eb1327)
    {
        _0xf8bb4844(PopupPageStarted, _0x9dbbf50e ?? string.Empty, _0xe5eb1327 ?? string.Empty);
    }

    public void _0x7feb9379()
    {
        if (_0x4834039c)
            return;
        _0x4834039c = true;
        if (_0x80b99a93 == null)
            return;
        try
        {
            _0x80b99a93.Call(_0xe20ad544._0x59d74307(new byte[7] { 149, 148, 130, 133, 131, 158, 136 }, 241));
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[17] { 249, 203, 204, 248, 199, 203, 217, 142, 202, 203, 221, 218, 220, 193, 215, 148, 142 }, 174) + exception.Message);
        }

        _0x80b99a93.Dispose();
        _0x80b99a93 = null;
    }

    public string _0x56844f61()
    {
        return _0x0f0014da(_0xe20ad544._0x59d74307(new byte[12] { 164, 166, 183, 150, 176, 166, 177, 130, 164, 166, 173, 183 }, 195));
    }

    public void _0x2543dec5()
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[4] { 125, 124, 113, 112 }, 21));
    }

    public event Action<string> PageStarted;
    public event Action<string, string> PopupPageFinished;
    public void _0x89de1ed0(string _0x87b80723, string _0x73f3fd54)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[9] { 255, 236, 251, 246, 202, 245, 234, 239, 234 }, 154), _0x87b80723 ?? string.Empty, _0x73f3fd54 ?? string.Empty);
    }

    private bool _0xa7a7537e(string _0xa720b09b)
    {
        if (_0x80b99a93 == null || _0x4834039c)
            return false;
        try
        {
            return _0x80b99a93.Call<bool>(_0xa720b09b);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[8] { 70, 116, 115, 71, 120, 116, 102, 49 }, 17) + _0xa720b09b + _0xe20ad544._0x59d74307(new byte[2] { 77, 87 }, 119) + exception.Message);
            return false;
        }
    }

    public event Action<string> PopupClosed;
    private bool _0x4834039c;
    private void _0x9d524c7a()
    {
        _0x209543c2 = new _0xe24c721a(this);
        using (var _0x658692d0 = new AndroidJavaClass(_0xe20ad544._0x59d74307(new byte[30] { 176, 188, 190, 253, 166, 189, 186, 167, 170, 224, 183, 253, 163, 191, 178, 170, 182, 161, 253, 134, 189, 186, 167, 170, 131, 191, 178, 170, 182, 161 }, 211)))
        using (var _0xbc7eb5d7 = _0x658692d0.GetStatic<AndroidJavaObject>(_0xe20ad544._0x59d74307(new byte[15] { 245, 227, 228, 228, 243, 248, 226, 215, 245, 226, 255, 224, 255, 226, 239 }, 150)))
        {
            if (_0xbc7eb5d7 == null)
                throw new InvalidOperationException(_0xe20ad544._0x59d74307(new byte[23] { 104, 126, 121, 121, 110, 101, 127, 74, 104, 127, 98, 125, 98, 127, 114, 43, 98, 120, 43, 101, 126, 103, 103 }, 11));
            _0x80b99a93 = new AndroidJavaObject(_0xe20ad544._0x59d74307(new byte[14] { 198, 203, 200, 208, 215, 138, 236, 193, 200, 212, 244, 197, 202, 193 }, 164), _0xbc7eb5d7, _0x209543c2);
        }
    }

    private sealed class _0xe24c721a : AndroidJavaProxy
    {
        private readonly _0x6a3f051f _0xd0db2134;
        public _0xe24c721a(_0x6a3f051f _0x859941ac) : base(_0xe20ad544._0x59d74307(new byte[34] { 124, 113, 114, 106, 109, 48, 95, 117, 38, 117, 115, 118, 39, 38, 125, 114, 127, 115, 119, 112, 110, 100, 113, 110, 110, 82, 119, 112, 117, 86, 113, 113, 117, 109 }, 30))
        {
            this._0xd0db2134 = _0x859941ac;
        }

        [UnityEngine.Scripting.Preserve]
        public int _0xa761f12f(string _0xe4d3f914, int _0x4a34e888, int _0x7a7a35ff)
        {
            _0x6a3f051f _0x90f38996 = _0xd0db2134;
            if (_0x90f38996 == null || _0x90f38996._0x4834039c || _0x90f38996.HandleRequest == null)
                return 1;
            try
            {
                bool _0x9d8cd849 = _0x90f38996.HandleRequest(_0xe4d3f914 ?? string.Empty, _0x4a34e888 != 0, _0x7a7a35ff != 0);
                return _0x9d8cd849 ? 1 : 0;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[16] { 43, 25, 30, 42, 21, 25, 11, 92, 20, 29, 18, 24, 16, 25, 70, 92 }, 124) + exception.Message);
                return 1;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void _0xdff48f8c(string _0x984f3262)
        {
            _0xd0db2134._0x573e2427(_0x984f3262);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0xf8384755(string _0xa6599538)
        {
            _0xd0db2134._0x2951377f(_0xa6599538);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0xf75d0503(int _0x1e39f4c1)
        {
            _0xd0db2134._0x1553ba10(_0x1e39f4c1);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0x107ff6f0(int _0x333b49ce, string _0xd5a662b3)
        {
            _0xd0db2134._0x8a356fb7(_0x333b49ce, _0xd5a662b3);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0x1901d6f0(string _0x4e138e32)
        {
            _0xd0db2134._0x2d60757b(_0x4e138e32);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0x2d56cf92(string _0xaf433059)
        {
            _0xd0db2134._0x3737e8f2(_0xaf433059);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0xde7f08c6(string _0xdc0122fb)
        {
            _0xd0db2134._0x5be235ad(_0xdc0122fb);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0xc1e17d1c(string _0xa808b66d, string _0x21d40ec3)
        {
            _0xd0db2134._0xe25141d3(_0xa808b66d, _0x21d40ec3);
        }

        [UnityEngine.Scripting.Preserve]
        public void _0xc82981a1(string _0xa30bceca, string _0x65d22e51)
        {
            _0xd0db2134._0x7d026a51(_0xa30bceca, _0x65d22e51);
        }

        [UnityEngine.Scripting.Preserve]
        public int _0x0c9ad548(string _0xf4d962e3)
        {
            _0x6a3f051f _0x7881f4bd = _0xd0db2134;
            if (_0x7881f4bd == null || _0x7881f4bd._0x4834039c || _0x7881f4bd.AllowCapture == null)
                return 1;
            try
            {
                return _0x7881f4bd.AllowCapture(_0xf4d962e3 ?? string.Empty);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[17] { 111, 93, 90, 110, 81, 93, 79, 24, 91, 89, 72, 76, 77, 74, 93, 2, 24 }, 56) + exception.Message);
                return 0;
            }
        }
    }

    public event Action<string> PopupOpened;
    public void _0x8757fc18(string _0x3b9302fb)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[12] { 194, 212, 197, 228, 194, 212, 195, 240, 214, 212, 223, 197 }, 177), _0x3b9302fb ?? string.Empty);
    }

    public event Action<string> PageCommitted;
    public void _0xd4ab923c()
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[9] { 4, 20, 5, 24, 27, 27, 35, 24, 7 }, 119));
    }

    public bool _0x532683fc()
    {
        return _0xa7a7537e(_0xe20ad544._0x59d74307(new byte[13] { 196, 203, 200, 212, 194, 243, 200, 215, 247, 200, 215, 210, 215 }, 167));
    }

    public event Action<string> PageError;
    private void _0x5be235ad(string _0x05cd679c)
    {
        _0xf8bb4844(PopupClosed, _0x05cd679c ?? string.Empty);
    }

    public void _0xfd7494b9(string _0xca362aed)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[4] { 11, 8, 6, 3 }, 103), _0xca362aed ?? string.Empty);
    }

    private _0xe24c721a _0x209543c2;
    public event Action<float> ProgressChanged;
    private void _0x2951377f(string _0x093f9707)
    {
        _0xf8bb4844(PageCommitted, _0x093f9707 ?? string.Empty);
    }

    public bool _0x912078eb(string _0xc6775da3)
    {
        if (_0x80b99a93 == null || _0x4834039c || string.IsNullOrEmpty(_0xc6775da3))
            return false;
        try
        {
            return _0x80b99a93.Call<bool>(_0xe20ad544._0x59d74307(new byte[10] { 114, 109, 114, 119, 114, 67, 110, 107, 116, 103 }, 2), _0xc6775da3);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[20] { 85, 103, 96, 84, 107, 103, 117, 34, 114, 109, 114, 119, 114, 67, 110, 107, 116, 103, 56, 34 }, 2) + exception.Message);
            return false;
        }
    }

    public void _0xce2a8635(bool _0x6abcdfe0)
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[9] { 80, 70, 87, 115, 66, 86, 80, 70, 71 }, 35), _0x6abcdfe0);
    }

    private AndroidJavaObject _0x80b99a93;
    public void _0x82629cdd()
    {
        _0x9ea1a5f3(_0xe20ad544._0x59d74307(new byte[4] { 89, 66, 69, 93 }, 42));
    }

    private void _0xf8bb4844<T1, T2>(Action<T1, T2> _0x48e7aee8, T1 _0xf5c79478, T2 _0x2eb314dc)
    {
        if (_0x4834039c || _0x48e7aee8 == null)
            return;
        try
        {
            _0x48e7aee8(_0xf5c79478, _0x2eb314dc);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[18] { 134, 180, 179, 135, 184, 180, 166, 241, 178, 176, 189, 189, 179, 176, 178, 186, 235, 241 }, 209) + exception.Message);
        }
    }

    public Func<string, bool, bool, bool> HandleRequest;
    private void _0x9ea1a5f3(string _0xf3b3bbb3, params object[] _0xebf3000d)
    {
        if (_0x80b99a93 == null || _0x4834039c)
            return;
        try
        {
            if (_0xebf3000d == null || _0xebf3000d.Length == 0)
                _0x80b99a93.Call(_0xf3b3bbb3);
            else
                _0x80b99a93.Call(_0xf3b3bbb3, _0xebf3000d);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(_0xe20ad544._0x59d74307(new byte[8] { 135, 181, 178, 134, 185, 181, 167, 240 }, 208) + _0xf3b3bbb3 + _0xe20ad544._0x59d74307(new byte[2] { 160, 186 }, 154) + exception.Message);
        }
    }
}

internal static class _0xe20ad544
{
    internal static string _0x59d74307(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}