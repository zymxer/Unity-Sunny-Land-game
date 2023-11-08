using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;

    private float targetAngle = 0.0f;
    private float radians = 0.0f;
    private float speedX = 0.0f;
    private float speedY = 0.0f;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0.0f, Space.World);
    }

    public void SetAxisSpeed(float angle)
    {
        targetAngle = angle;
        radians = targetAngle * Mathf.Deg2Rad;
        speedX = speed * Mathf.Cos(radians);
        speedY = speed * Mathf.Sin(radians);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DestroyTrigger") || collision.gameObject.CompareTag("World"))
        {
            Destroy(gameObject);
        }
    }
}
