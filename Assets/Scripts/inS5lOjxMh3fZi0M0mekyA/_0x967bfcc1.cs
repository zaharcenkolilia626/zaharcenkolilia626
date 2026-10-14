using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Leaves the current scene for another build index. Used by the game scene's Back slot
/// and by the result cards that return to the temple map.
/// </summary>
public sealed class _0x967bfcc1 : MonoBehaviour
{
    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this._0xe126d473());
        }
    }

    private void _0xe126d473()
    {
        if (this._0xfa732572)
        {
            return;
        }

        this._0xfa732572 = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(this._sceneIndex);
    }

    private bool _0xfa732572;
    [SerializeField]
    private int _sceneIndex;
    [SerializeField]
    private Button _button;
}