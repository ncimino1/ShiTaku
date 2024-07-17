using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // finalDay holds the day number when Game Over is reached
    public int finalDay = 4;

    // References to all the menus to manipulate

    public NPCMenu npcMenu;
    public PauseMenu pauseMenu;

    public GameOverMenu gameOverMenu;

    public DaysDisplayer daysDisplayer;

    // References to the managers needed

    public ActionManagerScript actionManager;

    public PlayerManager playerManager;
    


    // We will need a way to get a string ID from the NPC in question.
    // For now I'll have a default function.
    // May need to make use of the player manager.

    public string GetNewAction() {
        string newAction = "0000";
        return newAction;
    }


    // Start is called before the first frame update

    
    void Start()
    {
        npcMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        daysDisplayer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Cityscape") {
            daysDisplayer.gameObject.SetActive(true);
            daysDisplayer.SetDayText();

        // Conditions for determining whether npcmenu is active can be determined after connection of parts
        // add && playerManager.InteractionNotification.activeSelf to the first if statement when ready
            
            
            if (Input.GetKeyDown(KeyCode.Q)) {
                npcMenu.gameObject.SetActive(true);
                npcMenu.SetAPandCostText();

                // GetNewAction will have to give an ID based on what object is nearby when interacting
                npcMenu.LoadAction(GetNewAction());
                // Maybe have npcMenu.LoadAction() also load the apCost here and have apCost be permanent
            } else if (Input.GetKeyDown(KeyCode.Z)) {
                pauseMenu.gameObject.SetActive(true);
                pauseMenu.SetAPText();
            } else if (actionManager.ReturnDays() == finalDay) {
                npcMenu.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(true);
            }
        }     
    }

}
