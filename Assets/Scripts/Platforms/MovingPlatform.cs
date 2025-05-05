using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("True for Right/Up")] [SerializeField]
    private bool startDirection = true;

    [Space] [SerializeField] private MoveMode moveMode;

    [Header("For Horizontal mode")] [SerializeField]
    private float xMargin;

    [Header("For Vertical mode")] [SerializeField]
    private float yMargin;

    [Header("For Trajectory mode")] [SerializeField]
    private bool cycled;

    [SerializeField] private Transform[] waypoints = new Transform[0];

    private Transform _transform;
    private float deltaX;
    private float deltaY;
    private int direction;

    private Moving moving;
    private float nextX = 0.0f;
    private float nextY = 0.0f;

    private int pointIndex;

    private Vector2[] points;

    private Vector2 prevPosition;

    private Vector3 startPosition;

    // Start is called before the first frame update
    private void Start()
    {
        moving = GetComponent<Moving>();
        points = new Vector2[waypoints.Length];
        for (var i = 0; i < waypoints.Length; i++) points[i] = waypoints[i].position;
        startPosition = transform.position;
        _transform = GetComponent<Transform>();
        direction = startDirection ? 1 : -1;
        if (moveMode == MoveMode.Trajectory) transform.position = new Vector3(points[0].x, points[0].y, 0f);
    }

    // Update is called once per frame
    private void Update()
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

                var distanceToPoint = DistanceToPoint();
                if (distanceToPoint >= 1.5f)
                    transform.position = Vector2.MoveTowards(transform.position, points[pointIndex],
                        moving.Speed * Time.deltaTime);
                else
                    transform.position = Vector2.MoveTowards(transform.position, points[pointIndex],
                        moving.Speed * Time.deltaTime * distanceToPoint / 1.5f);
                if (CheckPointIndex()) UpdatePointIndex();
                break;
        }

        CheckDirection();
    }

    private bool CheckPointIndex()
    {
        return Mathf.Abs(transform.position.x - points[pointIndex].x) < 0.01f &&
               Mathf.Abs(transform.position.y - points[pointIndex].y) < 0.01f;
    }

    private float DistanceToPoint()
    {
        var distX = transform.position.x - points[pointIndex].x;
        var distY = transform.position.y - points[pointIndex].y;
        return Mathf.Sqrt(distX * distX + distY * distY);
    }

    private void CheckDirection()
    {
        switch (moveMode)
        {
            case MoveMode.Horizontal:
                if (startDirection)
                {
                    if (direction == 1 && transform.position.x >= startPosition.x + xMargin)
                        direction = -1;
                    else if (direction == -1 && transform.position.x <= startPosition.x) direction = 1;
                }
                else
                {
                    if (direction == 1 && transform.position.x >= startPosition.x)
                        direction = -1;
                    else if (direction == -1 && transform.position.x <= startPosition.x - xMargin) direction = 1;
                }

                break;
            case MoveMode.Vertical:
                if (startDirection)
                {
                    if (direction == 1 && transform.position.y >= startPosition.y + yMargin)
                        direction = -1;
                    else if (direction == -1 && transform.position.y <= startPosition.y) direction = 1;
                }
                else
                {
                    if (direction == 1 && transform.position.y >= startPosition.y)
                        direction = -1;
                    else if (direction == -1 && transform.position.y <= startPosition.y - yMargin) direction = 1;
                }

                break;
        }
    }

    private void UpdatePointIndex()
    {
        if (direction == 1 && pointIndex == points.Length - 1)
        {
            if (cycled)
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

    private enum MoveMode
    {
        Horizontal,
        Vertical,
        Trajectory
    }
}