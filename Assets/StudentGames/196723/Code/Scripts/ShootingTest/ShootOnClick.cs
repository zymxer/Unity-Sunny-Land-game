using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _196723
{
    public class ShootOnClick : MonoBehaviour
    {
        [SerializeField]
        private MouseData mouseData;
        [SerializeField]
        private GameObject projectilePrefab;
        private GameObject cloneProjectile;
        [SerializeField]
        private Transform shotPoint;

        private Vector3 shotPosition;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            mouseData.UpdateMouseData();
            if (Input.GetMouseButtonDown(0))
            {
                shotPosition = shotPoint.transform.position;
                cloneProjectile = Instantiate(projectilePrefab, shotPosition, Quaternion.identity);

                cloneProjectile.transform.eulerAngles = new Vector3(0f, 0f, mouseData.Angle());
                cloneProjectile.GetComponent<Projectile>().SetProjectile(mouseData.Angle());
            }
        }
    }
}
