// using System.Collections;
// using System.Collections.Generic;
// using System.Diagnostics.Tracing;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
//
// public class InteractController : MonoBehaviour
// {
//     public bool isInteracted;
//     
//     public GameObject player;
//     
//     private SpriteMovement spriteMovement;
//     
//     private MenuManager MenuManagerScript;
//     
//     public GameObject NPCMenu;
//     
//     private NPCMenu NPCManagerScript;
//
//     public GameObject Room;
//     
//     public GameObject NPC;
//
//     private CanvasGroup RoomCanvas;
//
//     private CanvasGroup NPCCanvas;
//
//     public bool NPCGone;
//
//     public RoomDetails Details;
//     
//     public virtual void Interact()
//     {
//         Debug.Log("Interacting with the object right now");
//
//         Room.SetActive(true);
//
//         //Change the image of the room that is being displayed based on a passed in parameter
//         RoomCanvas.GetComponent<Image>().sprite = Details.RoomImage;
//
//         if (!NPCGone)
//         {
//             NPCCanvas.GetComponent<Image>().sprite = Details.NPCImage;
//         }
//         
//         
//         Debug.Log("interact");
//
//         //When an NCPC is interacted with, fade in the room
//         if (RoomCanvas.alpha == 0){
//             isInteracted = true;
//             NPCManagerScript.exit = false;
//             StartCoroutine(FadeIn());
//             Debug.Log("Set true");
//             NPCManagerScript.inMenu = true;
//         }
//     }
//
//     //Fade in the room
//     public IEnumerator FadeIn()
//     {
//         //If the NPC is not gone, set it as active then fade it in
//         if(!NPCGone){
//             NPC.SetActive(true);
//         }
//
//         Debug.Log("Fading In");
//
//         isInteracted = true;
//
//         //Lock the movement of the player
//         spriteMovement.lockMovement = true;
//         spriteMovement.LockMovement();
//         Debug.Log("Player movement locked");
//         
//         if (!NPCGone)
//         {
//             NPCManagerScript.RoomNPC = RoomNPC.GetComponent<RoomSprite>();
//             NPCManagerScript.currNPCGone = false;
//         }
//         else
//         {
//             NPCManagerScript.currNPCGone = true;
//         }
//         
//         MenuManagerScript.setNPCMenu(true);
//
//         while (RoomCanvasGroup.alpha < 1)
//         {
//             RoomCanvasGroup.alpha += Time.deltaTime;
//
//             if(!NPCGone){
//                 NPCCanvasGroup.alpha += Time.deltaTime;
//             }
//             yield return null;
//         }
//     }
//
//     //Fade out the room
//     public IEnumerator FadeOut(MenuManager menuManager, CanvasGroup roomCanvasGroup, CanvasGroup npcCanvasGroup,
//         GameObject room, GameObject roomNPC)
//     {
//         Debug.Log("Fading out");
//
//         isInteracted = false;
//
//         //Unlock the movement of the player
//         spriteMovement.lockMovement = false;
//         Debug.Log("Player movement unlocked");
//
//         menuManager.setNPCMenu(false);
//         
//         Debug.Log(roomCanvasGroup.GetInstanceID());
//
//         while (roomCanvasGroup.alpha > 0)
//         {
//             roomCanvasGroup.alpha -= Time.deltaTime;
//             
//             Debug.Log(Time.deltaTime);
//
//             if(!NPCGone){
//                 npcCanvasGroup.alpha -= Time.deltaTime;
//             }
//             yield return null;
//         }
//         
//
//         room.SetActive(false);
//
//         if(!NPCGone){
//             roomNPC.SetActive(false);
//         }
//
//         fadeOut = null;
//     }
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         MenuManagerScript = GameMenuCanvas.GetComponent<MenuManager>();
//
//         NPCManagerScript = NPCMenu.GetComponent<NPCMenu>();
//
//         spriteMovement = player.GetComponent<SpriteMovement>();
//
//         NPCGone = false;
//     }
//
//     void Update()
//     {
//         //Check to see if the exit of the menu option was selected, if so, close the menu and fadeout 
//         if(NPCManagerScript.exit == true && _faded){
//             if (fadeOut != null)
//             {
//                 StopCoroutine(fadeOut);
//             }
//             fadeOut = StartCoroutine(FadeOut(MenuManagerScript, RoomCanvasGroup, NPCCanvasGroup, Room, RoomNPC));
//             NPCManagerScript.exit = false;
//             NPCManagerScript.inMenu = true;
//         }
//     }
// }