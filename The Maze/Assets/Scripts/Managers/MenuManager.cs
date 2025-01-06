using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public Canvas MainMenu, Guide, DifficultySelection;
    private Canvas previousScene;

    void Awake()
    {
        MainMenu.enabled = true;
        Guide.enabled = false;
        DifficultySelection.enabled = false;
    }

    public void OnPlayButton()
    {
        MainMenu.enabled = false;
        DifficultySelection.enabled = true;
        previousScene = MainMenu;
    }

    public void OnGuideButton()
    {
        MainMenu.enabled = false;
        Guide.enabled = true;
        previousScene = MainMenu;
    }

    public void OnDifficultySelection(bool IsHardModeButton)
    {
        GameManager.HardModeEnabled = IsHardModeButton;
        GameManager.startingHealth = IsHardModeButton ? 3:5;
        GameManager.IsGameRunning = true;
        SceneManager.LoadScene("Maze");
    }

    public void OnBackButton(Canvas currentScene)
    {
        currentScene.enabled = false;
        previousScene.enabled = true;
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

}
