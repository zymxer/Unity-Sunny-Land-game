using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Moving moving;
    private float radians;
    private float speedX;
    private float speedY;

    private float targetAngle;

    private void Awake()
    {
        moving = GetComponent<Moving>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0.0f, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DestroyTrigger")) Destroy(gameObject);
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
}