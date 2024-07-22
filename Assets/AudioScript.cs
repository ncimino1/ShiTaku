using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    //Audio source variable
    public AudioSource source;
    public AudioClip clip_menu_move;
    public AudioClip clip_menu_select;
    public AudioClip background_music;
    public AudioClip wave;

    //Game objects and references
    public GameObject GameMenuCanvas;
    NPCMenu NpcMenu;
    MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = background_music;
        source.Play();
        source.loop = true;
        NpcMenu = FindObjectOfType<NPCMenu>();
        menuManager = FindObjectOfType<MenuManager>();
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
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && (NpcMenu.inMenu == true || menuManager.isPaused == true))
        {
            source.PlayOneShot(clip_menu_move);
        }

        if(menuManager.gameOver == true)
        {
            source.PlayOneShot(wave);
            menuManager.gameOver = false;
        }
    }
}
