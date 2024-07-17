using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class NPCMenu : MonoBehaviour
{
    // References to this, the MenuManager, and ActionManager

    public NPCMenu thisNpcMenu;

    public MenuManager menuManager;

    public ActionManagerScript actionManager;

    // References to the textbox components on the top of npc menu

    public TextMeshProUGUI apText;

    public TextMeshProUGUI costText;


    /* 
        activeOption and numOptions are used to keep track of which menu option is ready
        to be selected.
    */
    public int activeOption = 0;
    public int numOptions = 4;

    /* 
       currentAction stores the string ID of the action type that gets loaded into it
       when interacting with an npc or object. With the ID, we can get the action details
       from ActionManager's actionList.
    */
    public string currentAction;

    /* 
        actionBenefit holds the reward of points we get if we do perform the action
        via the Decide button
    */

    public int actionBenefit;

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
                actionManager.UseAction(currentAction);
                currentAction = "";
                actionBenefit = 0;
                ExitMenu();
                break;
            case 3: 
                currentAction = "";
                actionBenefit = 0;
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

    public void SetAPandCostText() {
        apText = GameObject.FindWithTag("ActionPoints").GetComponent<TextMeshProUGUI>();
        costText = GameObject.FindWithTag("Cost").GetComponent<TextMeshProUGUI>();
    }

    public void SetCurrentAction(string newAction) {
        currentAction = newAction;
    }

    void Start() {  
        GenerateOptions();
        currentAction = "";
        actionBenefit = 0;
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

        int apCost = 0;

        if (actionManager.actionList.ContainsKey(currentAction)) {
            apCost = actionManager.actionList[currentAction][0];
        }

        costText.text = "Cost: " + apCost.ToString();
        
    }

    public void LoadAction(string actionId) {
        currentAction = actionId; // Makes sure NPC/Interactable holds the current action Type
        actionBenefit = actionManager.actionList[currentAction][1]; // Makes sure NPC/Interactable holds reward
    }
}
