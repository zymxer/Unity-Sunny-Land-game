using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.ComponentModel;

public class ClickData : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Transform reference;
    [SerializeField] private bool rotateReference;
    [SerializeField] private float xDistance;
    [SerializeField] private float yDistance;
    [SerializeField] private float distance;
    [SerializeField] private float angle;

    private Vector3 referencePosition;
    private float radians;
    private float cosin;

    private void Update()
    {
        UpdateClickData();
        if (rotateReference)
        {
            RotateReference();
        }
    }
    public void UpdateClickData()
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

    private void RotateReference()
    {
        reference.eulerAngles = new Vector3(0f, 0f, angle); //вместо этого надо ротейт с умной дельтой какой-то
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
