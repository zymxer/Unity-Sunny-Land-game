using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] private float speed;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public void SpeedUp(float ratio)
    {
        speed *= ratio;
    }

    public void SlowDown(float ratio)
    {
        speed /= ratio;
    }
}