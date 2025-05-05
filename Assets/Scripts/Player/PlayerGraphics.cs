using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator animator;
    private PlayerPhysics playerPhysics;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    private void FixedUpdate()
    {
        if (!playerPhysics.IsGrounded()) // change
        {
            if (IsFalling())
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
            else if (IsJumping())
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    public void SetWalking(bool walking)
    {
        animator.SetBool("isWalking", walking);
    }

    private bool IsFalling()
    {
        return _rigidbody.velocity.y < -0.01f;
    }

    private bool IsJumping()
    {
        return _rigidbody.velocity.y > 0.01f;
    }
}