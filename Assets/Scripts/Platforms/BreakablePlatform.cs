using UnityEngine;

[RequireComponent(typeof(Timer))]
public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] private float duration;

    [SerializeField] private Sprite[] stages = new Sprite[0];

    private int currentStage;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int stagesAmount;

    private Timer timer;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(duration);
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer.OnEnd().AddListener(OnTimerEnd);
        timer.OnValueChanged().AddListener(OnTimerChange);

        stagesAmount = stages.Length;
    }

    public void Activate()
    {
        timer.Activate();
    }

    private void OnTimerChange()
    {
        currentStage = Mathf.RoundToInt((stagesAmount - 1) * timer.TimePastPercent());
        spriteRenderer.sprite = stages[currentStage];
        if (currentStage == stagesAmount - 1)
        {
            GetComponent<Collider2D>().enabled = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1.0f;
        }
    }

    private void OnTimerEnd()
    {
        timer.Remove();
        Destroy(gameObject);
    }
}