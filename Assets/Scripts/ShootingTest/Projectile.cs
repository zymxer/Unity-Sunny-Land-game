using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Moving moving;

    private float targetAngle = 0.0f;
    private float radians = 0.0f;
    private float speedX = 0.0f;
    private float speedY = 0.0f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        moving = GetComponent<Moving>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0.0f, Space.World);
    }

    public void SetProjectile(float angle)
    {
        targetAngle = angle;
        radians = targetAngle * Mathf.Deg2Rad;
        speedX = moving.Speed * Mathf.Cos(radians);
        speedY = moving.Speed * Mathf.Sin(radians);
    }

    public void StopProjectile()
    {
        speedX = 0.0f;
        speedY = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DestroyTrigger"))
        {
            Destroy(gameObject);
        }
    }
}
