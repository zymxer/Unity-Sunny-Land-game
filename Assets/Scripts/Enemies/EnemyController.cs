using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType
{
    Melee,
    Range
}

public enum DamageType
{
    Descrete,
    Continuous
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private DamageType damageType;

    [Header("For continuous damage")] [SerializeField]
    private float damageDuration;

    [Space(5)] [SerializeField] private float attackCooldown;

    [SerializeField] private AttackType attackType;

    [Header("For range attack")] [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField] private GameObject shotPoint;

    [Space(5)] [SerializeField] private bool isMovingRight = true;

    [SerializeField] private float moveRange;

    [SerializeField] private int points = 6;

    [Space(5)] [SerializeField] private Slider healthSlider;

    [SerializeField] private Canvas healthCanvas;

    [SerializeField] private bool attackAnim;

    private Animator animator;
    private Timer cooldownTimer;
    private bool dead;

    private int direction;
    private Vector3 healthSliderScale;

    private Moving moving;
    private EnemyPathfinding pathfinding;

    private GameObject player;
    private StatsContainer playerStats;

    private Vector3 scale;
    private float startPositionX;
    private StatsContainer stats;

    private bool triggered;

    private void Awake()
    {
    }

    private void Start()
    {
        moving = GetComponent<Moving>();
        stats = GetComponent<StatsContainer>();
        animator = GetComponent<Animator>();
        pathfinding = GetComponent<EnemyPathfinding>();
        cooldownTimer = gameObject.AddComponent<Timer>();

        player = pathfinding.Player;
        playerStats = player.GetComponent<StatsContainer>();

        cooldownTimer.SetTimer(attackCooldown);

        direction = isMovingRight ? 1 : -1;

        scale = transform.localScale;
        healthSliderScale = healthSlider.transform.localScale;

        if (!isMovingRight) Rotate();

        stats.OnFirstHit.AddListener(TriggerEnemy);
        stats.OnZeroHealth.AddListener(KillEnemy);

        healthCanvas.enabled = false;
        healthSlider.maxValue = stats.GetMaxHealth();
        healthSlider.value = healthSlider.maxValue;

        startPositionX = transform.position.x;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!triggered) //basic movement
        {
            if (moveRange != 0.0f)
            {
                CheckDirection();
                transform.Translate(moving.Speed * Time.deltaTime * direction, 0.0f, 0.0f, Space.World);
            }
        }
        else //pathfinding
        {
            healthSlider.value = stats.Health;
            CheckDirection();
            transform.Translate(moving.Speed * pathfinding.Direction.x * Time.deltaTime,
                moving.Speed * pathfinding.Direction.y * Time.deltaTime, 0f, Space.World);
        }

        if (CanAttack())
        {
            if (attackAnim)
                animator.SetBool("isAttacking", true);
            else
                Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //vision trigger
    {
        if (collision.CompareTag("Player")) TriggerEnemy();
    }

    public int Points()
    {
        return points;
    }

    public void CheckDirection()
    {
        if (!triggered)
        {
            if (isMovingRight)
            {
                if (direction == 1 && transform.position.x >= startPositionX + moveRange)
                {
                    direction = -1;
                    Rotate();
                }
                else if (direction == -1 && transform.position.x <= startPositionX)
                {
                    direction = 1;
                    Rotate();
                }
            }
            else
            {
                if (direction == 1 && transform.position.x >= startPositionX)
                {
                    direction = -1;
                    Rotate();
                }
                else if (direction == -1 && transform.position.x <= startPositionX - moveRange)
                {
                    direction = 1;
                    Rotate();
                }
            }
        }
        else
        {
            if (direction == 1 && pathfinding.Direction.x < 0f)
            {
                direction = -1;
                Rotate();
            }
            else if (direction == -1 && pathfinding.Direction.x > 0f)
            {
                direction = 1;
                Rotate();
            }
            else if (pathfinding.Direction.x == 0.0f)
            {
            }
        }
    }

    private bool CanAttack()
    {
        return !cooldownTimer.IsActive() && pathfinding.OnAttackDistance && pathfinding.AboveTarget() && !dead;
    }

    public void TriggerEnemy()
    {
        if (!triggered)
        {
            pathfinding.Activate();
            triggered = true;
            healthCanvas.enabled = true;
            animator.SetBool("isActive", true);
        }
    }

    private void Attack()
    {
        if (transform.position.x > player.transform.position.x
            && transform.localScale.x > 0.0f)
        {
            direction = -1;
            Rotate();
        }
        else if (transform.position.x < player.transform.position.x
                 && transform.localScale.x < 0.0f)
        {
            direction = 1;
            Rotate();
        }

        if (attackType == AttackType.Melee)
        {
            if (damageType == DamageType.Descrete)
                playerStats.ChangeHP(-damage);
            else if (damageType == DamageType.Continuous)
                StatsEffect.AddEffect(player, StatType.HEALTH, -damage, damageDuration);
        }
        else if (attackType == AttackType.Range)
        {
            GameObject projectile;
            var shotPosition = shotPoint.transform.position;
            var angle = CalculateAngle();

            projectile = Instantiate(projectilePrefab, shotPosition, Quaternion.identity);
            projectile.transform.eulerAngles = new Vector3(0f, 0f, angle);
            projectile.GetComponent<Projectile>().SetProjectile(angle);
        }

        cooldownTimer.Activate();
    }

    private void KillEnemy()
    {
        if (!dead)
        {
            dead = true;
            GetComponent<Collider2D>().enabled = false;
            animator.SetBool("isDead", true);
            healthCanvas.enabled = false;
            moving.Speed = 0.0f;
            StartCoroutine(KillOnAnimationEnd());
            GameManager.instance.IncreaseEnemiesKilled();
            GameManager.instance.IncreaseScore(points);
            if (gameObject.GetComponent<AudioSource>() != null) gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
        Destroy(gameObject, 5.0f);
    }

    private void Rotate()
    {
        scale.x *= -1;
        transform.localScale = scale;

        healthSliderScale.x *= -1;
        healthSlider.transform.localScale = healthSliderScale;
    }

    private float CalculateAngle()
    {
        float xDistance, yDistance, distance, cosin, radians, angle;

        xDistance = shotPoint.transform.position.x - player.transform.position.x;
        yDistance = shotPoint.transform.position.y - player.transform.position.y;
        distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

        cosin = xDistance / distance;
        radians = Mathf.Acos(cosin);
        angle = radians * Mathf.Rad2Deg;

        if (yDistance < 0) angle *= -1;

        angle -= 180.0f;

        return angle;
    }

    public void OnAttackEnd()
    {
        animator.SetBool("isAttacking", false);
    }

    public void AttackOnAnim()
    {
        if (CanAttack()) Attack();
    }
}