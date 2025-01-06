using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int Length, Width, NumberOfEnemies;
    public static float remainingTime = 600;
    float minutes;
    float seconds;

    public static int Ante;
    public static int startingHealth;
    static int difficultyScale;
    static int min = 7, max = 10;
    public static bool HardModeEnabled;
    public static bool IsGameRunning;

    [Range(0.01f, 1.0f)] public static float ProbabilityOfGoodItem, ProbabilityOfBadItem, ProbabilityOfMystery;
    
    void Awake()
    {
       if (instance != null) { Destroy(gameObject); return; }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Ante = 1;
        NumberOfEnemies = 1;
        Length = Random.Range(min, max) * 2 + 1;
        Width = Random.Range(min, max) * 2 + 1;
        SetProbabilities();
    }
    void Update()
    {
        if (IsGameRunning) remainingTime -= Time.deltaTime;
    }

    public static void AnteUp()
    {
        Ante++;
        min += difficultyScale;
        max += difficultyScale;
        Length = Random.Range(min, max) * 2 + 1;
        Width = Random.Range(min, max) * 2 + 1;
        ProbabilityOfGoodItem -= difficultyScale / 2000f;
        ProbabilityOfBadItem += difficultyScale / 1000f;
        ProbabilityOfMystery += difficultyScale / 750f;
        if (HardModeEnabled)
        {
            difficultyScale += 2;
            NumberOfEnemies += 2;
        }
        else
        {
            difficultyScale++;
            NumberOfEnemies++;
        }
    }

    static void SetProbabilities()
    {
        ProbabilityOfGoodItem = 0.03f;
        ProbabilityOfBadItem = 0.02f;
        ProbabilityOfMystery = 0.03f;
    }

    public static void EndGame()
    {
        SceneManager.LoadScene("Menu");
        Ante = 1;
        difficultyScale = 0;
        min = 7;
        max = 10;
    }

    public static void ReloadScene()
    {
        // SceneManager.LoadScene("Menu");
        SceneManager.LoadScene("Maze");
        IsGameRunning = true;
    }
}
