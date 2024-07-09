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
    Animator anim; // Reference to the Animator component

    //Timer values to calulate the last time the NPC moved
    public float lastTimeMoved;
    public float currentTime;

    public bool shouldMove = true; //If the NPC should move or not

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

    //Function to generate a random direction for the NPC to move in for a max of 3 second
    public void RandomDirection(){
        if(currentTime - lastTimeMoved > 5){
            lastTimeMoved = currentTime;
            int randomDirection = Random.Range(0, 4);

            switch(randomDirection){
                case 0:
                    anim.SetBool("up", true);
                    anim.SetBool("down", false);
                    anim.SetBool("left", false);
                    anim.SetBool("right", false);
                    break;
                case 1:
                    anim.SetBool("down", true);
                    anim.SetBool("up", false);
                    anim.SetBool("left", false);
                    anim.SetBool("right", false);
                    break;
                case 2:
                    anim.SetBool("left", true);
                    anim.SetBool("up", false);
                    anim.SetBool("down", false);
                    anim.SetBool("right", false);
                    break;
                case 3:
                    anim.SetBool("right", true);
                    anim.SetBool("up", false);
                    anim.SetBool("down", false);
                    anim.SetBool("left", false);
                    break;
                default:
                    break;
            }
        }
    }

    //Function to stop the movement of the NPC
    void StopMovement(){
        Debug.Log("Stopping NPC");
        anim.SetBool("up", false);
        anim.SetBool("down", false);
        anim.SetBool("left", false);
        anim.SetBool("right", false);
    }

    //Function that actually moves the NPC based on the bool's set in the RandomDirection function
    public void Move(){
        Debug.Log("Moving NPC");

        if(anim.GetBool("up")){
            transform.Translate(Vector3.up * Time.deltaTime);
        }
        else if(anim.GetBool("down")){
            transform.Translate(Vector3.down * Time.deltaTime);
        }
        else if(anim.GetBool("left")){
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else if(anim.GetBool("right")){
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    
       
    }
    

    public void Start()
    {
        //Get the Animator component
        anim = GetComponent<Animator>();

        //Set the last time the NPC moved to the current time
        lastTimeMoved = Time.time;
    }

    //Set the bool's for movement
    void Update()
    {
        //If the NPC should move, call the Move function
        if(shouldMove){
            //Set the current time to the current time
            currentTime = Time.time;

            //Call the RandomDirection function to generate a random direction for the NPC to move in
            RandomDirection();

            Move();

            //Stop moving after a second
            if(currentTime - lastTimeMoved > 1){
                StopMovement();
            }
        }
    }
}
