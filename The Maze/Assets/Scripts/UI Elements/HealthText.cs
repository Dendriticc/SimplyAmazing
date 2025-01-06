using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField]
    TMP_Text Health;

    public void UpdateHealth(int life)
    {
        Health.text = "Life: " + life;
    }
}
