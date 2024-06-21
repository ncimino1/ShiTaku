using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject InteractionNotification;

    public bool isInRoom = false;
    
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
