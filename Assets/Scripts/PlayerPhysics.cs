using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveSpeed = 0.1f; // moving speed of the player
    [Space(10)]
    [Range(0.01f, 100.0f)]
    [SerializeField]
    private float jumpForce = 0.1f;

    [SerializeField]
    private float rayLength = 2.0f;

    [SerializeField]
    private Timer _jumpTimer;

    private Rigidbody2D _rigidbody;
    private bool _isWalking = false;
    private bool _isFacingRight = true;
    private Vector3 _scale;
    public LayerMask groundLayer;
    private PlayerGraphics _playerGraphics;
    private float jumpTimeMult;
    private MovingPlatform _movingPlatform;

    void Start()
    {
        _scale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
        _jumpTimer = GetComponent<Timer>();
        _jumpTimer.OnEnd().AddListener(AddJumpForce);
        _jumpTimer.OnValueChanged().AddListener(UpdateJumpTimeMult);
        _playerGraphics = GetComponent<PlayerGraphics>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (!_isFacingRight)
            {
                Rotate();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (_isFacingRight)
            {
                Rotate();
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _isWalking = true;
            _playerGraphics.SetWalking(true);
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _isWalking = true;
            _playerGraphics.SetWalking(true);
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            _isWalking = false;
            _playerGraphics.SetWalking(false);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndJump();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            StartJump();
        }

        if(_movingPlatform != null)
        {
            MoveWithPlatform();
        }
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.white);
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void StartJump()
    {
        if (IsGrounded())
        {
            _jumpTimer.Activate();
        }
    }

    private void EndJump()
    {
        if (_jumpTimer.IsActive())
        {
            _jumpTimer.End();
        }
    }

    private void AddJumpForce()
    {
        _rigidbody.AddForce(Vector2.up * jumpForce * jumpTimeMult, ForceMode2D.Impulse);
    }

    private void UpdateJumpTimeMult()
    {
        jumpTimeMult = _jumpTimer.TimePastPercent();
    }

    private void Rotate()
    {
        _isFacingRight = !_isFacingRight;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bonus"))
        {
            ScoreController.GetController().IncreaseScore(5);
            Debug.Log("Score + 5");
            collision.gameObject.SetActive(false);
        }
        if(collision.CompareTag("PlatformTop"))
        {
            _movingPlatform = collision.gameObject.transform.parent.gameObject.GetComponent<MovingPlatform>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlatformTop"))
        {
            _movingPlatform = null;
        }
    }

    private void MoveWithPlatform()
    {
        transform.Translate(_movingPlatform.DeltaX(), _movingPlatform.DeltaY(), 0.0f);
    }
}
