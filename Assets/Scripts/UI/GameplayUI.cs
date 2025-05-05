using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI instance;

    [SerializeField] private StatsContainer playerStats;

    [SerializeField] private SpellsController spellsController;

    [Header("UI Panels")] [Space(10)] [SerializeField]
    private Canvas gameplayCanvas;

    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private Canvas optionsCanvas;

    [SerializeField] private Canvas levelEndCanvas;

    [SerializeField] private Canvas gameOverCanvas;

    [Header("Stats")] [Space(10)] [SerializeField]
    private Slider playerHealth;

    [SerializeField] private Slider playerMana;

    [Header("Spells")] [Space(10)] [SerializeField]
    private Image[] spellsImages;

    [SerializeField] private SliderTimer[] spellsCooldowns;

    [Header("Other Stats")] [Space(10)] [SerializeField]
    private Image[] livesImages;

    [SerializeField] private TMP_Text timeText;

    [SerializeField] private Image[] keysImages;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private TMP_Text enemiesText;

    [Header("Settings")] [Space(10)] [SerializeField]
    private TMP_Text qualityText;

    [SerializeField] private Slider volumeSlider;

    [Header("Level End")] [Space(10)] [SerializeField]
    private TMP_Text scoreResultText;

    [SerializeField] private TMP_Text bestScoreText;

    private Canvas activeCanvas;

    private Image selectedSpellImage;

    private void Awake()
    {
        if (instance == null) instance = this;

        activeCanvas = gameplayCanvas;

        selectedSpellImage = spellsImages[0];
        for (var i = 1; i < spellsImages.Length; i++) spellsImages[i].color = Color.grey;
    }

    private void Start()
    {
        playerHealth.maxValue = playerStats.GetMaxHealth();
        playerMana.maxValue = playerStats.GetMaxMana();

        for (var i = 0; i < spellsCooldowns.Length; i++)
            spellsCooldowns[i].SetTimerSlider(spellsController.GetCooldownTimer(i));

        UpdateQualityText();
        UpdateSpellImages();
    }

    private void Update()
    {
        playerHealth.value = playerStats.Health;
        playerMana.value = playerStats.Mana;

        timeText.text = GameManager.instance.TimeToString();
    }

    public void UpdateSpellImages()
    {
        selectedSpellImage.color = Color.gray;
        selectedSpellImage = spellsImages[spellsController.SelectedSpellIndex];
        selectedSpellImage.color = Color.white;
    }

    public void UpdateLivesImages()
    {
        for (var i = 0; i < livesImages.Length; i++)
            livesImages[i].enabled = i < GameManager.instance.Lives ? true : false;
    }

    public void UpdateKeysImages()
    {
        for (var i = 0; i < keysImages.Length; i++)
            keysImages[i].color = i < GameManager.instance.KeysFound ? Color.white : Color.gray;
    }

    public void UpdateScoreResult(int score, int bestScore)
    {
        scoreResultText.text = "Your score = " + score;
        bestScoreText.text = "The best score = " + bestScore;
    }

    public void UpdateScore(int value)
    {
        scoreText.text = value.ToString();
    }

    public void UpdateEnemiesKilled(int value)
    {
        enemiesText.text = value.ToString();
    }

    public void ChangeState(GameState state)
    {
        activeCanvas.enabled = false;
        switch (state)
        {
            case GameState.GS_GAME:
                activeCanvas = gameplayCanvas;
                if (!GameManager.instance.GameStarted) return;
                break;
            case GameState.GS_PAUSEMENU:
                activeCanvas = pauseCanvas;
                break;
            case GameState.GS_OPTIONS:
                activeCanvas = optionsCanvas;
                break;
            case GameState.GS_LEVELCOMPLETED:
                activeCanvas = levelEndCanvas;
                break;
            case GameState.GS_GAME_OVER:
                activeCanvas = gameOverCanvas;
                break;
        }

        activeCanvas.enabled = true;
    }

    public void OptionsButton()
    {
        GameManager.instance.Options();
    }

    public void ResumeButton()
    {
        GameManager.instance.InGame();
    }

    public void RestartButton()
    {
        GameManager.instance.OnRestartButton();
    }

    public void ExitButton()
    {
        GameManager.instance.OnExitButton();
    }

    public void IncreaseQualityButton()
    {
        GameManager.instance.IncreaseQuality();
        UpdateQualityText();
    }

    public void DecreaseQualityButton()
    {
        GameManager.instance.DecreaseQuality();
        UpdateQualityText();
    }

    public void UpdateQualityText()
    {
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public float VolumeSliderValue()
    {
        return volumeSlider.value;
    }

    public void UpdateVolumeLevel()
    {
        GameManager.instance.SetVolume();
    }
}