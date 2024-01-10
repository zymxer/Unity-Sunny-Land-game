using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;


namespace _196723
{
    public class Firework : MonoBehaviour
    {
        [SerializeField]
        private float duration;
        [SerializeField]
        private float burstDuration;
        [SerializeField]
        private float radiusIncSpeed;
        [SerializeField]
        private float damage;
        [SerializeField]
        private bool affectsPlayer;
        [SerializeField]
        private AudioClip burstAudio;

        private ParticleSystem projectile;
        private ParticleSystem burst;

        private Timer projectileTimer;
        private Timer burstTimer;

        private CircleCollider2D fireworkCollider;

        // Start is called before the first frame update
        void Awake()
        {
            projectileTimer = gameObject.AddComponent<Timer>();
            projectileTimer.SetTimer(duration);
            projectileTimer.OnEnd().AddListener(StopFirework);
            projectileTimer.Activate();

            projectile = GetComponent<ParticleSystem>();
            burst = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

            projectile.Stop();
            var main = projectile.main;
            main.duration = duration;
            main.startLifetime = duration;
            projectile.Play();

            projectile.Emit(1);

            burstTimer = gameObject.AddComponent<Timer>();
            burstTimer.SetTimer(burstDuration);
            burstTimer.OnValueChanged().AddListener(OnBurst);
            burstTimer.OnEnd().AddListener(OnBurstEnd);

            burst.Stop();
            main = burst.main;
            main.duration = burstDuration;
            ParticleSystem.MinMaxCurve burstLife = new ParticleSystem.MinMaxCurve(burstDuration - 0.3f, burstDuration);
            burstLife.mode = ParticleSystemCurveMode.TwoConstants;
            main.startLifetime = burstLife;
            main.startSpeed = radiusIncSpeed;

            fireworkCollider = GetComponent<CircleCollider2D>();

        }

        private void StopFirework()
        {
            GetComponent<Projectile>().StopProjectile();
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = burstAudio;
            GetComponent<AudioSource>().Play();

            Destroy(projectile);
            burst.Play();
            burstTimer.Activate();
            projectileTimer.Remove();
        }

        private void OnBurst()
        {
            fireworkCollider.radius += radiusIncSpeed * burstTimer.GetDelta();
        }

        private void OnBurstEnd()
        {
            burstTimer.Remove();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                if ((!collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Player") && affectsPlayer))
                    && !collision.gameObject.CompareTag("Projectile"))
                {
                    StatsContainer stats = collision.gameObject.GetComponent<StatsContainer>();
                    if (projectileTimer.IsActive())
                    {
                        projectileTimer.End();
                        if (stats != null)
                        {
                            stats.ChangeHP(-damage);
                        }
                    }
                    else
                    {
                        if (stats != null)
                        {
                            stats.ChangeHP(-damage);
                        }
                    }
                }
            }
        }
    }
}