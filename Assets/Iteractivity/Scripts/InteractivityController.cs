using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractivityController : MonoBehaviour
{
    public bool  hasInteracted = false;
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;

    void Update()
    {
        if (isInRange){
            if (Input.GetKeyDown(interactKey)){
                interactAction.Invoke();
                hasInteracted = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            isInRange = true;
            collision.gameObject.GetComponent<PlayerManager>().NotifyPlayer();
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            isInRange = false;
            collision.gameObject.GetComponent<PlayerManager>().DeNotifyPlayer();
            Debug.Log("Player out of range");
        }
    }
}
