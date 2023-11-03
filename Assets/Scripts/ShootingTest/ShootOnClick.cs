using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnClick : MonoBehaviour
{
    [SerializeField]
    private ClickData clickData;
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
        if(Input.GetMouseButtonDown(0)) 
        {
            clickData.UpdateClickData();
            shotPosition = shotPoint.transform.position;// (transform.position);
            cloneProjectile = Instantiate(projectilePrefab, shotPosition, Quaternion.identity);

            cloneProjectile.transform.eulerAngles = new Vector3(0f, 0f, clickData.Angle());
            cloneProjectile.GetComponent<Projectile>().SetAxisSpeed(clickData.Angle());
        }
    }
}
