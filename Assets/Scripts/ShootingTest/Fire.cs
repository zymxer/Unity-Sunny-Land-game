using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float effectDuration;
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool affectsPlayer;

    private static ArrayList affectedObjects = new ArrayList();
    private BoxCollider2D boxCollider;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
