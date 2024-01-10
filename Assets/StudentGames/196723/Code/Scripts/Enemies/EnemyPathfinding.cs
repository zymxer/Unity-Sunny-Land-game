using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace _196723
{
    public class EnemyPathfinding : MonoBehaviour
    {
        [SerializeField]
        private float attackDistance;
        [SerializeField]
        private bool canFly;
        [Range(0.3f, 2.0f)]
        [SerializeField]
        private float pathUpdateInterval;
        [SerializeField]
        private float nextPointDistance;

        [Space]
        [SerializeField]
        private float groundRayLength = 2.0f;
        [SerializeField]
        private Transform groundChecker;
        [SerializeField]
        public LayerMask groundLayer;

        private GameObject player;
        private Transform target;
        private Rigidbody2D rb;

        private Seeker seeker;
        private Path path;
        private Vector2 direction;
        private int currentPoint;
        private bool reachedEnd;

        private bool onAttackDistance;

        private bool active;
        //private 

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            if (!canFly)
            {
                target = player.transform;
            }
            else
            {
                target = player.transform;
            }
        }

        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();

            InvokeRepeating("GeneratePath", 0f, pathUpdateInterval);
        }

        // Update is called once per frame
        void Update()
        {
            if (path != null)
            {
                //GetComponent<EnemyController>().CheckDirection();

                if (ReachedEndCheck())
                {
                    direction = Vector2.zero;
                    if (!AboveTarget() && canFly)
                    {
                        direction.y += 1.0f;
                    }
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
                        {
                            direction.x = Mathf.Sign(direction.x);
                        }
                        else
                        {
                            direction.x = 0.0f;
                        }
                    }
                }

                float distance = Vector2.Distance(path.vectorPath[currentPoint], (Vector2)transform.position);
                if (distance < nextPointDistance)
                {
                    currentPoint++;
                }
            }
        }

        public void GeneratePath()
        {
            if (active)
            {
                if (seeker.IsDone())
                {
                    seeker.StartPath(rb.position, target.position, OnPathGenerationEnd);
                }
            }
        }

        public void Activate()
        {
            active = true;
        }

        public void Disable()
        {
            active = false;
        }

        public bool OnAttackDistance
        {
            get { return onAttackDistance; }
        }

        public GameObject Player
        {
            get { return player; }
        }

        public Vector2 Direction
        {
            get { return direction; }
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
            if (canFly)
            {
                return transform.position.y > (target.position.y + 0.5f);
            }
            return true;
        }

        private bool ReachedEndCheck()
        {
            onAttackDistance = path.GetTotalLength() <= attackDistance;
            reachedEnd = (currentPoint >= path.vectorPath.Count) || (onAttackDistance);
            return reachedEnd;
        }

        private bool isGrounded()
        {
            return Physics2D.Raycast(groundChecker.position, Vector2.down, groundRayLength, groundLayer.value);
        }
    }

}
