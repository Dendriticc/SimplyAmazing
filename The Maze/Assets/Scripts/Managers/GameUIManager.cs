using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    Canvas mainUI, winnerUI, loserUI;

    void Awake()
    {
        mainUI.enabled = true;
        winnerUI.enabled = false;
        loserUI.enabled = false;
    }

    public void WinGame()
    {
        GameManager.IsGameRunning = false;
        mainUI.enabled = false;
        winnerUI.enabled = true;
        foreach (EnemyStateManager enemy in FindObjectsOfType<EnemyStateManager>()) enemy.enabled = false;
    }

    public void LoseGame()
    {
        GameManager.IsGameRunning = false;
        mainUI.enabled = false;
        loserUI.enabled = true;
        foreach (EnemyStateManager enemy in FindObjectsOfType<EnemyStateManager>()) enemy.enabled = false;
    }

    public void EndGame(){GameManager.EndGame();}

    public void ContinueGame()
    {
        GameManager.AnteUp();
        GameManager.ReloadScene();
        GameManager.IsGameRunning = true;
    }
}
