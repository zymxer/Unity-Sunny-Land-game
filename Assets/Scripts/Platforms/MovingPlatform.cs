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
    private List<Vector2> points = new List<Vector2>();
    [SerializeField]
    private Line line;

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
        if(line != null)
        {
            points = line.points;
        }
        prevPosition.x = transform.position.x;
        prevPosition.y = transform.position.y;
        switch (moveMode)
        {
            case MoveMode.Horizontal:
                //if(_xAmplitude != 0.0f)
                transform.Translate(speed * Time.deltaTime * direction * localTimeScale, 0.0f, 0.0f, Space.World);
                break;
            case MoveMode.Vertical:
                //if(_xAmplitude != 0.0f)
                transform.Translate(0.0f, speed * Time.deltaTime * direction * localTimeScale, 0.0f, Space.World);
                break;
            case MoveMode.Trajectory:
                nextX = Mathf.Lerp(transform.position.x, points[pointIndex].x, speed * Time.deltaTime);
                nextY = Mathf.Lerp(transform.position.y, points[pointIndex].y, speed * Time.deltaTime);
                transform.Translate(nextX - transform.position.x, nextY - transform.position.y, 0.0f, Space.World);
                if(Mathf.Abs(transform.position.x - points[pointIndex].x) < 0.01f &&  Mathf.Abs(transform.position.y - points[pointIndex].y) < 0.01f)
                {
                    UpdatePointIndex();
                }
                break;
            default: break;

        }
        UpdateDeltas();
        CheckDirection();
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
        if(direction == 1 && pointIndex == points.Count - 1)
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
                pointIndex = points.Count - 1;
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
        //Debug.Log(_deltaY);
    }

    public float SpeedX()
    {
        //return DeltaX() / Time.deltaTime;
        return speed * direction;
    }
    public float SpeedY()
    {
        //return _speed * _direction;
        //Debug.Log(DeltaY() * 1000.0f + " / " + Time.deltaTime * 1000.0f);
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
