using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float maxDistance;

    [SerializeField] private float effectDuration;

    [SerializeField] private float damage;

    [SerializeField] private float radius = 3.5f;

    [SerializeField] private GameObject fireParticles;

    private readonly List<GameObject> affectedObjects = new();
    private readonly List<Timer> affectedObjectsTimers = new();
    private ParticleSystem createdSystem;

    private Vector3 currentPosition;
    private ParticleSystem.MainModule mainModule;

    private MouseData mouseData;
    private Transform shotPoint;


    private void Start()
    {
        mainModule = GetComponent<ParticleSystem>().main;
        mainModule.startLifetime = maxDistance / mainModule.startSpeed.constant;
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
        var stats = other.GetComponent<StatsContainer>();
        if (stats != null)
            if (!affectedObjects.Contains(other))
            {
                affectedObjects.Add(other);
                StatsEffect.AddEffect(other, StatType.HEALTH, -damage, effectDuration);
                var effectTimer = gameObject.AddComponent<Timer>();
                effectTimer.SetTimer(effectDuration);
                effectTimer.OnEnd().AddListener(OnAffectedTimerEnd);
                effectTimer.Activate();
                affectedObjectsTimers.Add(effectTimer);
                AddFireEffect(other);
            }
    }

    public void SetFire(MouseData mouseData, Transform shotPoint)
    {
        this.mouseData = mouseData;
        this.shotPoint = shotPoint;
    }

    private void OnAffectedTimerEnd()
    {
        for (var i = affectedObjects.Count - 1; i >= 0; i--)
            if (affectedObjectsTimers[i].GetValue() <= 0.0f)
            {
                var timer = affectedObjectsTimers[i];
                affectedObjectsTimers[i].Remove();
                affectedObjects.RemoveAt(i);
                affectedObjectsTimers.RemoveAt(i);
                Destroy(timer);
            }
    }

    private void AddFireEffect(GameObject target)
    {
        var fireEffect = Instantiate(fireParticles, target.transform.position, Quaternion.identity);
        createdSystem = fireEffect.GetComponent<ParticleSystem>();
        createdSystem.Stop();
        var shapeModule = createdSystem.shape;
        var mainModule = createdSystem.main;
        mainModule.duration = effectDuration + mainModule.startLifetime.constant;
        fireEffect.transform.parent = target.transform;
        //shapeModule.scale = Vector3.Scale(target.GetComponent<Renderer>().bounds.size, target.transform.localScale);
        var newScale = new Vector3(target.GetComponent<Collider2D>().bounds.size.x, 1.0f,
            target.GetComponent<Collider2D>().bounds.size.y);
        shapeModule.scale = newScale;
        createdSystem.Play();
    }
}