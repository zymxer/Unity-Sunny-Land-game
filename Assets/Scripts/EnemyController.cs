using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private bool isMovingRight = true;
    [SerializeField]
    private float moveRange = 0.0f;

    [SerializeField]
    private int point = 6;

    private int direction;
    private float startPositionX;
    private bool isFacingRight = false;
    private Animator animator;
    private Vector3 scale;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public int Points()
    {
        return point;
    }

    private void Start()
    {
        direction = isMovingRight ? 1 : -1;
        scale = transform.localScale;
        if(isMovingRight)
        Rotate();
        startPositionX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
        transform.Translate(moveSpeed * Time.deltaTime * direction, 0.0f, 0.0f, Space.World);
    }

    private void CheckDirection()
    {
        if (isMovingRight)
        {
            if (direction == 1 && transform.position.x >= startPositionX + moveRange)
            {
                direction = -1;
                Rotate();
            }
            else if (direction == -1 && transform.position.x <= startPositionX)
            {
                direction = 1;
                Rotate();
            }
        }
        else
        {
            if (direction == 1 && transform.position.x >= startPositionX)
            {
                direction = -1;
                Rotate();
            }
            else if (direction == -1 && transform.position.x <= startPositionX - moveRange)
            {
                direction = 1;
                Rotate();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

        private void Rotate()
    {
        isFacingRight = !isFacingRight;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
