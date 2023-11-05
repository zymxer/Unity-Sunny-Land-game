using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class BreakablePlatform : MonoBehaviour
{
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private Sprite[] stages = new Sprite[0];

    private int currentStage = 0;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer.OnEnd().AddListener(OnTimerEnd);
        timer.OnValueChanged().AddListener(OnTimerChange);
    }

    public void Activate()
    {
        timer.Activate();
    }

    private void OnTimerChange()
    {
        currentStage = Mathf.RoundToInt((stages.Length - 1) * timer.TimePastPercent());
        spriteRenderer.sprite = stages[currentStage];
    }

    private void OnTimerEnd()
    {
        timer.Remove();
        Destroy(gameObject);
    }
}
