using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellsController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spellsObjects = new GameObject[0];
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private MouseData mouseData;
    [SerializeField]
    private Transform shotPoint;

    public Spell[] spells;
    public StatsContainer playerStats;
    public Spell selectedSpell;
    public int selectedSpellIndex;
    public Timer[] cooldowns;
    public Vector3 shotPosition;

    public static SpellsController instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            spells = new Spell[spellsObjects.Length];
            for (int i = 0; i < spellsObjects.Length; i++)
            {
                spells[i] = spellsObjects[i].GetComponent<Spell>();

            }
            selectedSpell = spells[0];
            playerStats = player.GetComponent<StatsContainer>();
            cooldowns = new Timer[spells.Length];
            for (int i = 0; i < cooldowns.Length; i++)
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
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ScrollSpellDown();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            if(CanCast())
            {
                if (selectedSpell.Type == SpellType.Firework || selectedSpell.Type == SpellType.Freeze)
                {
                    GameObject projectilePrefab = selectedSpell.gameObject, projectile;
                    shotPosition = shotPoint.transform.position;

                    projectile = Instantiate(projectilePrefab, shotPosition, Quaternion.identity);
                    projectile.transform.eulerAngles = new Vector3(0f, 0f, mouseData.Angle());
                    projectile.GetComponent<Projectile>().SetProjectile(mouseData.Angle());
                    projectile.GetComponent<Spell>().SetSpell(this, player, playerStats, selectedSpellIndex);
                    projectile.GetComponent<Spell>().CastSpell();
                    
                }   
            }
        }
    }

    public int GetSpellIndex(Spell spell)
    {
        for(int i = 0; i < spells.Length; i++)
        {
            if(spell == spells[i])
            {
                return i;
            }
        }
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

    public GameObject GetPlayer()
    {
        return player;
    }

    public bool CanCast()
    {
        return (playerStats.Mana >= selectedSpell.Cost && !InCooldown(selectedSpellIndex));
    }

    private void ScrollSpellUp()
    {
        selectedSpellIndex++;
        if (selectedSpellIndex >= spells.Length)
        {
            selectedSpellIndex = 0;
        }
        selectedSpell = spells[selectedSpellIndex];
    }

    private void ScrollSpellDown()
    {
        selectedSpellIndex--;
        if(selectedSpellIndex < 0)
        {
            selectedSpellIndex = spells.Length - 1;
        }
        selectedSpell = spells[selectedSpellIndex];
    }
}
