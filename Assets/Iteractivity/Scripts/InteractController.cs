using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractController : MonoBehaviour
{
    public bool isInteracted;
    //public GameObject interior;
    //public GameObject exterior;
    public GameObject player;
    public CanvasGroup RoomCanvasGroup;
    public string roomName;
    public GameObject GameMenuCanvas;
    private MenuManager MenuManagerScript;

    public virtual void Interact()
    {
        Debug.Log("Interacting with the object right now");

        // if (exterior.activeSelf)
        // {
        //     exterior.SetActive(false);
        //     interior.SetActive(true);
        //     player.transform.position = interior.transform.Find("Spawn").position;
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
                break;
            //Default to a white image
            default:
                RoomCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("White");
                break;
        }

        //When an NCPC is interacted with, fade in the room
        if (RoomCanvasGroup.alpha == 0)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    //Fade in the room
    public IEnumerator FadeIn()
    {
        MenuManagerScript.setNPCMenu(true);

        while (RoomCanvasGroup.alpha < 1)
        {
            RoomCanvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    //Fade out the room
    public IEnumerator FadeOut()
    {
        while (RoomCanvasGroup.alpha > 0)
        {
            RoomCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuManagerScript = GameMenuCanvas.GetComponent<MenuManager>();
    }
}