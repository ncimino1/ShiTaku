using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    Animator anim; // Reference to the Animator component

    public GameObject InteractionNotification;

    public bool isInRoom = false;

    public Stack<int> accumScore; // Holds score gained from previous actions performed 
    
    // Start is called before the first frame update to get the Animator component
    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Update is called once per frame to check for key presses and trigger animations
    public void Update()
    {
        if (Input.GetKey(KeyCode.W)){
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

    
}
