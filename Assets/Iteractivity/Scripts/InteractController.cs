using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractController : MonoBehaviour
{
    public bool isInteracted;
    public string sceneName;

    public virtual void Interact()
    {
        Debug.Log("Interacting with the object");

        if (!isInteracted)
        {
            isInteracted = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}

