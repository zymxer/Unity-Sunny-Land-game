using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private bool destroysSelf = true;

    [Space(10)]
    [SerializeField]
    private GameObject[] objectsToActivate = new GameObject[0];
    [SerializeField]
    private GameObject[] objectsToDisable = new GameObject[0];
    [SerializeField]
    private Behaviour[] componentsToEnable = new Behaviour[0];
    [SerializeField]
    private Behaviour[] componentsToDisable = new Behaviour[0];

    [SerializeField]
    private UnityEvent other = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ActivateObjects();
            DisableObjects();

            EnableComponents();
            DisableComponents();

            other.Invoke();

            if(destroysSelf)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ActivateObjects()
    {
        for(int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(true);
        }
    }

    private void DisableObjects()
    {
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToActivate[i].SetActive(false);
        }
    }

    private void EnableComponents()
    {
        for (int i = 0; i < componentsToEnable.Length; i++)
        {
            componentsToEnable[i].enabled = true;
        }
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

}
