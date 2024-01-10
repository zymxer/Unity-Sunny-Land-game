using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.Sprites;
using UnityEngine;

[RequireComponent(typeof(Moving))]

public class PlayerPhysics : MonoBehaviour
{
    [Header("Movement parameters")]

    private Moving moving;

    [Space(10)]
    [Range(0.01f, 100.0f)]
    [SerializeField]
    private float jumpForce = 0.1f;

    [SerializeField]
    private float jumpTimerDuration = 0.0f;

    [Space]
    [SerializeField]
    private float rayLength = 2.0f;

    private Timer jumpTimer;

    private Vector3 startPosition;

    private Rigidbody2D _rigidbody;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private Vector3 scale;
    public LayerMask groundLayer;
    private PlayerGraphics playerGraphics;
    private float jumpTimeMult;

    GameObject platform = null;

    private float movingPlatformSpeedX;

    private bool isGrounded = false;

    private bool onPressTop = false;

    void Start()
    {
        moving = GetComponent<Moving>();

        startPosition = transform.position;
        scale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();

        jumpTimer = gameObject.AddComponent<Timer>();
        jumpTimer.SetTimer(jumpTimerDuration);
        jumpTimer.OnEnd().AddListener(AddJumpForce);
        jumpTimer.OnValueChanged().AddListener(UpdateJumpTimeMult);

        playerGraphics = GetComponent<PlayerGraphics>();

        GetComponent<StatsContainer>().OnZeroHealth.AddListener(GameManager.instance.KillPlayer);

    }


    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            isGrounded = CheckGrounded();
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                playerGraphics.SetWalking(true);
                transform.Translate(moving.Speed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                isWalking = true;
                playerGraphics.SetWalking(true);
                transform.Translate(-moving.Speed * Time.deltaTime, 0.0f, 0.0f, Space.World);
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

            //Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.white);
        }
    }

    private bool CheckGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }    

    public bool IsFacingRight()
    {
        return isFacingRight;
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
            GameManager.instance.IncreaseScore(picked.GetPoints());
            picked.StartPickupAnimation();
        }
        if(collision.CompareTag("Platform"))
        {
            GameObject platform = collision.gameObject;
            BreakablePlatform breakablePlatform = platform.GetComponent<BreakablePlatform>();
            MovingPlatform movingPlatform = platform.GetComponent<MovingPlatform>();
            if(breakablePlatform != null)
            {
                breakablePlatform.Activate();
            }
            if(movingPlatform != null)
            {
                transform.SetParent(platform.transform);
            }
        }
        //if(collision.CompareTag("LevelEnd"))
        //{
        //    if (keysFound == keysAmount)
        //    {
        //        ScoreController.GetController().IncreaseScore(100 * lives);
        //        GameManager.instance.LevelCompleted();
        //    }
        //}


        if (collision.CompareTag("Key"))
        {
            //audioSource.PlayOneShot(keySound, AudioListener.volume);

            collision.gameObject.SetActive(false);
            GameManager.instance.IncreaseKeysAmount();

        }

        if (collision.CompareTag("Heart"))
        {
            GameManager.instance.IncreaseLives();
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("FallCollider"))
        {
            GameManager.instance.KillPlayer();
        }

        if(collision.CompareTag("Press") && isGrounded)
        {
            GameManager.instance.KillPlayer();
        }

        if(collision.CompareTag("Checkpoint"))
        {
            GameManager.instance.Checkpoint = collision.transform;
        }

        if(collision.CompareTag("PressTop"))
        {
            onPressTop = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("World"))
        {
            if (onPressTop)
            {
                onPressTop = false;
                GameManager.instance.KillPlayer();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") && collision.gameObject.activeInHierarchy)
        {
            transform.SetParent(null);
        }

        if (collision.CompareTag("PressTop"))
        {
            onPressTop = false;
        }
    }
}
