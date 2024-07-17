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
    public GameObject RoomNPC;
    public GameObject Room;

    public bool NPCGone;

    public virtual void Interact()
    {
        Debug.Log("Interacting with the object right now");

        Room.SetActive(true);

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

                //If the door is interacted with, have the npc not be displayed
                if(!NPCGone){
                    NPCCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("shrineWorker");
                }
                break;

            case "smile":
                RoomCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("smiling_friends");

                //If the door is interacted with, have the npc not be displayed
                if(!NPCGone){
                    NPCCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("fire_fighter");
                }
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
        //If the NPC is not gone, set it as active then fade it in
        if(!NPCGone){
            RoomNPC.SetActive(true);
        }

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

            if(!NPCGone){
                NPCCanvasGroup.alpha += Time.deltaTime;
            }
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

            if(!NPCGone){
                NPCCanvasGroup.alpha -= Time.deltaTime;
            }
            yield return null;
        }

        Room.SetActive(false);

        if(!NPCGone){
            RoomNPC.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuManagerScript = GameMenuCanvas.GetComponent<MenuManager>();

        NPCManagerScript = NPCMenu.GetComponent<NPCMenu>();

        spriteMovement = player.GetComponent<SpriteMovement>();

        NPCGone = false;
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