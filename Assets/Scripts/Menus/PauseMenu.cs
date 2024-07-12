using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PauseMenu : MonoBehaviour
{
    public PauseMenu thisPauseMenu;
    public int activeOption = 0;
    public int numOptions = 2;

    public ActionManagerScript actionManager;

    public TextMeshProUGUI apText;

    public Image[] optionPanels;

    // Trigger options within the game
    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    // For NPCmenu, vanish menu, for pause menu, this will be replaced by GoToScene
    public void ExitMenu() {
        activeOption = 0;
        Debug.Log("Menu is closed.");
        thisPauseMenu.gameObject.SetActive(false);
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
                ExitMenu();
                break;
            case 1:
                GoToScene("MainMenuScene");
                break;
            default:
                break;
        }
    }

    public void GenerateOptions() {
        optionPanels = new Image[numOptions];
        List<string> availableOptions = new List<string>();
        // Put in available option strings here
        availableOptions.Add("ExitPause");
        availableOptions.Add("GoToMainMenu");

        for (int i = 0; i < numOptions; i++) {
            GameObject currentObject = GameObject.FindWithTag(availableOptions[i]);
            optionPanels[i] = currentObject.GetComponent<Image>();
        }
    }

    public void SetAPText() {
        apText = GameObject.FindWithTag("ActionPoints").GetComponent<TextMeshProUGUI>();
    }

    void Start() {  
        GenerateOptions();
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
            optionPanels[i].color = new Color32(255, 255, 255, aValue);
        }

        int actionPoints = actionManager.ReturnActionPoints();
        
        apText.text = "Action Points: " + actionPoints.ToString();
        
    }
}

