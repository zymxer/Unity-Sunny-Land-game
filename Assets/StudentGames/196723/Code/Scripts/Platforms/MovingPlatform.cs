using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingPlatform : MonoBehaviour
{
    private enum MoveMode { Horizontal, Vertical, Trajectory };

    private Moving moving;

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

    private int pointIndex = 0;
    private float nextX = 0.0f;
    private float nextY = 0.0f;

    private Vector2 prevPosition;
    private float deltaX = 0.0f;
    private float deltaY = 0.0f;
   
    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponent<Moving>();
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
                transform.Translate(moving.Speed * Time.deltaTime * direction, 0.0f, 0.0f, Space.World);
                break;
            case MoveMode.Vertical:
                transform.Translate(0.0f, moving.Speed * Time.deltaTime * direction, 0.0f, Space.World);
                break;
            case MoveMode.Trajectory:

                float distanceToPoint = DistanceToPoint();
                if (distanceToPoint >= 1.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, points[pointIndex], moving.Speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, points[pointIndex], moving.Speed * Time.deltaTime * distanceToPoint / 1.5f);
                }
                if(CheckPointIndex())
                {
                    UpdatePointIndex();
                }
                break;
            default: break;

        }
        CheckDirection();
    }

    private bool CheckPointIndex()
    {
        return Mathf.Abs(transform.position.x - points[pointIndex].x) < 0.01f && Mathf.Abs(transform.position.y - points[pointIndex].y) < 0.01f;
    }

    private float DistanceToPoint()
    {
        float distX = transform.position.x - points[pointIndex].x;
        float distY = transform.position.y - points[pointIndex].y;
        return Mathf.Sqrt(distX * distX + distY * distY);
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
                if (startDirection)
                {
                    if (direction == 1 && transform.position.y >= startPosition.y + yMargin)
                    {
                        direction = -1;
                    }
                    else if (direction == -1 && transform.position.y <= startPosition.y)
                    {
                        direction = 1;
                    }
                }
                else
                {
                    if (direction == 1 && transform.position.y >= startPosition.y)
                    {
                        direction = -1;
                    }
                    else if (direction == -1 && transform.position.y <= startPosition.y - yMargin)
                    {
                        direction = 1;
                    }
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

    public float Speed()
    {
        return moving.Speed * direction;
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
