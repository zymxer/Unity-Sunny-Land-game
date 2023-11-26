using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingPlatform : MonoBehaviour
{
    private enum MoveMode { Horizontal, Vertical, Trajectory };

    [Header("< 10 for Horizontal/Vertical, > 50 for Trajectory")]
    [SerializeField]
    private float speed = 0.0f;
    [Header("True for Right/Up")]
    [SerializeField]
    private bool startDirection = true;

    [Space]
    [SerializeField]
    private MoveMode moveMode;

    [Header("For Horizontal mode")]
    [SerializeField]
    private float xMargin = 0.0f;

    [Header("For Vertical mode")]
    [SerializeField]
    private float yMargin = 0.0f;

    [Header("For Trajectory mode")]
    [SerializeField]
    private bool cycled;

    [SerializeField]
    private Transform[] waypoints = new Transform[0];

    private Vector2[] points;

    private Vector3 startPosition;
    private Transform _transform;
    private int direction;
    private float localTimeScale = 1.0f;

    private int pointIndex = 0;
    private float nextX = 0.0f;
    private float nextY = 0.0f;

    private Vector2 prevPosition;
    private float deltaX = 0.0f;
    private float deltaY = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector2[waypoints.Length]; 
        for(int i = 0; i < waypoints.Length; i++)
        {
            points[i] = waypoints[i].position;
        }
        startPosition = transform.position;
        _transform = GetComponent<Transform>();
        direction = startDirection ? 1 : -1;
        if(moveMode == MoveMode.Trajectory)
        {
            transform.position = new Vector3(points[0].x, points[0].y, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        prevPosition.x = transform.position.x;
        prevPosition.y = transform.position.y;
        switch (moveMode)
        {
            case MoveMode.Horizontal:
                transform.Translate(speed * Time.deltaTime * direction * localTimeScale, 0.0f, 0.0f, Space.World);
                break;
            case MoveMode.Vertical:
                transform.Translate(0.0f, speed * Time.deltaTime * direction * localTimeScale, 0.0f, Space.World);
                break;
            case MoveMode.Trajectory:
                transform.position = Vector2.MoveTowards(transform.position, points[pointIndex], speed * Time.deltaTime * localTimeScale);
                if(CheckPointIndex())
                {
                    UpdatePointIndex();
                }
                break;
            default: break;

        }
        UpdateDeltas();
        CheckDirection();
    }

    private bool CheckPointIndex()
    {
        return Mathf.Abs(transform.position.x - points[pointIndex].x) < 0.01f && Mathf.Abs(transform.position.y - points[pointIndex].y) < 0.01f;
    }

    private void CheckDirection()
    {
        switch (moveMode)
        {
            case MoveMode.Horizontal:
                if(startDirection)
                {
                    if (direction == 1 && transform.position.x >= startPosition.x + xMargin)
                    {
                        direction = -1;
                    }
                    else if (direction == -1 && transform.position.x <= startPosition.x)
                    {
                        direction = 1;
                    }
                }
                else
                {
                    if (direction == 1 && transform.position.x >= startPosition.x)
                    {
                        direction = -1;
                    }
                    else if (direction == -1 && transform.position.x <= startPosition.x - xMargin)
                    {
                        direction = 1;
                    }
                }
                break;
            case MoveMode.Vertical:
                if (direction == 1 && transform.position.y >= startPosition.y + yMargin)
                {
                    direction = -1;
                }
                else if (direction == -1 && transform.position.y <= startPosition.y)
                {
                    direction = 1;
                }
                break;
            default: break;

        }
    }

    private void UpdatePointIndex()
    {
        if(direction == 1 && pointIndex == points.Length - 1)
        {
            if(cycled)
            {
                pointIndex = 0;
                return;
            }
            direction = -1;
        }
        else if (direction == -1 && pointIndex == 0)
        {
            if (cycled)
            {
                pointIndex = points.Length - 1;
                return;
            }
            direction = 1;
        }
        pointIndex += direction;
    }

    private void UpdateDeltas()
    {
        deltaX = transform.position.x - prevPosition.x;
        deltaY = transform.position.y - prevPosition.y;
    }

    public float SpeedX()
    {
        return speed * direction;
    }
    public float SpeedY()
    {
        //return _speed * _direction;
        return DeltaY() / Time.deltaTime;
    }

    public void SlowDown(float ratio)
    {
        localTimeScale /= ratio;
    }

    public void SpeedUp(float ratio)
    {
        localTimeScale *= ratio;
    }

    public float DeltaX()
    {
        return deltaX;
    }
    public float DeltaY()
    {
        return deltaY;
    }
}
