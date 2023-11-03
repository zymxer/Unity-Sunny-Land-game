using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D _rigidbody;
    private PlayerPhysics playerPhysics;
    void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    void FixedUpdate()
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
        //if(!_playerPhysics.OnPlatform())
        //{
        //    if (IsFalling())
        //    {
        //        _animator.SetBool("isFalling", true);
        //        _animator.SetBool("isJumping", false);
        //    }
        //    else if (IsJumping())
        //    {
        //        _animator.SetBool("isJumping", true);
        //        _animator.SetBool("isFalling", false);
        //    }
        //}
        //else
        //{
        //    _animator.SetBool("isJumping", false);
        //    _animator.SetBool("isFalling", false);
        //}
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
