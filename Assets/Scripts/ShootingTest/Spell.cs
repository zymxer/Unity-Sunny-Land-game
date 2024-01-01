using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType { Firework, Freeze, Fire, Sword, Knot};
public class Spell : MonoBehaviour
{
    [SerializeField]
    private float cost;
    [SerializeField]
    private bool continuous;
    [SerializeField]
    private float cooldown;
    private SpellsController controller;
    [SerializeField]
    private SpellType type;

    private GameObject player;
    private StatsContainer playerStats;
    private StatsEffect effect;
    private int spellIndex;
    private bool active;

    public void SetSpell(SpellsController controller, GameObject player, StatsContainer playerStats, int index)
    {
        this.controller = controller;
        this.player = player;
        this.playerStats = playerStats;
        spellIndex = index;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public SpellType Type
    { 
        get { return type; } 
    }

    public float Cost
    { 
        get { return cost; } 
    }

    public int Index
    {
        get { return spellIndex; }
    }

    public bool Continuous
    {
        get { return continuous; }
    }

    public void CastSpell()
    {
        if(continuous)
        {
            effect = StatsEffect.AddEffect(player, StatType.MANA, -cost, 10000);
        }
        else
        {
            playerStats.ChangeMana(-cost);
            controller.StartCooldown(spellIndex);
        }

    }

    public void StopSpell()
    {
        if(continuous)
        {
            effect.StopEffect();
            controller.StartCooldown(spellIndex);
            gameObject.GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject, 4.0f);
        }
    }
}
