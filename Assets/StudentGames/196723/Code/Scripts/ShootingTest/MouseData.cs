using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.UI;

public class MouseData : MonoBehaviour
{
    [SerializeField]
    private Transform reference;

    private Vector3 position;
    private float xDistance;
    private float yDistance;
    private float distance;
    [SerializeField] private float angle;

    private Vector3 referencePosition;
    private float radians;
    private float cosin;

    private void Update()
    {
        UpdateMouseData();
    }
    public void UpdateMouseData()
    {
        referencePosition = reference.position;
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateDistance();
        CalculateAngle();
    }

    public float Angle()
    {
        return angle;
    }

    public float Radians()
    {
        return radians;
    }

    public float Distance()
    {
        return distance;
    }

    public Vector2 GetMousePosition()
    {
        return position;
    }

    public void SetTransform(Transform newTransform)
    {
        reference = newTransform;
    }

    private void CalculateDistance()
    {
        xDistance = position.x - referencePosition.x;
        yDistance = position.y - referencePosition.y;
        distance = Mathf.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
    }

    private void CalculateAngle()
    {
        cosin = xDistance / distance;
        radians = Mathf.Acos(cosin);
        angle = radians * Mathf.Rad2Deg;
        CheckQuarter();
    }

    private void CheckQuarter()
    {
        if (yDistance < 0)
        {
            angle *= -1;
        }
    }
}
