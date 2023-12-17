using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsUITest : MonoBehaviour
{
    public StatsContainer statsContainer;

    public Slider manaSlider;
    public Slider healthSlider;


    public void DecreaseMana()
    {
        statsContainer.DecreaseMana(30);
    }

    public void DecreaseHealth()
    {
        statsContainer.DecreaseHP(20);
    }

    private void Update()
    {
        manaSlider.value = statsContainer.Mana;
        healthSlider.value = statsContainer.Health;
    }

    private void Start()
    {
        manaSlider.maxValue = statsContainer.GetMaxMana();
        healthSlider.maxValue = statsContainer.GetMaxHealth();
        manaSlider.value = statsContainer.Mana;
        healthSlider.value = statsContainer.Health;
    }
}
