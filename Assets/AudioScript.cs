using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    //Audio source variable
    public AudioSource source;
    public AudioClip clip_menu_move;
    public AudioClip clip_menu_select;

    public GameObject GameMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            source.PlayOneShot(clip_menu_select);
        }


        //See if any of the chidren canvas of the GameMenuCanvas are active
        //If they are, play the sound
        foreach (Transform child in GameMenuCanvas.transform)
        {
            if (child.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) && child.gameObject.name != "DaysDisplayer - Outline")
            {
                source.PlayOneShot(clip_menu_move);
            }
        }
    }
}
