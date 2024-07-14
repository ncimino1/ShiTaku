using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class NPCMenu : MonoBehaviour
{

    public NPCMenu thisNpcMenu;

    public ActionManagerScript actionManager;

    // public MenuManager menuManager;

    public TextMeshProUGUI apText;
    public int activeOption = 0;
    public int numOptions = 4;

    /* 
       currentAction stores the string ID of the action that gets loaded into it
       when interacting with an npc or object. With the ID, we can get the action
       from ActionManager's actionList.
    */
    public string currentAction;


    public Image[] optionPanels;

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    // For NPCmenu, vanish menu, for pause menu, this will be replaced by GoToScene
    public void ExitMenu() {
        activeOption = 0;
        Debug.Log("Menu is closed.");
        thisNpcMenu.gameObject.SetActive(false);
    }

    public void HandleScroll(bool IsDown) {
        int nextOption = activeOption + (IsDown ? 1 : -1);
        if (nextOption >= 0 && nextOption < numOptions) {
            activeOption = nextOption;
        }
    }

    public void HandleSelection() {
        switch (activeOption) {
            case 0: 
                // For NPC, Investigate
                break;
            case 1:
                // For NPC, Interact
                
                break;
            case 2:
                // For NPC, Decide
                actionManager.DecrementAP();
                break;
            case 3: 
                currentAction = "";
                ExitMenu();
                break;
            default:
                break;
        }
    }

    public void GenerateOptions() {
        optionPanels = new Image[numOptions];
        List<string> availableOptions = new List<string>();
        // Put in available option strings here
        availableOptions.Add("Investigate");
        availableOptions.Add("Interact");
        availableOptions.Add("Decide");
        availableOptions.Add("Leave");

        for (int i = 0; i < numOptions; i++) {
            GameObject currentObject = GameObject.FindWithTag(availableOptions[i]);
            optionPanels[i] = currentObject.GetComponent<Image>();
        }
    }

    public void SetAPText() {
        apText = GameObject.FindWithTag("ActionPoints").GetComponent<TextMeshProUGUI>();
    }

    public void SetCurrentAction(string newAction) {
        currentAction = newAction;
    }

    void Start() {  
        GenerateOptions();
        currentAction = "";
    }

    void Update() {
        // Updates Main Menu based on the current Key Input, use switch statement to determine.

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            HandleScroll(true);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            HandleScroll(false);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            HandleSelection();
        }


        // Write code to update the outlines for the options in the options panel

        for (int i = 0; i < numOptions; i++) {
            optionPanels[i].enabled = (i == activeOption);
        }


        int actionPoints = actionManager.ReturnActionPoints();
        
        apText.text = "Action Points: " + actionPoints.ToString();
        
    }
}
