using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    Animator anim; // Reference to the Animator component

    public GameObject InteractionNotification;

    SpriteMovement spriteMovement;

    public Dialouge dialouge;
    
    public bool startDialouge = true;
    public bool startDialogueOver = false;

    public bool isInRoom = false;

    public Stack<int> accumScore; // Holds score gained from previous actions performed 

    public bool failOverride = false;
    public bool passOverride = false;

    public int highestScore;
    
    // Start is called before the first frame update to get the Animator component
    public void Start()
    {
        anim = GetComponent<Animator>();

        spriteMovement = GetComponent<SpriteMovement>();

        accumScore = new Stack<int>();

        //Tutorial dialouge
        FindAnyObjectByType<DialougeManager>().StartDialouge(dialouge);
    }

    //Update is called once per frame to check for key presses and trigger animations
    public void Update()
    {
        if (spriteMovement.lockMovement)
        {
            anim.SetBool("up", false);
            anim.SetBool("down", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.W)){
            anim.SetBool("up", true);
            anim.SetBool("down", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.S)){
            anim.SetBool("down", true);
            anim.SetBool("up", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.A)){
            anim.SetBool("left", true);
            anim.SetBool("down", false);
            anim.SetBool("up", false);
            anim.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.D)){
            anim.SetBool("right", true);
            anim.SetBool("down", false);
            anim.SetBool("left", false);
            anim.SetBool("up", false);
        }
        else{
            anim.SetBool("up", false);
            anim.SetBool("down", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }

        if(Input.GetKeyDown(KeyCode.E) && !isInRoom && !startDialogueOver){
            Interact();
        }
    }

    public virtual void Interact(){
        Debug.Log("Empty Interacting with NPC");

        if(startDialouge){
            FindAnyObjectByType<DialougeManager>().StartDialouge(dialouge);
            startDialouge = false;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().sentences.Count <= 0){
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                startDialogueOver = true;
                return;
            }
            else{
                FindAnyObjectByType<DialougeManager>().DisplayNextSentence();
            }
        }
    }

    public void EnterRoom(){
        Debug.Log("Player in room");
        isInRoom = true;
    }

    public void ExitRoom(){
        Debug.Log("Player out of room");
        isInRoom = false;
    }

    public void NotifyPlayer(){
        InteractionNotification.SetActive(true);
        Debug.Log("Player notified");
    }

    public void DeNotifyPlayer(){
        InteractionNotification.SetActive(false);
        Debug.Log("Player denotified");
    }

    public int GetFinalScore() {
        int finalScore = 0;
        while(accumScore.Count != 0) {
            finalScore = finalScore + accumScore.Pop();
        }

        return finalScore;
    }

    
}
