using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.Sprites;
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

    private int keysFound = 0;
    private int keysAmount = 3;
    private int lives = 3;
    private Vector3 startPosition;

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

    private void Awake()
    {
        startPosition = transform.position;
    }

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartJump();
        }



        if(movingPlatform != null)
        {
            MoveWithPlatform();
        }


        //if (wasOnPlatform && !isGrounded)
        //{
        //    transform.Translate(movingPlatformSpeedX * Time.deltaTime, 0.0f, 0.0f, Space.World);
        //}
        //else if (wasOnPlatform && isGrounded)
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
            BreakablePlatform breakablePlatform = collision.gameObject.transform.parent.gameObject.GetComponent<BreakablePlatform>();
            movingPlatform = collision.gameObject.transform.parent.gameObject.GetComponent<MovingPlatform>();
            if(breakablePlatform != null)
            {
                breakablePlatform.Activate();
            }
        }
        if(collision.CompareTag("LevelEnd"))
        {
            if(keysFound == keysAmount)
            {
                Debug.Log("Level is finished!");
            }
            else
            {
                Debug.Log("Not all keys are found!");
            }
        }

        if (collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            if(transform.position.y > enemy.transform.position.y)
            {
                ScoreController.GetController().IncreaseScore(enemy.GetComponent<EnemyController>().Points());
                Debug.Log("Enemy died!");
            }
            else
            {
                lives--;
                transform.position = startPosition;
                _rigidbody.velocity = Vector3.zero;
                if (lives == 0)
                {
                    Debug.Log("Game over!");
                }
                else
                {
                    Debug.Log(lives + " lives remaind!");
                }

            }
        }
        if (collision.CompareTag("Key"))
        {
            keysFound++;
            Debug.Log(keysFound + "/" + keysAmount + " found!");
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Heart"))
        {
            lives++;
            Debug.Log(lives-1 + " + 1, now " + lives);
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("FallCollider"))
        {
            Death();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlatformTop"))
        {
            if(movingPlatform != null)
            {
                movingPlatformSpeedX = movingPlatform.SpeedX();
                wasOnPlatform = true;
                movingPlatform = null;
            }
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

    private void Death()
    {
        lives--;
        transform.position = startPosition;
        _rigidbody.velocity = Vector3.zero;
        if (lives == 0)
        {
            Debug.Log("Game over!");
        }
        else
        {
            Debug.Log(lives + " lives remaind!");
        }
    }
}
