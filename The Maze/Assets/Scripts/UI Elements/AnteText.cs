using UnityEngine;
using TMPro;

public class AnteText : MonoBehaviour
{
    [SerializeField]
    TMP_Text Ante;

    void Awake()
    {
        if (GameManager.HardModeEnabled) Ante.text = "Hard Ante: ";
        else Ante.text = "Ante: ";
        
        Ante.text += GameManager.Ante;
    }
}
