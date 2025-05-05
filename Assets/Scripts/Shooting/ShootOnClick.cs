using UnityEngine;

public class ShootOnClick : MonoBehaviour
{
    [SerializeField] private MouseData mouseData;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform shotPoint;

    private GameObject cloneProjectile;

    private Vector3 shotPosition;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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