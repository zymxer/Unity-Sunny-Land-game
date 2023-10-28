using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private PlayerPhysics _playerPhysics;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerPhysics = GetComponent<PlayerPhysics>();
    }

    void FixedUpdate()
    {
        if(!_playerPhysics.IsGrounded()) // change
        {
            if (IsFalling())
            {
                _animator.SetBool("isFalling", true);
                _animator.SetBool("isJumping", false);
            }
            else if(IsJumping())
            {
                _animator.SetBool("isJumping", true);
                _animator.SetBool("isFalling", false);
            }
        }
        else
        {
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isFalling", false);
        }
    }

    public void SetWalking(bool walking)
    {
        _animator.SetBool("isWalking", walking);
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
