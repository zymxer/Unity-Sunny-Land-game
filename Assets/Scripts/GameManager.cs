using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_LEVELCOMPLETED,
    GS_GAME_OVER,
    GS_OPTIONS
}

public class GameManager : MonoBehaviour
{
    private const string keyHighScore = "HighScore196723";
    public static GameManager instance;
    public GameState currentGameState;

    [SerializeField] private GameObject player;

    [SerializeField] private GameplayUI gameplayUI;

    [SerializeField] private Transform lastCheckpoint;

    private int minutes;

    private StatsContainer playerStats;
    private int seconds;

    private Timer timer;

    public int Lives { get; private set; } = 3;

    public int KeysFound { get; private set; }

    public int Score { get; private set; }

    public int EnemiesKilled { get; private set; }

    public bool GameStarted { get; private set; }

    public Transform Checkpoint
    {
        get => lastCheckpoint;
        set => lastCheckpoint = value;
    }


    //[SerializeField] private Slider volumeSlider;

    private void Awake()
    {
        if (instance == null) instance = this;

        timer = gameObject.AddComponent<Timer>();
        timer.OnValueChanged().AddListener(OnTimerChange);
        timer.SetTimer(10000);

        if (!PlayerPrefs.HasKey(keyHighScore)) PlayerPrefs.SetInt(keyHighScore, 0);

        playerStats = player.GetComponent<StatsContainer>();
    }

    private void Start()
    {
        //InGame();

        gameplayUI.UpdateLivesImages();
        gameplayUI.UpdateKeysImages();
        gameplayUI.UpdateScore(Score);
        gameplayUI.UpdateEnemiesKilled(EnemiesKilled);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchState();

        if (Input.GetKeyDown(KeyCode.P)) Freeze.SpeedUpPlayer();
    }

    public void StartGame()
    {
        GameStarted = true;
    }

    public void ActivateTimer()
    {
        timer.Activate();
    }

    public void IncreaseScore(int value)
    {
        Score += value;
        gameplayUI.UpdateScore(Score);
    }

    public void IncreaseEnemiesKilled()
    {
        EnemiesKilled++;
        gameplayUI.UpdateEnemiesKilled(EnemiesKilled);
    }

    public void IncreaseKeysAmount()
    {
        KeysFound++;
        gameplayUI.UpdateKeysImages();
    }

    public void IncreaseLives()
    {
        Lives = Mathf.Min(Lives + 1, 8);
        gameplayUI.UpdateLivesImages();
    }

    public void DecreaseLives()
    {
        Lives--;
        if (Lives == -1) GameOver();
        gameplayUI.UpdateLivesImages();
    }

    public void KillPlayer()
    {
        DecreaseLives();
        player.transform.position = lastCheckpoint.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<StatsContainer>().HealAll();
        SpellsController.instance.StopContinuousSpell();
        SpellsController.instance.ResetCooldowns();
        StatsEffect.ClearPlayerEffects();
        Freeze.SpeedUpPlayer();
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

        if (currentGameState == GameState.GS_LEVELCOMPLETED)
        {
        }

        if (currentGameState == GameState.GS_GAME)
        {
            if (GameStarted) SpellsController.instance.enabled = true;
            Time.timeScale = 1.0f;
        }
        else
        {
            SpellsController.instance.StopContinuousSpell();
            SpellsController.instance.enabled = false;
            Time.timeScale = 0.0f;
        }
    }

    public void CheckKeysFound()
    {
        if (KeysFound >= 3) LevelCompleted();
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
        Score += Lives * 100;
        var highScore = PlayerPrefs.GetInt(keyHighScore);
        if (highScore < Score)
        {
            highScore = Score;
            PlayerPrefs.SetInt(keyHighScore, highScore);
        }

        gameplayUI.UpdateScoreResult(Score, highScore);
    }

    public void GameOver()
    {
        GetComponent<AudioSource>().Play();
        SetGameState(GameState.GS_GAME_OVER);
    }

    private void SwitchState()
    {
        if (currentGameState == GameState.GS_PAUSEMENU || currentGameState == GameState.GS_OPTIONS)
            InGame();
        else if (currentGameState == GameState.GS_GAME) PauseMenu();
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