using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private float targetAngle = 0.0f;

    private float radians;
    private float speedX = 0.0f;
    private float speedY = 0.0f;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        timer.OnEnd().AddListener(OnTimerEnd);
        _rigidbody = GetComponent<Rigidbody2D>();
        //_rigidbody.AddForce(transform.rotation.eulerAngles);
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


    private void OnTimerEnd()
    {
        timer.Remove();
        Destroy(gameObject);
    }
}
