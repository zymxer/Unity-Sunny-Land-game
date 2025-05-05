using UnityEngine;

public class MouseData : MonoBehaviour
{
    [SerializeField] private Transform reference;

    [SerializeField] private float angle;
    private float cosin;
    private float distance;

    private Vector3 position;
    private float radians;

    private Vector3 referencePosition;
    private float xDistance;
    private float yDistance;

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
        distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);
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
        if (yDistance < 0) angle *= -1;
    }
}