using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingPlatform : MonoBehaviour
{
    private enum MoveMode { Horizontal, Vertical, Trajectory };

    [Header("< 10 for Horizontal/Vertical, > 50 for Trajectory")]
    [SerializeField]
    private float _speed = 0.0f;
    [Header("True for Right/Up")]
    [SerializeField]
    private bool _startDirection = true;

    [Space]
    [SerializeField]
    private MoveMode _moveMode;

    [Header("For Horizontal mode")]
    [SerializeField]
    private float _xAmplitude = 0.0f;

    [Header("For Vertical mode")]
    [SerializeField]
    private float _yAmplitude = 0.0f;

    [Header("For Trajectory mode")]
    [SerializeField]
    private bool _cycled;
    [SerializeField]
    private List<Vector2> _points = new List<Vector2>();
    [SerializeField]
    private Line _line;

    private Vector3 _startPosition;
    private Transform _transform;
    private int _direction;
    private float _localTimeScale = 1.0f;

    private int _pointIndex = 0;
    private float _nextX = 0.0f;
    private float _nextY = 0.0f;

    private Vector2 _prevPosition;
    private float _deltaX = 0.0f;
    private float _deltaY = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _transform = GetComponent<Transform>();
        _direction = _startDirection ? 1 : -1;
        if(_moveMode == MoveMode.Trajectory)
        {
            transform.position = new Vector3(_points[0].x, _points[0].y, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_line != null)
        {
            _points = _line.points;
        }
        _prevPosition.x = transform.position.x;
        _prevPosition.y = transform.position.y;
        switch (_moveMode)
        {
            case MoveMode.Horizontal:
                //if(_xAmplitude != 0.0f)
                transform.Translate(_speed * Time.deltaTime * _direction * _localTimeScale, 0.0f, 0.0f, Space.World);
                break;
            case MoveMode.Vertical:
                //if(_xAmplitude != 0.0f)
                transform.Translate(0.0f, _speed * Time.deltaTime * _direction * _localTimeScale, 0.0f, Space.World);
                break;
            case MoveMode.Trajectory:
                _nextX = Mathf.Lerp(transform.position.x, _points[_pointIndex].x, _speed * Time.deltaTime);
                _nextY = Mathf.Lerp(transform.position.y, _points[_pointIndex].y, _speed * Time.deltaTime);
                transform.Translate(_nextX - transform.position.x, _nextY - transform.position.y, 0.0f, Space.World);
                if(Mathf.Abs(transform.position.x - _points[_pointIndex].x) < 0.01f &&  Mathf.Abs(transform.position.y - _points[_pointIndex].y) < 0.01f)
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
        switch (_moveMode)
        {
            case MoveMode.Horizontal:
                if(_direction == 1 && _transform.position.x >= _startPosition.x + _xAmplitude)
                {
                    _direction = -1;
                }
                else if(_direction == -1 && _transform.position.x <= _startPosition.x - _xAmplitude)
                {
                    _direction = 1;
                }
                break;
            case MoveMode.Vertical:
                if (_direction == 1 && _transform.position.y >= _startPosition.y + _yAmplitude)
                {
                    _direction = -1;
                }
                else if (_direction == -1 && _transform.position.y <= _startPosition.y - _yAmplitude)
                {
                    _direction = 1;
                }
                break;
            default: break;

        }
    }

    private void UpdatePointIndex()
    {
        if(_direction == 1 && _pointIndex == _points.Count - 1)
        {
            if(_cycled)
            {
                _pointIndex = 0;
                return;
            }
            _direction = -1;
        }
        else if (_direction == -1 && _pointIndex == 0)
        {
            if (_cycled)
            {
                _pointIndex = _points.Count - 1;
                return;
            }
            _direction = 1;
        }
        _pointIndex += _direction;
    }

    private void UpdateDeltas()
    {
        _deltaX = transform.position.x - _prevPosition.x;
        _deltaY = transform.position.y - _prevPosition.y;
    }

    public float DeltaX()
    {
        return _deltaX;
    }
    public float DeltaY()
    {
        return _deltaY;
    }
}
