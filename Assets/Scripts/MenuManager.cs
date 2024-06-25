using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public NPCMenu npcMenu;
    public PauseMenu pauseMenu;
    // Start is called before the first frame update

    
    void Start()
    {
        //npcMenu.SetAPText();
        //pauseMenu.SetAPText();
        npcMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Conditions for determining whether npcmenu is active can be determined after connection of parts
        // For Javi and Andrew, this is where you write your condition for the npcMenu to appear after interaction
        if (Input.GetKeyDown(KeyCode.Q)) {
            npcMenu.gameObject.SetActive(true);
            npcMenu.SetAPText();
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.SetAPText();
        }
    }
}
