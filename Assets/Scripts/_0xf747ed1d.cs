using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class _0xf747ed1d : MonoBehaviour
{
    private void _0x04408472()
    {
        float _0x11092247 = Time.time - _0xfce97d84;
        float _0xed286c34 = Mathf.Clamp01(_0x11092247 / slider_wait_time);
        slider_loading.value = _0xed286c34 * 100;
    }

    private void _0xd8893e20()
    {
    // any UI setup can go here if needed
    }

    [SerializeField]
    private _0xe908af87 _panelsFlow;
    private float _0xfce97d84;
    private void _0x3ea10fab()
    {
        _0x04408472();
        _0x3833c4c9();
    }

    private void _0x3833c4c9()
    {
        if (!_0x6a66d1f2)
        {
            float _0x73033af4 = Time.time - _0xfce97d84;
            if (_0x73033af4 >= slider_wait_time)
            {
                _0xbf1b727b();
                _0x6a66d1f2 = true;
            }
        }
    }

    private void _0xbf1b727b()
    {
        _panelsFlow._0xbbe3098d(_panelsFlow.panel_menu, _panelsFlow.panel_loader);
    }

    private void Update()
    {
        _0x3ea10fab();
    }

    [SerializeField]
    private int slider_wait_time = 30;
    private void Start()
    {
        _0xd8893e20();
        _0xfce97d84 = Time.time;
    }

    private void Awake()
    {
        _panelsFlow = _0xe908af87.instance;
    }

    private bool _0x6a66d1f2 = false;
    [SerializeField]
    private Slider slider_loading;
}