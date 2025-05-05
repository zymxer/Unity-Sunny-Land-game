using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private int _points;

    [Space] [SerializeField] private float _pickupSpeed;

    [SerializeField] private float _pickupYOffset;

    private Animator _animator;
    private Vector3 _animEndPosition;
    private bool _animStarted;

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animEndPosition = new Vector3(transform.position.x, transform.position.y + _pickupYOffset, 0.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_animStarted)
            transform.position = Vector3.Lerp(transform.position, _animEndPosition, _pickupSpeed * Time.deltaTime);
    }

    public void StartPickupAnimation()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false; //if-y
        _animator.SetBool("isPicked", true);
        _animStarted = true;
    }

    public void EndPickupAnimation()
    {
        gameObject.SetActive(false);
    }

    public int GetPoints()
    {
        return _points;
    }
}