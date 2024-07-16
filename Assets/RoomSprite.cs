using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSprite : MonoBehaviour
{

    //Local variables
    public bool hasInteracted;
    public bool isImportant = true;

    //Reference to the other scripts
    public Dialouge dialouge; // Reference to the Dialouge script
    FadeInOut fade; // Reference to the FadeInOut script

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

    public virtual void EmptyInteract()
    {
        Debug.Log("Interacting with NPC");

        if(!hasInteracted){
            FindAnyObjectByType<DialougeManager>().StartEmptyDialouge(dialouge);
            hasInteracted = true;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().emptySentences.Count <= 0){
                hasInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                return;
            }
            else{
                FindAnyObjectByType<DialougeManager>().DisplayNextEmptySentence();
            }
        }
    }

    public virtual void DecideInteract(){
        Debug.Log("Decide with NPC");

        if(!hasInteracted){
            FindAnyObjectByType<DialougeManager>().StartEndDialouge(dialouge);
            hasInteracted = true;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().exitSentences.Count <= 0){
                hasInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();

                //If the NPC is important, despawn it after the dialouge is over
                if(isImportant){
                    StartCoroutine(LoadDespawn());
                }

                return;
            }
            else{
                FindAnyObjectByType<DialougeManager>().DisplayNextEndSentence();
            }
        }
    }

    IEnumerator LoadDespawn(){
        fade.Despawn();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Get the FadeInOut script
        fade = FindAnyObjectByType<FadeInOut>();
        isImportant = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
