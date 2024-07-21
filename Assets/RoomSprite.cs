using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSprite : MonoBehaviour
{

    //Local variables
    public bool isImportant = true;

    //Reference to the other scripts
    FadeInOut fade; // Reference to the FadeInOut script
    NPCMenu npcMenu; //Reference to the NPCMenu script
    public GameObject interact;
    public RoomDetails Details;

    private DialougeManager _manager;

    public virtual void Interact()
    {
        Debug.Log("Interacting with NPC");
        Details.HasDecideInteracted = false;
        Details.HasRebuildInteracted = false;

        if(!Details.HasInteracted){
            FindAnyObjectByType<DialougeManager>().StartDialouge(Details.NPCDialouge);
            Details.HasInteracted = true;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().sentences.Count <= 0){
                Details.HasInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                return;
            }
            else{
                _manager.TurnOnBox();
                _manager.DisplayNextSentence();
            }
        }
    }

    // public virtual void EmptyInteract()
    // {
    //     Debug.Log("Empty Interacting with NPC");

    //     if(!hasInteracted){
    //         FindAnyObjectByType<DialougeManager>().StartEmptyDialouge(dialouge);
    //         hasInteracted = true;
    //     }

    //     else{
    //         //Check to see if there is a next sentence, if there is display it. Else end the dialouge
    //         if(FindAnyObjectByType<DialougeManager>().emptySentences.Count <= 0){
    //             hasInteracted = false;
    //             FindAnyObjectByType<DialougeManager>().EndDialouge();
    //             return;
    //         }
    //         else{
    //             FindAnyObjectByType<DialougeManager>().DisplayNextEmptySentence();
    //         }
    //     }
    // }

    public void RebuildInteract()
    {
        Details.HasInteracted = false;
        Details.HasDecideInteracted = false;

        if (!Details.HasRebuildInteracted)
        {
            FindAnyObjectByType<DialougeManager>().StartRebuildDialogue(Details.NPCDialouge);
            Details.HasRebuildInteracted = true;
        }
        else
        {
            if (FindAnyObjectByType<DialougeManager>().rebuildSentences.Count <= 0)
            {
                Details.HasRebuildInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                
                npcMenu.actionManager.UseAction(npcMenu.currentAction);
                npcMenu.currentAction = "";
                npcMenu.actionBenefit = 0;
                npcMenu.CurrentTile.Destroyed = false;
                npcMenu.CurrentTile.GetComponent<SpriteRenderer>().sprite = npcMenu.CurrentTile.FixedAsset;

                return;
            }
            else
            {
                FindAnyObjectByType<DialougeManager>().DisplayNextRebuildSentence();
            }
        }
    }

    public virtual void DecideInteract(){
        Debug.Log("Decide with NPC");
        Debug.Log(Details.HasDecideInteracted);
        
        Debug.Log(Details.NPCDialouge.name);

        Details.HasInteracted = false;
        Details.HasRebuildInteracted = false;

        if(!Details.HasDecideInteracted){
            FindAnyObjectByType<DialougeManager>().StartEndDialouge(Details.NPCDialouge);
            Details.HasDecideInteracted = true;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().exitSentences.Count <= 0){
                Details.HasDecideInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();

                //If the NPC is important, despawn it after the dialouge is over
                if(isImportant){
                    npcMenu.actionManager.UseAction(npcMenu.currentAction);
                    npcMenu.currentAction = "";
                    npcMenu.actionBenefit = 0;
                    Details.NPCResolved = true;
                    StartCoroutine(LoadDespawn());
                }

                return;
            }
            else{
                FindAnyObjectByType<DialougeManager>().DisplayNextEndSentence();
            }
        }
    }

    public void DeactivateDialogue()
    {
        _manager.TurnOffBox();
    }

    IEnumerator LoadDespawn(){
        fade.Despawn();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        DeactivateDialogue();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Get the FadeInOut script
        fade = FindAnyObjectByType<FadeInOut>();
        npcMenu = FindAnyObjectByType<NPCMenu>();
        isImportant = true;
        _manager = FindAnyObjectByType<DialougeManager>();
    }
}
