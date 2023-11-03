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
    private Timer jumpTimer;

    private Rigidbody2D _rigidbody;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private Vector3 scale;
    public LayerMask groundLayer;
    private PlayerGraphics playerGraphics;
    private float jumpTimeMult;
    private MovingPlatform movingPlatform;

    private float movingPlatformSpeedX;
    private bool wasOnPlatform = false;

    private bool isGrounded = false;

    void Start()
    {
        scale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
        jumpTimer = GetComponent<Timer>();
        jumpTimer.OnEnd().AddListener(AddJumpForce);
        jumpTimer.OnValueChanged().AddListener(UpdateJumpTimeMult);
        playerGraphics = GetComponent<PlayerGraphics>();
    }


    void Update()
    {
        if(CheckGrounded())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (!isFacingRight)
            {
                Rotate();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (isFacingRight)
            {
                Rotate();
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            isWalking = true;
            playerGraphics.SetWalking(true);
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            isWalking = true;
            playerGraphics.SetWalking(true);
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            isWalking = false;
            playerGraphics.SetWalking(false);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndJump();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            StartJump();
        }



        if(movingPlatform != null)
        {
            MoveWithPlatform();
        }


        //if(wasOnPlatform && !isGrounded)
        //{
        //    transform.Translate(movingPlatformSpeedX * Time.deltaTime, 0.0f, 0.0f, Space.World);
        //}
        //else if(wasOnPlatform && isGrounded)
        //{
        //    wasOnPlatform = false;
        //}
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.white);
    }

    private bool CheckGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }    

    private void StartJump()
    {
        if (IsGrounded())
        {
            jumpTimer.Activate();
        }
    }

    private void EndJump()
    {
        if (jumpTimer.IsActive())
        {
            jumpTimer.End();
        }
    }

    private void AddJumpForce()
    {
        _rigidbody.AddForce(Vector2.up * jumpForce * jumpTimeMult, ForceMode2D.Impulse);
    }

    private void UpdateJumpTimeMult()
    {
        jumpTimeMult = jumpTimer.TimePastPercent();
    }

    private void Rotate()
    {
        isFacingRight = !isFacingRight;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bonus"))
        {
            Bonus picked = collision.gameObject.GetComponent<Bonus>();
            ScoreController.GetController().IncreaseScore(picked.GetPoints());
            Debug.Log("Score + " + picked.GetPoints());
            picked.StartPickupAnimation();
        }
        if(collision.CompareTag("PlatformTop"))
        {
            movingPlatform = collision.gameObject.transform.parent.gameObject.GetComponent<MovingPlatform>();
        }
        if(collision.CompareTag("LevelEnd"))
        {
            Debug.Log("Level is finished!");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlatformTop"))
        {
            // add speed
            movingPlatformSpeedX = movingPlatform.SpeedX();
            wasOnPlatform = true;
            movingPlatform = null;
        }
    }

    private void MoveWithPlatform()
    {
        transform.Translate(movingPlatform.DeltaX(), movingPlatform.DeltaY(), 0.0f);
    }

    public bool OnPlatform()
    {
        return movingPlatform != null;
    }
}
