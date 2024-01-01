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
    private float radiusMultiplyer = 3.5f;

    private Vector3 currentPosition;
    private BoxCollider2D boxCollider;
    private CapsuleCollider2D capsuleCollider;

    private float radius;
    private MouseData mouseData;
    private Transform shotPoint;
    private ParticleSystem.MainModule mainModule;

    private void Start()
    {
        mainModule = GetComponent<ParticleSystem>().main;
        
    }

    public void SetFire(MouseData mouseData, Transform shotPoint, BoxCollider2D playerCollider)
    {
        this.mouseData = mouseData;
        this.shotPoint = shotPoint;
        boxCollider = playerCollider;
        radius = playerCollider.size.x * Mathf.Sqrt(2) / radiusMultiplyer; 
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
