using Pathfinding;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float attackDistance;

    [SerializeField] private bool canFly;

    [Range(0.3f, 2.0f)] [SerializeField] private float pathUpdateInterval;

    [SerializeField] private float nextPointDistance;

    [Space] [SerializeField] private float groundRayLength = 2.0f;

    [SerializeField] private Transform groundChecker;

    [SerializeField] public LayerMask groundLayer;

    private bool active;
    private int currentPoint;
    private Vector2 direction;

    private Path path;

    private Rigidbody2D rb;
    private bool reachedEnd;

    private Seeker seeker;
    private Transform target;

    public bool OnAttackDistance { get; private set; }

    public GameObject Player { get; private set; }

    public Vector2 Direction => direction;
    //private 

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        if (!canFly)
            target = Player.transform;
        else
            target = Player.transform;
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("GeneratePath", 0f, pathUpdateInterval);
    }

    // Update is called once per frame
    private void Update()
    {
        if (path != null)
        {
            //GetComponent<EnemyController>().CheckDirection();

            if (ReachedEndCheck())
            {
                direction = Vector2.zero;
                if (!AboveTarget() && canFly) direction.y += 1.0f;
                return;
            }

            direction = ((Vector2)path.vectorPath[currentPoint] - (Vector2)transform.position).normalized;


            if (!canFly)
            {
                if (!isGrounded())
                {
                    GetComponent<EnemyController>().CheckDirection();
                    direction = Vector2.zero;
                }
                else
                {
                    direction.y = 0.0f;
                    if (Mathf.Abs(direction.x) >= 0.05f)
                        direction.x = Mathf.Sign(direction.x);
                    else
                        direction.x = 0.0f;
                }
            }

            var distance = Vector2.Distance(path.vectorPath[currentPoint], transform.position);
            if (distance < nextPointDistance) currentPoint++;
        }
    }

    public void GeneratePath()
    {
        if (active)
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.position, OnPathGenerationEnd);
    }

    public void Activate()
    {
        active = true;
    }

    public void Disable()
    {
        active = false;
    }

    private void OnPathGenerationEnd(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentPoint = 0;
        }
    }

    public bool AboveTarget()
    {
        if (canFly) return transform.position.y > target.position.y + 0.5f;
        return true;
    }

    private bool ReachedEndCheck()
    {
        OnAttackDistance = path.GetTotalLength() <= attackDistance;
        reachedEnd = currentPoint >= path.vectorPath.Count || OnAttackDistance;
        return reachedEnd;
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(groundChecker.position, Vector2.down, groundRayLength, groundLayer.value);
    }
}