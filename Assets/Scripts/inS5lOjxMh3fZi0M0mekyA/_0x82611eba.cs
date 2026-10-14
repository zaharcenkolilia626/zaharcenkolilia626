using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The menu's primary call to action. The template holds a reference to this Button but
/// never subscribes to it, so the listener is ours. Scene changes go by build index only -
/// obfuscation renames scene assets, never indices.
/// </summary>
public sealed class _0x82611eba : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    private void _0x0425e7b6()
    {
        if (this._0x8e5b779f)
        {
            return;
        }

        this._0x8e5b779f = true;
        if (this._progress != null)
        {
            this._progress._0xa00752ac(this._progress._0xbd59124c);
        }

        SceneManager.LoadScene(this._sceneIndex);
    }

    [SerializeField]
    private int _sceneIndex = 1;
    [SerializeField]
    private _0x9e4438d2 _progress;
    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0x0425e7b6());
        }
    }

    private bool _0x8e5b779f;
}