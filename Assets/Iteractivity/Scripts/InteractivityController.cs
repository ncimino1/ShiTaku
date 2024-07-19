using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDetails
{
    public Sprite RoomImage;
    public Sprite NPCImage;
    public Dialouge NPCDialouge;
    public bool NPCResolved;
    public bool HasInteracted;
}

public class InteractivityController : MonoBehaviour
{
    public bool  hasInteracted = false;
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;

    public RoomDetails Details;

    public NPCMenu Menu;

    void Update()
    {
        if (isInRange){
            if (Input.GetKeyDown(interactKey))
            {
                Menu.CurrInteraction = gameObject;
                Debug.Log("calling");
                Menu.Details = Details;
                Menu.gameObject.SetActive(true);
                // interactAction.Invoke();
                Menu.Interact();
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
