using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class _0xa1a91ae5 : MonoBehaviour
{
    private void _0x11e6f7d2()
    {
        if (_0xdc141041 != null)
            return;
        _0x1b5cec95();
        _0xdc141041 = new GameObject(_0x43f6bc50._0x5d017c8c(new byte[13] { 183, 149, 142, 145, 134, 132, 158, 164, 143, 149, 136, 138, 130 }, 231), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        _0xdc141041.transform.SetParent(transform, false);
        Canvas _0x8c065c4b = _0xdc141041.GetComponent<Canvas>();
        _0x8c065c4b.renderMode = RenderMode.ScreenSpaceOverlay;
        _0x8c065c4b.sortingOrder = short.MaxValue;
        _0x8c065c4b.overrideSorting = true;
        var _0x6b4bf287 = new GameObject(_0x43f6bc50._0x5d017c8c(new byte[8] { 63, 16, 19, 15, 25, 62, 29, 14 }, 124), typeof(RectTransform), typeof(Image));
        _0x544a5fb9 = _0x6b4bf287.GetComponent<RectTransform>();
        _0x544a5fb9.SetParent(_0xdc141041.transform, false);
        _0x6b4bf287.GetComponent<Image>().color = new Color(0.08f, 0.08f, 0.1f, 1f);
        int _0x570b51b2 = CloseBarPixels();
        _0x8f9651a0(Screen.safeArea, _0x570b51b2);
        Sprite _0xacdeeeb3 = Resources.GetBuiltinResource<Sprite>(_0x43f6bc50._0x5d017c8c(new byte[20] { 239, 243, 149, 233, 209, 211, 212, 149, 239, 243, 233, 202, 200, 211, 206, 223, 148, 202, 201, 222 }, 186));
        var _0xed17aa57 = new GameObject(_0x43f6bc50._0x5d017c8c(new byte[5] { 154, 181, 182, 170, 188 }, 217), typeof(RectTransform), typeof(Image), typeof(Button));
        var _0xca8d5e29 = _0xed17aa57.GetComponent<RectTransform>();
        _0xca8d5e29.SetParent(_0x544a5fb9, false);
        _0xca8d5e29.anchorMin = new Vector2(1f, 0f);
        _0xca8d5e29.anchorMax = new Vector2(1f, 1f);
        _0xca8d5e29.pivot = new Vector2(1f, 0.5f);
        _0xca8d5e29.sizeDelta = new Vector2(Mathf.Max(_0x570b51b2 * 2.4f, 120f), 0f);
        _0xca8d5e29.anchoredPosition = new Vector2(-12f, 0f);
        Image _0x30a11c70 = _0xed17aa57.GetComponent<Image>();
        _0x30a11c70.sprite = _0xacdeeeb3;
        _0x30a11c70.type = Image.Type.Sliced;
        _0x30a11c70.color = new Color(0.95f, 0.96f, 0.98f, 1f);
        _0xed17aa57.GetComponent<Button>().onClick.AddListener(_0x02a16d0f);
        var _0xb8637072 = new GameObject(_0x43f6bc50._0x5d017c8c(new byte[5] { 9, 36, 39, 32, 41 }, 69), typeof(RectTransform), typeof(Text));
        var _0xab8aaa65 = _0xb8637072.GetComponent<RectTransform>();
        _0xab8aaa65.SetParent(_0xca8d5e29, false);
        _0xab8aaa65.anchorMin = Vector2.zero;
        _0xab8aaa65.anchorMax = Vector2.one;
        _0xab8aaa65.offsetMin = _0xab8aaa65.offsetMax = Vector2.zero;
        Text _0xb2943766 = _0xb8637072.GetComponent<Text>();
        _0xb2943766.text = _0x43f6bc50._0x5d017c8c(new byte[5] { 253, 210, 209, 205, 219 }, 190);
        _0xb2943766.font = Resources.GetBuiltinResource<Font>(_0x43f6bc50._0x5d017c8c(new byte[17] { 135, 174, 172, 170, 168, 178, 153, 190, 165, 191, 162, 166, 174, 229, 191, 191, 173 }, 203));
        _0xb2943766.fontSize = Mathf.Max(18, _0x570b51b2 / 3);
        _0xb2943766.alignment = TextAnchor.MiddleCenter;
        _0xb2943766.color = new Color(0.12f, 0.12f, 0.15f, 1f);
    }

    private void OnApplicationFocus(bool _0xea296f4b)
    {
        if (_0xea296f4b && _0xad657e3b)
            _0xf1e9cb9d();
    }

    private void _0x8f9651a0(Rect _0x0170a161, int _0xa6c400f7)
    {
        if (_0x544a5fb9 == null || Screen.width <= 0 || Screen.height <= 0)
            return;
        _0x544a5fb9.anchorMin = new Vector2(_0x0170a161.xMin / Screen.width, (_0x0170a161.yMax - _0xa6c400f7) / Screen.height);
        _0x544a5fb9.anchorMax = new Vector2(_0x0170a161.xMax / Screen.width, _0x0170a161.yMax / Screen.height);
        _0x544a5fb9.offsetMin = _0x544a5fb9.offsetMax = Vector2.zero;
    }

    private void _0xf1e9cb9d()
    {
        if (_0x61d19a64 == null || !_0x61d19a64._0xb52774be || Screen.width <= 0 || Screen.height <= 0)
            return;
        Rect _0x944d0467 = Screen.safeArea;
        int _0xc8c50828 = CloseBarPixels();
        int _0xa001d3ee = Mathf.Max(0, Mathf.RoundToInt(_0x944d0467.xMin));
        int _0x0a4db48f = Mathf.Max(0, Mathf.RoundToInt(Screen.width - _0x944d0467.xMax));
        int _0x261b3c88 = Mathf.Max(0, Mathf.RoundToInt(_0x944d0467.yMin));
        int _0xd2d088b1 = Mathf.Max(0, Mathf.RoundToInt(Screen.height - _0x944d0467.yMax)) + _0xc8c50828;
        _0x61d19a64._0xda2e39e2(_0xa001d3ee, _0xd2d088b1, _0x0a4db48f, _0x261b3c88, Screen.width, Screen.height);
        _0x8f9651a0(_0x944d0467, _0xc8c50828);
        _0xbb0b4f55 = Screen.safeArea;
        _0x13ed1dbd = new Vector2(Screen.width, Screen.height);
    }

    public void _0xec23ef47(string _0x846108a1)
    {
        if (string.IsNullOrWhiteSpace(_0x846108a1))
        {
            Debug.LogError(_0x43f6bc50._0x5d017c8c(new byte[28] { 128, 162, 185, 166, 177, 179, 169, 240, 128, 191, 188, 185, 179, 169, 240, 133, 130, 156, 240, 185, 163, 240, 181, 189, 160, 164, 169, 254 }, 208), this);
            return;
        }

        _0x02a16d0f();
        _0x61d19a64 = new _0x6a3f051f();
        if (!_0x61d19a64._0xb52774be)
        {
            _0x61d19a64._0x7feb9379();
            _0x61d19a64 = null;
            Application.OpenURL(_0x846108a1);
            return;
        }

        _0x61d19a64._0x3a9e0e0c(true);
        _0x61d19a64.HandleRequest = (_0x89e8d832, _0xe6a3e0ff, _0xe05824a2) => true;
        _0xf1e9cb9d();
        _0x61d19a64._0xfd7494b9(_0x846108a1);
        _0x61d19a64._0x82629cdd();
        _0x11e6f7d2();
        _0xad657e3b = true;
        Input.backButtonLeavesApp = false;
    }

    private Rect _0xbb0b4f55;
    private static int CloseBarPixels()
    {
        float _0x3f37c3ba = Screen.dpi > 0f ? Screen.dpi : 160f;
        return Mathf.Max(48, Mathf.RoundToInt(48f * _0x3f37c3ba / 160f));
    }

    private void Update()
    {
        if (!_0xad657e3b || _0x61d19a64 == null)
            return;
        if (_0xbb0b4f55 != Screen.safeArea || _0x13ed1dbd.x != Screen.width || _0x13ed1dbd.y != Screen.height)
        {
            _0xf1e9cb9d();
        }

        if (BackReleased())
            _0x2af0fa32();
    }

    private Vector2 _0x13ed1dbd;
    private void _0x2af0fa32()
    {
        if (_0x61d19a64 == null)
        {
            _0x02a16d0f();
            return;
        }

        if (_0x61d19a64._0x532683fc())
            return;
        if (_0x61d19a64._0x7bde7cb2)
        {
            _0x61d19a64._0x2f6580a7();
            return;
        }

        _0x02a16d0f();
    }

    private void _0x02a16d0f()
    {
        _0xad657e3b = false;
        if (_0x61d19a64 != null)
        {
            _0x61d19a64._0x2543dec5();
            _0x61d19a64._0x7feb9379();
            _0x61d19a64 = null;
        }

        if (_0xdc141041 != null)
        {
            Destroy(_0xdc141041);
            _0xdc141041 = null;
            _0x544a5fb9 = null;
        }
    }

    public static bool _0xad657e3b { get; private set; }

    private _0x6a3f051f _0x61d19a64;
    private RectTransform _0x544a5fb9;
    private void _0x1b5cec95()
    {
        EventSystem _0xefe4f182 = null;
        EventSystem[] _0x3f09ec7d = Resources.FindObjectsOfTypeAll<EventSystem>();
        for (int _0xb9927af1 = 0; _0xb9927af1 < _0x3f09ec7d.Length; _0xb9927af1++)
        {
            EventSystem _0x9739d318 = _0x3f09ec7d[_0xb9927af1];
            if (_0x9739d318 == null || !_0x9739d318.isActiveAndEnabled)
                continue;
            if (_0x9739d318.gameObject.hideFlags != HideFlags.None)
                continue;
            _0xefe4f182 = _0x9739d318;
            break;
        }

        if (_0xefe4f182 == null)
        {
            var _0x2e1d3c1c = new GameObject(_0x43f6bc50._0x5d017c8c(new byte[14] { 59, 25, 2, 29, 10, 8, 18, 59, 4, 2, 5, 31, 14, 25 }, 107));
            _0x2e1d3c1c.transform.SetParent(transform, false);
            _0x2e1d3c1c.AddComponent<EventSystem>();
            _0x2e1d3c1c.AddComponent<InputSystemUIInputModule>();
            return;
        }

        if (_0xefe4f182.GetComponent<InputSystemUIInputModule>() == null)
        {
            StandaloneInputModule _0x1ae7cc6d = _0xefe4f182.GetComponent<StandaloneInputModule>();
            if (_0x1ae7cc6d != null)
                Destroy(_0x1ae7cc6d);
            _0xefe4f182.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private static bool BackReleased()
    {
        Keyboard _0xd1aee18c = Keyboard.current;
        if (_0xd1aee18c != null && _0xd1aee18c.escapeKey.wasReleasedThisFrame)
            return true;
#if ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyUp(KeyCode.Escape);
#else
        return false;
#endif
    }

    private void OnDestroy()
    {
        _0x02a16d0f();
    }

    private GameObject _0xdc141041;
}

internal static class _0x43f6bc50
{
    internal static string _0x5d017c8c(byte[] data, byte key)
    {
        var buffer = new byte[data.Length];
        for (var i = 0; i < data.Length; i++)
            buffer[i] = (byte)(data[i] ^ key);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
}