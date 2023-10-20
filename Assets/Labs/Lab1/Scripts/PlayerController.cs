using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)]
    [SerializeField] 
    private float moveSpeed = 0.1f; // moving speed of the player
    [Space(10)]
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float jumpForce = 0.1f;

    public float rayLength = 2.0f;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _isWalking = false;
    private bool _isFacingRight = true;
    private Vector3 _scale;
    public LayerMask groundLayer;

    private void Awake()
    {
        _scale = transform.localScale;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if(!_isFacingRight)
            {
                _scale.x *= -1;
                transform.localScale = _scale;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (_isFacingRight)
            {
                _scale.x *= -1;
                transform.localScale = _scale;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _isFacingRight = true;
            _isWalking = true;
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _isFacingRight = false;
            _isWalking = true;
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            _isWalking = false;
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            _isWalking = false;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.white);
        _animator.SetBool("isGrounded", IsGrounded());
        _animator.SetBool("isWalking", _isWalking);
        if(!_isFacingRight)
        {

        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
    }

    public void Test()
    {
        Debug.Log("Test Successful");
    }

}
