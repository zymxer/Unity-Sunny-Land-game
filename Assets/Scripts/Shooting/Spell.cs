using UnityEngine;

public enum SpellType
{
    Firework,
    Freeze,
    Fire,
    Sword,
    Knot
}

public class Spell : MonoBehaviour
{
    [SerializeField] private float cost;

    [SerializeField] private bool continuous;

    [SerializeField] private float cooldown;

    [SerializeField] private SpellType type;

    private bool active;
    private SpellsController controller;
    private StatsEffect effect;

    private GameObject player;
    private StatsContainer playerStats;

    public SpellType Type => type;

    public float Cost => cost;

    public int Index { get; private set; }

    public bool Continuous => continuous;

    public void SetSpell(SpellsController controller, GameObject player, StatsContainer playerStats, int index)
    {
        this.controller = controller;
        this.player = player;
        this.playerStats = playerStats;
        Index = index;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public void CastSpell()
    {
        if (continuous)
        {
            effect = StatsEffect.AddEffect(player, StatType.MANA, -cost, 10000);
        }
        else
        {
            playerStats.ChangeMana(-cost);
            controller.StartCooldown(Index);
        }
    }

    public void StopSpell()
    {
        if (continuous)
        {
            effect.StopEffect();
            controller.StartCooldown(Index);
            gameObject.GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject, 4.0f);
        }
    }
}