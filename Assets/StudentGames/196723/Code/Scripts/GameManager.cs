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
    private GameObject player;
    [SerializeField]
    private GameplayUI gameplayUI;

    [SerializeField]
    private Transform lastCheckpoint;

    const string keyHighScore = "HighScoreLevel1";

    private int lives = 3;
    private int score = 0;
    private int enemiesKilled = 0;
    private int keysFound = 0;

    private Timer timer;
    private int minutes;
    private int seconds;

    private bool gameStarted = false;


    //[SerializeField] private Slider volumeSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        timer = gameObject.AddComponent<Timer>();
        timer.OnValueChanged().AddListener(OnTimerChange);
        timer.SetTimer(10000);

        if(!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        //qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    private void Start()
    {
        //InGame();

        gameplayUI.UpdateLivesImages();
        gameplayUI.UpdateKeysImages();
        gameplayUI.UpdateScore(score);
        gameplayUI.UpdateEnemiesKilled(enemiesKilled);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState();
        }
    }

    public int Lives
    {
        get { return lives; }
    }

    public int KeysFound
    {
        get { return keysFound; }
    }

    public int Score
    {
        get { return score; }
    }

    public int EnemiesKilled
    { 
        get { return enemiesKilled; } 
    }

    public bool GameStarted
    {
        get { return gameStarted; }
    }

    public Transform Checkpoint
    {
        get { return lastCheckpoint; }
        set { lastCheckpoint = value; }
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void ActivateTimer()
    {
        timer.Activate();
    }

    public void IncreaseScore(int value)
    {
        score += value;
        gameplayUI.UpdateScore(score);
    }

    public void IncreaseEnemiesKilled()
    {
        enemiesKilled++;
        gameplayUI.UpdateEnemiesKilled(enemiesKilled);
    }

    public void IncreaseKeysAmount()
    {
        keysFound++;
        gameplayUI.UpdateKeysImages();
    }

    public void IncreaseLives()
    {
        lives = Mathf.Min(lives+1, 8);
        gameplayUI.UpdateLivesImages();
    }

    public void DecreaseLives()
    {
        lives--;
        if(lives == -1)
        {
            GameOver();
        }
        gameplayUI.UpdateLivesImages();
    }

    public void KillPlayer()
    {
        DecreaseLives();
        player.transform.position = lastCheckpoint.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<StatsContainer>().HealAll();
        StatsEffect.ClearPlayerEffects();
    }


    public void SetVolume()
    {
        AudioListener.volume = gameplayUI.VolumeSliderValue();
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
    }

    public void IncreaseQuality()
    {
        QualitySettings.IncreaseLevel();
    }

    public void DecreaseQuality()
    {
        QualitySettings.DecreaseLevel();
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
        gameplayUI.ChangeState(currentGameState);

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
        }
        if(currentGameState == GameState.GS_GAME)
        {
            if(gameStarted)
            {
                SpellsController.instance.enabled = true;
            }
            Time.timeScale = 1.0f;
        }
        else
        {
            SpellsController.instance.StopContinuousSpell();
            SpellsController.instance.enabled = false;
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
        gameplayUI.UpdateSpellImages();
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
        if (currentGameState == GameState.GS_PAUSEMENU || currentGameState == GameState.GS_OPTIONS)
        {
            InGame();
        }
        else
        {
            PauseMenu();
        }
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
