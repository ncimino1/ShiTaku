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
        //10% chance to move in a random direction
        int randomDirection = Random.Range(0, 5);

        switch(randomDirection){
            case 0:
                anim.SetBool("up", true);
                Move();
                break;
            case 1:
                anim.SetBool("down", true);
                Move();
                break;
            case 2:
                anim.SetBool("left", true);
                Move();
                break;
            case 3:
                anim.SetBool("right", true);
                Move();
                break;
            default:
                break;
        }
    }

    //Function to stop the movement of the NPC
    IEnumerator StopMovement(){
        yield return new WaitForSeconds(3);
        anim.SetBool("up", false);
        anim.SetBool("down", false);
        anim.SetBool("left", false);
        anim.SetBool("right", false);
    }

    //Function that actually moves the NPC based on the bool's set in the RandomDirection function
    public void Move(){
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
        anim = GetComponent<Animator>();
    }

    //Set the bool's for movement
    void Update()
    {
        //Call the RandomDirection function to generate a random direction for the NPC to move in
        RandomDirection();

        //Have the NPC move idle and stand still for a few seconds
        StartCoroutine(StopMovement());

    }
}
