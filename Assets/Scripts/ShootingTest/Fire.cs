using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 currentPosition;

    private MouseData mouseData;
    private Transform shotPoint;
    private ParticleSystem.MainModule mainModule;

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
        currentPosition.y += radius * Mathf.Sin(mouseData.Radians());
        transform.position = currentPosition;
    }

    private void OnParticleCollision(GameObject other)
    {
        StatsContainer stats = other.GetComponent<StatsContainer>();
        if (stats != null) 
        {
            StatsEffect.AddEffect(other, StatType.HEALTH, damage, effectDuration);
        }
    }
}
