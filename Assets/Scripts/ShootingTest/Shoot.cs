using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            double Ydist = worldPosition.y + 66.8;
            double Xdist = Math.Abs(worldPosition.x);
            double Dist = Math.Sqrt((Ydist * Ydist) + (Xdist * Xdist));
            double cosin = Xdist / Dist;
            float Rad = Mathf.Acos(Convert.ToSingle(cosin));
            float Angle = Rad * Mathf.Rad2Deg;
            if (worldPosition.x > 0)
            {
                Angle = 180 - Angle;
            }
/*            int RocketIndex = UnityEngine.Random.Range(0, Rockets.Length);
            Rocket = Rockets[RocketIndex];
            Rocket.startLifetime = Convert.ToSingle(Dist) / Rocket.startSpeed;
            Instantiate(Rocket, new Vector3(0, -66.8f, 0), transform.rotation * Quaternion.Euler(Angle, 90f, 90f));*/
        }
    }
}
