using TMPro;
using UnityEngine;

/// <summary>
/// Blanks the template labels this game does not dress. The scene files already carry
/// empty strings for them; this is the runtime guard that keeps a stray placeholder from
/// reaching the screen if anything re-serialises one. It clears text only and never
/// switches a panel off - the template's own controllers address them by reference.
/// </summary>
public sealed class _0x87f45db7 : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] _undressed = new TMP_Text[0];
    private void Start()
    {
        for (int _0xf8261581 = 0; _0xf8261581 < this._undressed.Length; _0xf8261581++)
        {
            if (this._undressed[_0xf8261581] != null)
            {
                this._undressed[_0xf8261581].text = string.Empty;
            }
        }
    }
}