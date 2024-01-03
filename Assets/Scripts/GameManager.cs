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

    [SerializeField]
    private GameplayUI gameplayUI;

    [SerializeField]
    private TMP_Text qualityText;

    const string keyHighScore = "HighScoreLevel1";

    private int lives = 3;
    private int points = 0;
    private int enemiesKilled = 0;
    private int keysFound = 0;

    private Timer timer;
    private int minutes;
    private int seconds;



    //[SerializeField] private Slider volumeSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InGame();

        timer = gameObject.AddComponent<Timer>();
        timer.OnValueChanged().AddListener(OnTimerChange);
        timer.SetTimer(10000, true, false);

        if(!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        //qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    private void Start()
    {
        gameplayUI.UpdateLivesImages();
        gameplayUI.UpdateKeysImages();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState();
        }
    }

    //public void SetVolume()
    //{
    //    AudioListener.volume = volumeSlider.value;
    //}

    public int Lives
    {
        get { return lives; }
    }

    public int KeysFound
    {
        get { return keysFound; }
    }

    public string TimeToString()
    {
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public float TimePast()
    {
        return timer.TimePast();
    }

    private void OnTimerChange()
    {
        minutes = (int)timer.TimePast() / 60;
        seconds = (int)timer.TimePast() % 60;
        //timerText.text = timer.TimePast().ToString();
        //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateEnemies()
    {
        enemiesKilled++;
        //enemiesText.text = enemiesKilled.ToString();
    }

    //public void AddKeys()
    //{
    //    keysTab[keysFound++].color = Color.white;
    //}

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

    //public void EnableLives(int pLives)
    //{
    //    for(int i = 0; i < lives.Length; i++)
    //    {
    //        if(i < pLives)
    //        {
    //            lives[i].enabled = true;
    //        }
    //        else
    //        {
    //            lives[i].enabled = false;
    //        }
    //    }
    //}

    private void SetGameState(GameState state)
    {
        currentGameState = state;

        //inGameCanvas.SetActive(currentGameState == GameState.GS_GAME);
        //pauseMenuCanvas.SetActive(currentGameState == GameState.GS_PAUSEMENU);
        //levelCompletedCanvas.SetActive(currentGameState == GameState.GS_LEVELCOMPLETED);
        //optionsCanvas.SetActive(currentGameState == GameState.GS_OPTIONS);

        if (currentGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int highScore = PlayerPrefs.GetInt(keyHighScore);
            if (currentScene.name == "Level1")
            {
                //if(highScore < ScoreController.GetController().GetScore())
                //{
                //    highScore = ScoreController.GetController().GetScore();
                //    PlayerPrefs.SetInt(keyHighScore, highScore);
                //}
            }
            //scoreText.text = "Your score = " + ScoreController.GetController().GetScore();
            //highScoreText.text = "The best score = " + highScore;
        }
        if(currentGameState == GameState.GS_GAME)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
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
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void OnExitButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
