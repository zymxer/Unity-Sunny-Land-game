using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ClickData : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Transform reference;
    [SerializeField] private float xDistance;
    [SerializeField] private float yDistance;
    [SerializeField] private float distance;
    [SerializeField] private float angle;
    private Vector3 _referencePosition;
    public float radians;
    public float cosin;
    public Transform test;

    public void UpdateClickData()
    {
        _referencePosition = reference.position;
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateDistance();
        CalculateAngle();
        test.eulerAngles = new Vector3(0f, 0f, angle); //вместо этого надо ротейт с умной дельтой какой-то
    }

    private void CalculateDistance()
    {
        xDistance = position.x - _referencePosition.x;
        yDistance = position.y - _referencePosition.y;
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
        if(yDistance < 0)
        {
            angle *= -1;
        }
    }

    private void Update()
    {
        UpdateClickData();
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
}
