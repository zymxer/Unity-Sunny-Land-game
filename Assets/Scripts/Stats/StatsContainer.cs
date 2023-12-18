using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { MANA, HEALTH};
public class StatsContainer : MonoBehaviour
{
    [SerializeField]
    private float health = 0;
    private float maxHealth;

    [SerializeField]
    private float mana = 0;
    private float maxMana;

    [SerializeField]
    private float healthRestoreCooldown;

    [SerializeField]
    private float healthRestoreSpeed;

    [SerializeField]
    private float manaRestoreCooldown;

    [SerializeField]
    private float manaRestoreSpeed;

    private Timer healthRestoreCDTimer;
    private Timer healthRestoreTimer;

    private Timer manaRestoreCDTimer;
    private Timer manaRestoreTimer;


    private void Awake()
    {
        maxHealth = health;
        maxMana = mana;
    }

    private void Start()
    {

        healthRestoreTimer = gameObject.AddComponent<Timer>();
        healthRestoreTimer.SetTimer(3.0f, true, true);
        healthRestoreTimer.OnValueChanged().AddListener(RestoreHealth);

        manaRestoreTimer = gameObject.AddComponent<Timer>();
        manaRestoreTimer.SetTimer(1.0f, true, true);
        manaRestoreTimer.OnValueChanged().AddListener(RestoreMana);

        healthRestoreCDTimer = gameObject.AddComponent<Timer>();
        healthRestoreCDTimer.SetTimer(healthRestoreCooldown);
        healthRestoreCDTimer.OnStart().AddListener(healthRestoreTimer.Stop);
        healthRestoreCDTimer.OnEnd().AddListener(healthRestoreTimer.Continue);

        manaRestoreCDTimer = gameObject.AddComponent<Timer>();
        manaRestoreCDTimer.SetTimer(manaRestoreCooldown);
        manaRestoreCDTimer.OnStart().AddListener(manaRestoreTimer.Stop);
        manaRestoreCDTimer.OnEnd().AddListener(manaRestoreTimer.Continue);
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public float Mana
    {
        get
        {
            return mana;
        }
        set
        {
            mana = value;
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    public void SetMaxHP()
    {
        health = maxHealth;
    }

    public void SetMinHP()
    {
        health = 0;
    }

    public void SetMaxMana()
    {
        mana = maxMana;
    }

    public void SetMinMana()
    {
        mana = 0;
    }


    public void IncreaseHP(float value)
    {
        health += value;
        health = Mathf.Min(health, maxHealth);
    }

    public void DecreaseHP(float value)
    {
        health -= value;
        health = Mathf.Max(health, 0);
        healthRestoreCDTimer.RestartWithOnStart();
    }

    public void IncreaseMana(float value)
    {
        mana += value;
        mana = Mathf.Min(mana, maxMana);
    }

    public void DecreaseMana(float value)
    {
        mana -= value;
        mana = Mathf.Max(mana, 0);
        manaRestoreCDTimer.RestartWithOnStart();
    }

    public void RestoreHealth()
    {
        if(health < maxHealth)
        {
            IncreaseHP(healthRestoreSpeed * healthRestoreTimer.GetDelta());
        }
        else
        {
            healthRestoreTimer.Stop();
        }
    }

    public void RestoreMana()
    {
        if (mana < maxMana)
        {
            IncreaseMana(manaRestoreSpeed * manaRestoreTimer.GetDelta());
        }
        else
        {
            manaRestoreTimer.Stop();
        }
    }

}
