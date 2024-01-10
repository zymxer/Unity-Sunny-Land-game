using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _196723
{
    public class Moving : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
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

}