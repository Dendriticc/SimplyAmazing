using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [SerializeField]
    TMP_Text timer;

    void Update()
    {
        timer.text = "Time Remaining: " + Mathf.FloorToInt(GameManager.remainingTime / 60f).ToString("00") + ":" + Mathf.FloorToInt(GameManager.remainingTime % 60).ToString("00");
    }
}
