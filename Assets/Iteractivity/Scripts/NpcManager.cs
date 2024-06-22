using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public bool isInteracted = false;
    public bool hasItem;
    public string itemName;
    public bool hasInteracted;
    public bool isImportant;

    public Dialouge dialouge;

    public virtual void Interact()
    {
        Debug.Log("Interacting with NPC");

        if(!hasInteracted){
            FindAnyObjectByType<DialougeManager>().StartDialouge(dialouge);
            hasInteracted = true;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().sentences.Count <= 0){
                hasInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                return;
            }
            else{
                FindAnyObjectByType<DialougeManager>().DisplayNextSentence();
            }

        }


        
    }
}
