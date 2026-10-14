using UnityEngine;

public class _0xe908af87 : MonoBehaviour
{
    public GameObject panel_loader;
    public void _0xfc07c7a5(GameObject _0x0fa6625a)
    {
        _0x0fa6625a.SetActive(false);
    }

    public void _0xbbe3098d(GameObject _0x5163c23b, GameObject _0x8e3fcbde)
    {
        _0x5163c23b.SetActive(true);
        _0x8e3fcbde.SetActive(false);
    }

    public GameObject panel_menu;
    public static _0xe908af87 instance;
    public void _0xab3e70dd(GameObject _0x895a38d7)
    {
        _0x895a38d7.SetActive(true);
    }

    private void Awake()
    {
        instance = this;
    }
}