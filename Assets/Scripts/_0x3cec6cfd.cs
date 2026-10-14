using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class _0x3cec6cfd : MonoBehaviour
{
    private void Start()
    {
        _0xaa7aae06();
    }

    private void _0xaa7aae06()
    {
        button_ui_main_privacy_policy.onClick.AddListener(() => _controllerPrivacyPolicy._0xec23ef47(privacyPolicy));
    }

    private void Update()
    {
        _0xf0c9bbee();
    }

    private void _0xf0c9bbee()
    {
        _0x8771426a();
    }

    [SerializeField]
    private _0xa1a91ae5 _controllerPrivacyPolicy;
    //ui
    [SerializeField]
    private TextMeshProUGUI text_ui_main_score;
    [SerializeField]
    private Button button_ui_main_privacy_policy;
    [SerializeField]
    private _0xe908af87 _panelsFlow;
    [SerializeField]
    private Button button_ui_main_play;
    private void _0x8771426a()
    {
        text_ui_main_score.text = game_score.ToString();
    }

    //gameplay
    [SerializeField]
    private int game_score = 0;
    private void Awake()
    {
        _panelsFlow = _0xe908af87.instance;
    }

    [SerializeField]
    private string privacyPolicy = "";
}