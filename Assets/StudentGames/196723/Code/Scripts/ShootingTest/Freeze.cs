using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _196723
{
    public class Freeze : MonoBehaviour
    {
        [SerializeField]
        private float slowRatio = 2.0f;
        [SerializeField]
        private Color color; //for test

        [SerializeField]
        private float duration = 1.0f;

        [SerializeField]
        private bool affectsPlayer;

        [SerializeField]
        private GameObject snowParticles;

        private ParticleSystem createdSystem;
        private GameObject target;
        private Timer timer;

        private int firstHit = 0;
        private Color ogColor;

        private static ArrayList affectedObjects = new ArrayList();

        private static Color effectColor;
        private static bool colorSet = false;
        // Start is called before the first frame update
        private void Awake()
        {
            if (colorSet == false)
            {
                effectColor = color;
                colorSet = true;
            }
        }

        void Start()
        {
            effectColor = color;
            timer = gameObject.AddComponent<Timer>();
            timer.SetTimer(duration);
            timer.OnStart().AddListener(OnTimerStart);
            timer.OnEnd().AddListener(OnTimerEnd);
            GetComponent<ParticleSystem>().Emit(1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                if (collision.CompareTag("World"))
                {
                    Destroy(gameObject);
                }


                if (affectsPlayer)
                {
                    if (!collision.CompareTag("Enemy") || (collision.CompareTag("Enemy") && firstHit != 0))
                    {
                        AddFreezeEffect(collision);
                    }
                    else
                    {
                        firstHit++;
                    }
                }
                else
                {
                    if (!collision.CompareTag("Player") && !collision.CompareTag("World"))
                    {
                        AddFreezeEffect(collision);
                    }
                }
            }
        }

        private void AddFreezeEffect(Collider2D collision)
        {
            target = collision.gameObject;
            if (!affectedObjects.Contains(target))
            {
                gameObject.SetActive(false);
                GameObject snowEffect = Instantiate(snowParticles, target.transform.position, Quaternion.identity);
                createdSystem = snowEffect.GetComponent<ParticleSystem>();
                createdSystem.Stop();
                ParticleSystem.ShapeModule shapeModule = createdSystem.shape;
                ParticleSystem.MainModule mainModule = createdSystem.main;
                mainModule.duration = duration + mainModule.startLifetime.constant;
                snowEffect.transform.parent = target.transform;
                Vector3 newScale = new Vector3(target.GetComponent<Collider2D>().bounds.size.x, 1.0f, target.GetComponent<Collider2D>().bounds.size.y);
                shapeModule.scale = newScale;
                createdSystem.Play();

                affectedObjects.Add(target);
                timer.Activate();

            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTimerStart()
        {
            SlowDownTimers();
            SlowDownObject();
        }

        private void OnTimerEnd()
        {
            if (affectedObjects.Contains(target))
            {
                affectedObjects.Remove(target);
                SpeedUpObject();
                SpeedUpTimers();
            }
            if (createdSystem != null)
            {
                createdSystem.Stop();
            }
            timer.Remove();
            Destroy(gameObject, 2.0f);
        }

        private void SlowDownTimers()
        {
            if (target != null)
            {
                Timer[] timers = target.GetComponents<Timer>();
                foreach (Timer timer in timers)
                {
                    timer.SlowDown(slowRatio);
                }
            }
        }

        private void SpeedUpTimers()
        {
            if (target != null)
            {
                Timer[] timers = target.GetComponents<Timer>();
                foreach (Timer timer in timers)
                {
                    timer.SpeedUp(slowRatio);
                }
            }
        }

        private void SlowDownObject()
        {
            if (target != null)
            {
                Moving moving = target.GetComponent<Moving>();
                if (moving != null)
                {
                    moving.SlowDown(slowRatio);
                }
                if (target.GetComponent<SpriteRenderer>() != null)
                {
                    ogColor = target.GetComponent<SpriteRenderer>().color;
                    target.GetComponent<SpriteRenderer>().color = color;
                }

                if (target.GetComponent<Animator>() != null)
                {
                    target.GetComponent<Animator>().speed /= slowRatio;
                }
            }
        }

        public void SpeedUpObject()
        {
            if (target != null)
            {
                if (target.GetComponent<SpriteRenderer>() != null)
                {
                    target.GetComponent<SpriteRenderer>().color = ogColor;
                }
                Moving moving = target.GetComponent<Moving>();
                if (moving != null)
                {
                    moving.SpeedUp(slowRatio);
                }
                if (target.GetComponent<Animator>() != null)
                {
                    target.GetComponent<Animator>().speed *= slowRatio;
                }
            }
        }

        public static void SpeedUpPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (affectedObjects.Contains(player))
            {
                affectedObjects.Remove(player);
                if (player.GetComponent<SpriteRenderer>().color == effectColor)
                {
                    Debug.Log("here");
                    Timer[] timers = player.GetComponents<Timer>();
                    foreach (Timer timer in timers)
                    {
                        timer.SpeedUp(2.0f);
                    }

                    if (player.GetComponent<SpriteRenderer>() != null)
                    {
                        player.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    Moving moving = player.GetComponent<Moving>();
                    if (moving != null)
                    {
                        moving.SpeedUp(2.0f);
                    }
                    if (player.GetComponent<Animator>() != null)
                    {
                        player.GetComponent<Animator>().speed *= 2.0f;
                    }
                }
            }
        }

    }
}
