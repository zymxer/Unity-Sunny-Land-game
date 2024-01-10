using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace _196723
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField]
        private int _points = 0;
        [Space]
        [SerializeField]
        private float _pickupSpeed = 0.0f;
        [SerializeField]
        private float _pickupYOffset = 0.0f;

        private Animator _animator;
        private Vector3 _animEndPosition;
        private bool _animStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _animEndPosition = new Vector3(transform.position.x, transform.position.y + _pickupYOffset, 0.0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (_animStarted)
            {
                transform.position = Vector3.Lerp(transform.position, _animEndPosition, _pickupSpeed * Time.deltaTime);
            }
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

}
