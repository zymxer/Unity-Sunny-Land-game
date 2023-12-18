using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsUITest : MonoBehaviour
{
    public GameObject target;
    private StatsContainer statsContainer;

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

    public void DHealthEffect()
    {
        StatsEffect.AddEffect(target, StatType.HEALTH, -5, 3);
    }

    private void Update()
    {
        manaSlider.value = statsContainer.Mana;
        healthSlider.value = statsContainer.Health;
    }

    private void Start()
    {
        statsContainer = target.GetComponent<StatsContainer>();
        manaSlider.maxValue = statsContainer.GetMaxMana();
        healthSlider.maxValue = statsContainer.GetMaxHealth();
        manaSlider.value = statsContainer.Mana;
        healthSlider.value = statsContainer.Health;
    }
}
