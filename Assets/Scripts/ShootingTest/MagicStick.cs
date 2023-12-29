using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStick : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Vector3 playerScale;
    private bool playerFacingRight = true;

    private StatsContainer statsContainer;

    [SerializeField]
    private Transform shootPoint;

    private MouseData mouseData;

    private void Start()
    {
        statsContainer = player.GetComponent<StatsContainer>();
        mouseData = gameObject.AddComponent<MouseData>();
        mouseData.SetTransform(transform);

        playerScale = player.transform.localScale;
    }

    private void Update()
    {
        RotateStick();
    }

    private void RotateStick()
    {
        float angle = mouseData.Angle(), stickAngle = angle;
        if(!playerFacingRight)
        {
            stickAngle -= 180.0f;
        }
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, stickAngle);
        if ((angle > 90.0f || angle < -90.0f) && playerFacingRight)
        {
            RotatePlayer();
            playerFacingRight = false;
        }
        else if ((angle <= 90.0f && angle >= -90.0f) && !playerFacingRight)
        {
            RotatePlayer();
            playerFacingRight = true;
        }
    }

    private void RotatePlayer()
    {
        playerScale.x *= -1;
        player.transform.localScale = playerScale;
    }
}
