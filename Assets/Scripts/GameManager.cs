using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    //define the game states
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    // Store the current game state
    public GameState currentState;

    // Store the previous game state
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stats Display")]
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timeSurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(3);
    public List<Image> chosenPassiveItemsUI = new List<Image>(3);

    [Header("Highscore")]
    public Text highscoreDisplay; // Text to display top 3 highscores
    public GameObject nameInputPanel; // Panel with input field for player name
    public InputField playerNameInputField; // Input field for player name

    [Header("Stopwatch")]
    public float timeLimit; // The time limit in seconds
    float stopwatchTime; // The current time on the stopwatch
    public Text stopwatchDisplay;

    public bool isGameOver = false;

    public bool choosingUpgrade;

    public GameObject playerObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
        DisableScreens();
    }

    void Update()
    {
        // Check if game should end (no more spawning + debug menu not open)
        if (!isGameOver && ShouldGameEnd())
        {
            GameOver();
        }

        // Define the behaviour for each state
        switch (currentState)
        {
            case GameState.Gameplay:
                //code for the gameplay state;
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                //code for the paused state;
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                //code for the game over state;
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                //code for the level up state;
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; // Pause the game
                    Debug.Log("Upgrades Shown");
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Pause the game
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resume the game
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        playerObject.SendMessage("Die");
        ChangeState(GameState.GameOver);
        Debug.Log("Game Over");
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);

        // Hide all in-game HUD elements
        PlayerStats playerStats = playerObject.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.HideHUD();
        }

        // Display highscores
        DisplayHighscores();

        // Check if player qualifies for highscore
        string currentTime = timeSurvivedDisplay.text;
        int position = HighscoreManager.instance.CheckHighscorePosition(currentTime);

        if (position != -1)
        {
            Debug.Log($"Player qualifies for highscore at position {position}");
            ShowNameInputPanel();
        }
        else
        {
            Debug.Log("Player did not qualify for highscore");
        }
    }

    void DisplayHighscores()
    {
        if (highscoreDisplay == null)
            return;

        List<string> highscores = HighscoreManager.instance.GetFormattedHighscores();

        if (highscores.Count == 0)
        {
            highscoreDisplay.text = "No Highscores Yet";
            return;
        }

        string highscoreText = "=== HIGHSCORES ===\n";
        foreach (string entry in highscores)
        {
            highscoreText += entry + "\n";
        }

        highscoreDisplay.text = highscoreText;
    }

    void ShowNameInputPanel()
    {
        if (nameInputPanel != null && playerNameInputField != null)
        {
            nameInputPanel.SetActive(true);
            playerNameInputField.text = "";
            playerNameInputField.ActivateInputField();
        }
    }

    public void SubmitHighscore()
    {
        if (playerNameInputField == null || playerNameInputField.text.Length == 0)
        {
            Debug.LogWarning("Player name is empty!");
            return;
        }

        string playerName = playerNameInputField.text;
        string time = timeSurvivedDisplay.text;

        HighscoreManager.instance.AddHighscore(playerName, time);

        // Refresh highscore display
        DisplayHighscores();

        // Hide name input panel
        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(false);
        }
    }

    bool ShouldGameEnd()
    {
        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
        DebugMenuManager debugMenu = Object.FindAnyObjectByType<DebugMenuManager>();

        if (spawner == null)
            return false;

        // Check if spawning is complete and debug menu is closed
        bool spawnComplete = spawner.IsSpawningComplete();
        bool debugMenuActive = (debugMenu != null && debugMenu.MenuOpen);

        return spawnComplete && !debugMenuActive;
    }

    public void AssignChosenCharacterUI(Sprite icon, string name)
    {
        chosenCharacterImage.sprite = icon;
        chosenCharacterName.text = name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.LogError("Mismatch in the number of UI slots and data provided.");
            return;
        }

        for(int i = 0; i < chosenWeaponUI.Count; i++)
        {
            if(chosenWeaponsData[i].sprite)
            {
                chosenWeaponUI[i].sprite = chosenWeaponsData[i].sprite;
                chosenWeaponUI[i].enabled = true;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
                chosenPassiveItemsUI[i].enabled = true;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60F);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60F);

        // Update stopwatch text to display elapsed time
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        ChangeState(GameState.Gameplay);
    }
}
