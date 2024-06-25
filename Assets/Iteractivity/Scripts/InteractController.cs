using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractController : MonoBehaviour
{
    public bool isInteracted;
    public GameObject interior;
    public GameObject exterior;
    public GameObject player;

    public virtual void Interact()
    {
        Debug.Log("Interacting with the object");

        if (exterior.activeSelf)
        {
            exterior.SetActive(false);
            interior.SetActive(true);
            player.transform.position = interior.transform.Find("Spawn").position;
        }
        else
        {
            exterior.SetActive(true);
            interior.SetActive(false);
        }
    }
}