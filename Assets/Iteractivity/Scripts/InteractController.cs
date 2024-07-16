using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractController : MonoBehaviour
{
    public bool isInteracted;
    public GameObject interior;
    public GameObject exterior;
    public GameObject player;
    private SpriteMovement spriteMovement;
    public CanvasGroup RoomCanvasGroup;
    public CanvasGroup NPCCanvasGroup;
    public string roomName;
    public GameObject GameMenuCanvas;
    private MenuManager MenuManagerScript;
    public GameObject NPCMenu;
    private NPCMenu NPCManagerScript;

    public virtual void Interact()
    {
        Debug.Log("Interacting with the object right now");

        // if (exterior.activeSelf)
        // {
        //     exterior.SetActive(false);
        //     interior.SetActive(true);
        //     // player.transform.position = interior.transform.Find("Spawn").position;
        // }
        // else
        // {
        //     exterior.SetActive(true);
        //     interior.SetActive(false);
        // }

        //Change the image of the room that is being displayed based on a passed in parameter
        switch(roomName){
            case "tavern":
                RoomCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Tavern");
                NPCCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("shrineWorker");
                break;
            //Default to a white image
            default:
                RoomCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("White");
                NPCCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("White");
                break;
        }

        //When an NCPC is interacted with, fade in the room
        if (RoomCanvasGroup.alpha == 0){
            isInteracted = true;
            NPCManagerScript.exit = false;
            StartCoroutine(FadeIn());
        }
        
    }

    //Fade in the room
    public IEnumerator FadeIn()
    {
        Debug.Log("Fading In");

        isInteracted = true;

        //Lock the movement of the player
        spriteMovement.lockMovement = true;
        spriteMovement.LockMovement();
        Debug.Log("Player movement locked");
        
        MenuManagerScript.setNPCMenu(true);

        while (RoomCanvasGroup.alpha < 1)
        {
            RoomCanvasGroup.alpha += Time.deltaTime;
            NPCCanvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    //Fade out the room
    public IEnumerator FadeOut()
    {
        Debug.Log("Fading out");

        isInteracted = false;

        //Unlock the movement of the player
        spriteMovement.lockMovement = false;
        Debug.Log("Player movement unlocked");

        MenuManagerScript.setNPCMenu(false);

        while (RoomCanvasGroup.alpha > 0)
        {
            RoomCanvasGroup.alpha -= Time.deltaTime;
            NPCCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuManagerScript = GameMenuCanvas.GetComponent<MenuManager>();

        NPCManagerScript = NPCMenu.GetComponent<NPCMenu>();

        spriteMovement = player.GetComponent<SpriteMovement>();
    }

    void Update()
    {
        //Check to see if the exit of the menu option was selected, if so, close the menu and fadeout 
        if(NPCManagerScript.exit == true){
            StartCoroutine(FadeOut());
            NPCManagerScript.exit = false;
        }
    }
}