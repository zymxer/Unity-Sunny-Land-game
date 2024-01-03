using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private StatsContainer playerStats;
    [SerializeField]
    private SpellsController spellsController;

    [Header("UI Panels")]
    [Space(10)]
    [SerializeField]
    private GameObject gameplayPanel;

    [Header("Stats")]
    [Space(10)]
    [SerializeField]
    private Slider playerHealth;
    [SerializeField]
    private Slider playerMana;

    [Header("Spells")]
    [Space(10)]
    [SerializeField]
    private Image[] spellsImages;
    [SerializeField]
    private SliderTimer[] spellsCooldowns;

    [Header("Other Stats")]
    [Space(10)]
    [SerializeField]
    private Image[] livesImages;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private Image[] keysImages;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text enemiesText;


    private Image selectedSpellImage;

    private void Start()
    {
        playerHealth.maxValue = playerStats.GetMaxHealth();
        playerMana.maxValue = playerStats.GetMaxMana();

        for(int i = 0; i < spellsCooldowns.Length; i++)
        {
            spellsCooldowns[i].SetTimerSlider(spellsController.GetCooldownTimer(i));
        }

        selectedSpellImage = spellsImages[0];
        selectedSpellImage.color = Color.white;
        for(int i = 1;  i < spellsImages.Length; i++)
        {
            spellsImages[i].color = Color.grey;
        }
    }

    private void Update()
    {
        playerHealth.value = playerStats.Health;
        playerMana.value = playerStats.Mana;

        timeText.text = GameManager.instance.TimeToString();

        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            selectedSpellImage.color = Color.grey;
            selectedSpellImage = spellsImages[spellsController.SelectedSpellIndex];
            selectedSpellImage.color = Color.white;
        }
    }

    public void UpdateLivesImages()
    {
        for(int i = 0; i < livesImages.Length; i++)
        {
            livesImages[i].enabled = i < GameManager.instance.Lives ? true : false;
        }
    }

    public void UpdateKeysImages()
    {
        for (int i = 0; i < keysImages.Length; i++)
        {
            keysImages[i].color = i < GameManager.instance.KeysFound ? Color.white : Color.gray;
        }
    }
}
