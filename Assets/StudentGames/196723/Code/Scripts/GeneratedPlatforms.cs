using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace _196723
{
    public class GeneratedPlatforms : MonoBehaviour
    {
        public GameObject platform;
        public float GmarginX;
        public float GmarginY;


        public float speed;
        public float moveMarginX;

        public Vector3 startPosition;
        private const int PLATFORMS_NUM = 5;
        private GameObject[] platforms = new GameObject[PLATFORMS_NUM];
        private Vector3[] positions = new Vector3[PLATFORMS_NUM];

        private Vector3[] margins = new Vector3[PLATFORMS_NUM];

        private int[] currentDirections = new int[PLATFORMS_NUM];

        private void Start()
        {
            CreatePlatforms();
            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                margins[i] = new Vector3(positions[i].x + moveMarginX, 0.0f, 0.0f);
            }
            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                if (i < PLATFORMS_NUM / 2)
                {
                    currentDirections[i] = 1;
                }
                else if (i > PLATFORMS_NUM / 2)
                {
                    currentDirections[i] = -1;
                }
            }
        }

        private void Update()
        {
            MovePlatforms();
        }

        private Vector3 GetIPosition(int i)
        {
            return new Vector3(startPosition.x + GmarginX * i, startPosition.y + GmarginY * i, 0.0f);
        }

        private void CreatePlatforms()
        {
            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                positions[i] = GetIPosition(i);
                platforms[i] = Instantiate(platform, positions[i], Quaternion.identity);
            }
        }

        private void MovePlatforms()
        {
            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                platforms[i].transform.Translate(speed * currentDirections[i] * Time.deltaTime, 0.0f, 0.0f);
                if (currentDirections[i] == 1 && platforms[i].transform.position.x >= margins[i].x)
                {
                    UpdateDirections();
                }
            }
        }

        private void UpdateDirections()
        {
            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                currentDirections[i] *= -1;
            }
        }
    }

}
