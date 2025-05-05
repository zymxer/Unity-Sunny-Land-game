using UnityEngine;

public class SpellsController : MonoBehaviour
{
    public static SpellsController instance;

    [SerializeField] private GameObject[] spellsObjects = new GameObject[0];

    [SerializeField] private GameObject player;

    [SerializeField] private MouseData mouseData;

    [SerializeField] private Transform shotPoint;

    private Spell continuousSpell;
    private Timer[] cooldowns;
    private StatsContainer playerStats;
    private Spell selectedSpell;
    private Vector3 shotPosition;

    private Spell[] spells;

    public int SelectedSpellIndex { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            spells = new Spell[spellsObjects.Length];
            for (var i = 0; i < spellsObjects.Length; i++) spells[i] = spellsObjects[i].GetComponent<Spell>();
            selectedSpell = spells[0];
            playerStats = player.GetComponent<StatsContainer>();
            cooldowns = new Timer[spells.Length];
            for (var i = 0; i < cooldowns.Length; i++)
            {
                cooldowns[i] = gameObject.AddComponent<Timer>();
                cooldowns[i].SetTimer(spells[i].GetCooldown());
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ScrollSpellUp();
            GameplayUI.instance.UpdateSpellImages();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ScrollSpellDown();
            GameplayUI.instance.UpdateSpellImages();
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C))
        {
            if (CanCast())
            {
                if (selectedSpell.Type == SpellType.Firework || selectedSpell.Type == SpellType.Freeze)
                {
                    GameObject projectilePrefab = selectedSpell.gameObject, projectile;
                    shotPosition = shotPoint.transform.position;

                    projectile = Instantiate(projectilePrefab, shotPosition, Quaternion.identity);
                    projectile.transform.eulerAngles = new Vector3(0f, 0f, mouseData.Angle());
                    projectile.GetComponent<Projectile>().SetProjectile(mouseData.Angle());
                    projectile.GetComponent<Spell>().SetSpell(this, player, playerStats, SelectedSpellIndex);
                    projectile.GetComponent<Spell>().CastSpell();
                }
                else if (selectedSpell.Type == SpellType.Fire)
                {
                    GameObject firePrefab = selectedSpell.gameObject, fire;
                    shotPosition = shotPoint.transform.position;

                    fire = Instantiate(firePrefab, shotPosition, Quaternion.identity);
                    fire.GetComponent<Fire>().SetFire(mouseData, shotPoint);
                    fire.GetComponent<Spell>().SetSpell(this, player, playerStats, SelectedSpellIndex);
                    fire.GetComponent<Spell>().CastSpell();
                    continuousSpell = fire.GetComponent<Spell>();
                }
            }
        }

        else if (Input.GetMouseButtonUp(0) || playerStats.Mana == 0.0f || Input.GetKeyUp(KeyCode.C))
        {
            StopContinuousSpell();
        }
    }

    public Timer GetCooldownTimer(int i)
    {
        return cooldowns[i];
    }

    public void StopContinuousSpell()
    {
        if (continuousSpell != null)
        {
            continuousSpell.gameObject.GetComponent<AudioSource>().Stop();
            continuousSpell.StopSpell();
            continuousSpell = null;
        }
    }

    public int GetSpellIndex(Spell spell)
    {
        for (var i = 0; i < spells.Length; i++)
            if (spell == spells[i])
                return i;
        return -1;
    }

    public bool InCooldown(int i)
    {
        return cooldowns[i].IsActive();
    }

    public void StartCooldown(int i)
    {
        cooldowns[i].Activate();
    }

    public void ResetCooldowns()
    {
        for (var i = 0; i < cooldowns.Length; i++)
            if (cooldowns[i].IsActive())
                cooldowns[i].SetValue(0.0f);
        //cooldowns[i].ResetTimer();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public bool CanCast()
    {
        if (!selectedSpell.Continuous) return playerStats.Mana >= selectedSpell.Cost && !InCooldown(SelectedSpellIndex);
        return !InCooldown(SelectedSpellIndex);
    }

    private void ScrollSpellUp()
    {
        SelectedSpellIndex++;
        if (SelectedSpellIndex >= spells.Length) SelectedSpellIndex = 0;
        selectedSpell = spells[SelectedSpellIndex];
    }

    private void ScrollSpellDown()
    {
        SelectedSpellIndex--;
        if (SelectedSpellIndex < 0) SelectedSpellIndex = spells.Length - 1;
        selectedSpell = spells[SelectedSpellIndex];
    }
}