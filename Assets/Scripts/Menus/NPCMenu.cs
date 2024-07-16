using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class NPCMenu : MonoBehaviour
{
    public Dialouge emptyDialouge; // Reference to the Dialouge script
    public bool hasInteracted;
    public NPCMenu thisNpcMenu;
    public ActionManagerScript actionManager;
    public TextMeshProUGUI apText;
    public int activeOption = 0;
    public int numOptions = 4;
    public CanvasGroup RoomCanvasGroup;
    public GameObject RoomNPC;
    public Image[] optionPanels;
    public bool exit;
    public bool hasDecided;
    

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    // For NPCmenu, vanish menu, for pause menu, this will be replaced by GoToScene
    public void ExitMenu() {
        exit = true; 
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
                if(hasDecided){
                    EmptyInteract();
                }
                else{
                    RoomNPC.GetComponent<RoomSprite>().Interact();
                }
                break;
            case 2:
                // For NPC, Decide
                if (hasDecided) {
                    EmptyInteract();
                }
                else{
                    RoomNPC.GetComponent<RoomSprite>().DecideInteract();
                    actionManager.DecrementAP();
                }
                break;
            case 3: 
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

    public virtual void EmptyInteract()
    {
        Debug.Log("Empty Interacting with NPC");

        if(!hasInteracted){
            FindAnyObjectByType<DialougeManager>().StartEmptyDialouge(emptyDialouge);
            hasInteracted = true;
        }

        else{
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if(FindAnyObjectByType<DialougeManager>().emptySentences.Count <= 0){
                hasInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                return;
            }
            else{
                FindAnyObjectByType<DialougeManager>().DisplayNextEmptySentence();
            }
        }
    }

    void Start() {  
        GenerateOptions();
        hasDecided = false;
        hasInteracted = false;
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
            byte aValue = 0;
            if (i == activeOption) {
                aValue = 255;
            } 
            optionPanels[i].color = new Color32(195, 118, 55, aValue);
        }


        int actionPoints = actionManager.ReturnActionPoints();
        
        apText.text = "Action Points: " + actionPoints.ToString();
        
    }
}
