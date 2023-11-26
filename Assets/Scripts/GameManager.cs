using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS }
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameState currentGameState;

    [SerializeField] private Image[] keysTab;
    [SerializeField] private Image[] lives;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text enemiesText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text qualityText;

    const string keyHighScore = "HighScoreLevel1";

    private int minutes;
    private int seconds;
    private int keysFound = 0;
    private int enemiesKilled = 0;

    [SerializeField] private GameObject inGameCanvas;
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject levelCompletedCanvas;
    [SerializeField] private GameObject optionsCanvas;

    [SerializeField] private Slider volumeSlider;

    private void Awake()
    {
        InGame();
        if(instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.grey;
        }
        timer.OnValueChanged().AddListener(OnTimerChange);
        if(!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState();
        }
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    private void OnTimerChange()
    {
        minutes = (int)timer.TimePast() / 60;
        seconds = (int)timer.TimePast() % 60;
        //timerText.text = timer.TimePast().ToString();
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateEnemies()
    {
        enemiesKilled++;
        enemiesText.text = enemiesKilled.ToString();
    }

    public void AddKeys()
    {
        keysTab[keysFound++].color = Color.white;
    }

    public void IncreaseQuality()
    {
        QualitySettings.IncreaseLevel();
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void DecreaseQuality()
    {
        QualitySettings.DecreaseLevel();
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void EnableLives(int pLives)
    {
        for(int i = 0; i < lives.Length; i++)
        {
            if(i < pLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }

    private void SetGameState(GameState state)
    {
        currentGameState = state;

        inGameCanvas.SetActive(currentGameState == GameState.GS_GAME);
        pauseMenuCanvas.SetActive(currentGameState == GameState.GS_PAUSEMENU);
        levelCompletedCanvas.SetActive(currentGameState == GameState.GS_LEVELCOMPLETED);
        optionsCanvas.SetActive(currentGameState == GameState.GS_OPTIONS);

        if (currentGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int highScore = PlayerPrefs.GetInt(keyHighScore);
            if (currentScene.name == "Level1")
            {
                if(highScore < ScoreController.GetController().GetScore())
                {
                    highScore = ScoreController.GetController().GetScore();
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }
            }
            scoreText.text = "Your score = " + ScoreController.GetController().GetScore();
            highScoreText.text = "The best score = " + highScore;
        }
        if(currentGameState == GameState.GS_GAME)
        {
            timer.Continue();
        }
        else
        {
            timer.Stop();
        }
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }
    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    private void SwitchState()
    {
        if (currentGameState == GameState.GS_PAUSEMENU)
        {
            InGame();
        }
        else
        {
            PauseMenu();
        }
    }

    public void OnResumeButton()
    {
        InGame();
    }

    public void OnRestartButton()
    {
        Debug.Log("I am");
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void OnExitButton()
    {
        Debug.Log("Stupid");
        //SceneManager.LoadSceneAsync("MainMenu");
    }
}
