using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip_menu_move;
    public AudioClip clip_menu_select;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            source.PlayOneShot(clip_menu_select);
        }


        //See if any of the chidren canvas of the GameMenuCanvas are active
        //If they are, play the sound
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            source.PlayOneShot(clip_menu_move);
        }
    }
}
