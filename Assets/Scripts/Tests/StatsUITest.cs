using UnityEngine;
using UnityEngine.UI;

public class StatsUITest : MonoBehaviour
{
    public GameObject target;

    public Slider manaSlider;
    public Slider healthSlider;
    private StatsContainer statsContainer;

    private void Start()
    {
        //statsContainer = target.GetComponent<StatsContainer>();
        //manaSlider.maxValue = statsContainer.GetMaxMana();
        //healthSlider.maxValue = statsContainer.GetMaxHealth();
        //manaSlider.value = statsContainer.Mana;
        //healthSlider.value = statsContainer.Health;
    }

    private void Update()
    {
        //manaSlider.value = statsContainer.Mana;
        //healthSlider.value = statsContainer.Health;
        if (Input.GetKeyDown(KeyCode.P))
            Time.timeScale = 0.0f;
        else if (Input.GetKeyDown(KeyCode.O)) Time.timeScale = 1.0f;
    }


    public void DecreaseMana()
    {
        statsContainer.ChangeMana(-30);
    }

    public void DecreaseHealth()
    {
        statsContainer.ChangeHP(-20);
    }

    public void DHealthEffect()
    {
        StatsEffect.AddEffect(target, StatType.HEALTH, -5, 10000);
    }
}