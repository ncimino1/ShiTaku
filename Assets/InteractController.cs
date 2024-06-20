using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    public bool isInteracted;

    public virtual void Interact()
    {
        Debug.Log("Interacting with the object");

        if (!isInteracted)
        {
            isInteracted = true;
        }
    }
}

