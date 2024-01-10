using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _196723
{
    public enum StatType { MANA, HEALTH };
    public class StatsContainer : MonoBehaviour
    {
        [SerializeField]
        private float health = 0;
        private float maxHealth;

        [SerializeField]
        private float mana = 0;
        private float maxMana;

        [Space(10)]
        [SerializeField]
        private bool regeterates = false;

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

        private UnityEvent onZeroHealth = new UnityEvent();
        private UnityEvent onFirstHit = new UnityEvent();
        private bool firstHit = false;

        private void Awake()
        {
            maxHealth = health;
            maxMana = mana;
        }

        private void Start()
        {
            if (regeterates)
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

        public void SetMaxHP(float value)
        {
            maxHealth = value;
        }

        public void SetMaxMana(float value)
        {
            maxMana = value;
        }

        public void SetHPToMax()
        {
            health = maxHealth;
        }

        public void SetManaToMax()
        {
            mana = maxMana;
        }

        public void SetHPToMin()
        {
            health = 0;
        }

        public void SetManaToMin()
        {
            mana = 0;
        }

        public void HealAll()
        {
            health = maxHealth;
            mana = maxMana;
        }


        public UnityEvent OnFirstHit
        {
            get { return onFirstHit; }
        }

        public UnityEvent OnZeroHealth
        {
            get { return onZeroHealth; }
        }

        public void ChangeHP(float value)
        {
            health += value;
            if (value < 0.0f)
            {
                health = Mathf.Max(health, 0.0f);
                if (regeterates)
                {
                    healthRestoreCDTimer.RestartWithOnStart();
                }
                if (!firstHit)
                {
                    firstHit = true;
                    onFirstHit.Invoke();
                }
                if (health == 0.0f)
                {
                    onZeroHealth.Invoke();
                }
            }
            else
            {
                health = Mathf.Min(health, maxHealth);
            }
        }

        public void ChangeMana(float value)
        {
            mana += value;
            if (value < 0.0f)
            {
                mana = Mathf.Max(mana, 0);
                if (regeterates)
                {
                    manaRestoreCDTimer.RestartWithOnStart();
                }
            }
            else
            {
                mana = Mathf.Min(mana, maxMana);
            }
        }

        public void RestoreHealth()
        {
            if (health < maxHealth)
            {
                ChangeHP(healthRestoreSpeed * healthRestoreTimer.GetDelta());
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
                ChangeMana(manaRestoreSpeed * manaRestoreTimer.GetDelta());
            }
            else
            {
                manaRestoreTimer.Stop();
            }
        }

    }
}
