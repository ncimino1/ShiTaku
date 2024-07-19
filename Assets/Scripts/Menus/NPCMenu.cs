using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;


public class NPCMenu : MonoBehaviour
{
    public Dialouge emptyDialouge; // Reference to the Dialouge script
    public bool hasInteracted;
    public ActionManagerScript actionManager;
    public TextMeshProUGUI apText;
    public int activeOption = 0;
    public int numOptions = 4;
    public Image[] optionPanels;

    public bool inMenu;  

    public bool currNPCGone;

    public GameObject CurrInteraction;

    //setup
    public RoomDetails Details;

    //static
    public GameObject Room;

    //static
    public GameObject NPC;

    //static
    public CanvasGroup RoomCanvas;

    //static
    public CanvasGroup _npcCanvas;

    //static
    public MenuManager MenuManagerScript;
    
    //static 
    public RoomSprite _roomSprite;

    //static
    public GameObject Player;

    //static
    public SpriteMovement _spriteMovement;
    
    public virtual void Interact()
    {
        if (RoomCanvas.alpha == 0)
        {
            _roomSprite.Details = Details;
            CurrInteraction.SetActive(false);
            StartCoroutine(FadeIn());
            inMenu = true;
        }
    }
    
    public IEnumerator FadeIn()
    {
        //If the NPC is not gone, set it as active then fade it in

        Debug.Log("Fading In");
        
        //Lock the movement of the player
        _spriteMovement.lockMovement = true;
        _spriteMovement.LockMovement();
        Debug.Log("Player movement locked");
        
        Room.SetActive(true);
        
        if(!Details.NPCResolved){
            NPC.SetActive(true);
            _roomSprite.Interact();
            currNPCGone = false;
        }
        else
        {
            currNPCGone = true;
        }
        
        MenuManagerScript.setNPCMenu(true);
        
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        while (RoomCanvas.alpha < 1)
        {
            RoomCanvas.alpha += Time.deltaTime;

            if(!Details.NPCResolved){
                _npcCanvas.alpha += Time.deltaTime;
            }
            yield return null;
        }
    }
    
    public IEnumerator FadeOut(MenuManager menuManager, CanvasGroup roomCanvasGroup, CanvasGroup npcCanvasGroup,
        GameObject room, GameObject roomNPC)
    {
        Debug.Log("Fading out");

        //Unlock the movement of the player
        _spriteMovement.lockMovement = false;
        Debug.Log("Player movement unlocked");
        
        _roomSprite.DeactivateDialogue();
        
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        
        while (roomCanvasGroup.alpha > 0)
        {
            roomCanvasGroup.alpha -= Time.deltaTime;
            
            if(!Details.NPCResolved){
                npcCanvasGroup.alpha -= Time.deltaTime;
            }
            yield return null;
        }
        
        room.SetActive(false);

        if(!Details.NPCResolved){
            roomNPC.SetActive(false);
        }
        
        CurrInteraction.SetActive(true);
        menuManager.setNPCMenu(false);
    }

    // For NPCmenu, vanish menu, for pause menu, this will be replaced by GoToScene
    public void ExitMenu() {
        activeOption = 0;
        StartCoroutine(FadeOut(MenuManagerScript, RoomCanvas, _npcCanvas, Room, NPC));
    }

    public void HandleScroll(bool IsDown) {
        int nextOption = activeOption + (IsDown ? 1 : -1);
        if (nextOption >= 0 && nextOption < numOptions) {
            activeOption = nextOption;
        }
    }

    public void HandleSelection() {
        //Try to see if the npc has already decided, if they have, then they can't interact again; If they arent there should be an error when trying
        //to interact with them
        // try{
        //     RoomNPC =  FindAnyObjectByType<RoomSprite>();
        //     currNPCGone = false;
        // }
        // catch(Exception ex){
        //     currNPCGone = true;
        // }


        switch (activeOption) {
            case 0: 
                // For NPC, Investigate

                break;
            case 1:
                // For NPC, Interact
                if(currNPCGone){
                    EmptyInteract();
                }
                else{
                    _roomSprite.Interact();
                }
                break;
            case 2:
                // For NPC, Decide
                // if (RoomNPC.GetComponent<RoomSprite>().hasDecided) {
                if(currNPCGone) {
                    EmptyInteract();
                }
                else{
                    _roomSprite.DecideInteract();
                    // actionManager.DecrementAP();
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
        hasInteracted = false;
    }

    void Update() {
        // Updates Main Menu based on the current Key Input, use switch statement to determine.

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            HandleScroll(true);
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleScroll(false);
        } else if (Input.GetKeyDown(KeyCode.E) && inMenu) {
            Debug.Log("here");
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
