using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float effectDuration;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float radius = 3.5f;
    [SerializeField]
    private GameObject fireParticles;

    private Vector3 currentPosition;

    private MouseData mouseData;
    private Transform shotPoint;
    private ParticleSystem.MainModule mainModule;
    private List<GameObject> affectedObjects = new List<GameObject>();
    private List<Timer> affectedObjectsTimers = new List<Timer>();
    private ParticleSystem createdSystem;


    private void Start()
    {
        mainModule = GetComponent<ParticleSystem>().main;
        mainModule.startLifetime = maxDistance / mainModule.startSpeed.constant;
    }

    public void SetFire(MouseData mouseData, Transform shotPoint)
    {
        this.mouseData = mouseData;
        this.shotPoint = shotPoint;
    }

    private void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, mouseData.Angle());
        currentPosition = shotPoint.position;
        currentPosition.x += radius * Mathf.Cos(mouseData.Radians());
        currentPosition.y += radius * Mathf.Sin(mouseData.Radians()) * Mathf.Sign(mouseData.Angle());
        transform.position = currentPosition;
    }

    private void OnParticleCollision(GameObject other)
    {
        StatsContainer stats = other.GetComponent<StatsContainer>();
        if (stats != null) 
        {
            if(!affectedObjects.Contains(other))
            {
                affectedObjects.Add(other);
                StatsEffect.AddEffect(other, StatType.HEALTH, -damage, effectDuration);
                Timer effectTimer = gameObject.AddComponent<Timer>();
                effectTimer.SetTimer(effectDuration);
                effectTimer.OnEnd().AddListener(OnAffectedTimerEnd);
                effectTimer.Activate();
                affectedObjectsTimers.Add(effectTimer);
                AddFireEffect(other);
            }
        }
    }

    private void OnAffectedTimerEnd()
    {
        for (int i = affectedObjects.Count - 1; i >= 0; i--)
        {
            if (affectedObjectsTimers[i].GetValue() <= 0.0f)
            {
                Timer timer = affectedObjectsTimers[i];
                affectedObjectsTimers[i].Remove();
                affectedObjects.RemoveAt(i);
                affectedObjectsTimers.RemoveAt(i);
                Destroy(timer);
            }
        }
    }

    private void AddFireEffect(GameObject target)
    {
        GameObject fireEffect = Instantiate(fireParticles, target.transform.position, Quaternion.identity);
        createdSystem = fireEffect.GetComponent<ParticleSystem>();
        createdSystem.Stop();
        ParticleSystem.ShapeModule shapeModule = createdSystem.shape;
        ParticleSystem.MainModule mainModule = createdSystem.main;
        mainModule.duration = effectDuration + mainModule.startLifetime.constant;
        fireEffect.transform.parent = target.transform;
        shapeModule.scale = Vector3.Scale(target.GetComponent<Renderer>().bounds.size, target.transform.localScale);
        createdSystem.Play();
    }
}
