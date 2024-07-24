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

    // public bool hasInteracted;
    public ActionManagerScript actionManager;

    public TextMeshProUGUI apText;

    public TextMeshProUGUI costText;


    /*
        activeOption and numOptions are used to keep track of which menu option is ready
        to be selected.
    */
    public int activeOption = 0;
    public int numOptions = 4;
    public Image[] optionPanels;

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
    public bool inMenu;

    public GameObject CurrInteraction;

    public Tile CurrentTile;

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

    //Sounds
    public AudioSource source;
    public AudioClip door_clip;

    //static
    public SpriteMovement _spriteMovement;

    private Coroutine start;
    private Coroutine stop;

    public virtual void Interact()
    {
        if (RoomCanvas.alpha == 0)
        {
            _roomSprite.Details = Details;
            RoomCanvas.GetComponent<Image>().sprite = Details.RoomImage;
            _npcCanvas.GetComponent<Image>().sprite = Details.NPCImage;
            CurrInteraction.SetActive(false);
            inMenu = true;
            start = StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeIn()
    {
        //If the NPC is not gone, set it as active then fade it in

        //Play the sound
        source.PlayOneShot(door_clip);

        Debug.Log("Fading In");

        //Lock the movement of the player
        _spriteMovement.LockMovement();
        Debug.Log("Player movement locked");

        Room.SetActive(true);

        if (!Details.NPCResolved)
        {
            NPC.SetActive(true);
            _roomSprite.Interact();
            _npcCanvas.alpha = 0;
        }
        else
        {
            Debug.Log("gone");
        }

        MenuManagerScript.setNPCMenu(true);

        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        while (RoomCanvas.alpha < 1)
        {
            RoomCanvas.alpha += Time.deltaTime;

            if (!Details.NPCResolved)
            {
                _npcCanvas.alpha += Time.deltaTime;
            }

            yield return null;
        }
    }

    public IEnumerator FadeOut(MenuManager menuManager, CanvasGroup roomCanvasGroup, CanvasGroup npcCanvasGroup,
        GameObject room, GameObject roomNPC)
    {
        Debug.Log("Fading out");

        //Play the sound
        source.PlayOneShot(door_clip);

        inMenu = false;

        _roomSprite.DeactivateDialogue();

        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        while (roomCanvasGroup.alpha > 0)
        {
            roomCanvasGroup.alpha -= Time.deltaTime;

            if (!Details.NPCResolved)
            {
                npcCanvasGroup.alpha -= Time.deltaTime;
            }

            yield return null;
        }

        room.SetActive(false);

        if (!Details.NPCResolved)
        {
            roomNPC.SetActive(false);
        }

        CurrInteraction.SetActive(true);
        menuManager.setNPCMenu(false);

        //Unlock the movement of the player
        _spriteMovement.UnlockMovement();
        Debug.Log("Player movement unlocked");
    }

    // For NPCmenu, vanish menu, for pause menu, this will be replaced by GoToScene
    public void ExitMenu()
    {
        activeOption = 0;
        if (start != null)
        {
            StopCoroutine(start);
        }

        stop = StartCoroutine(FadeOut(MenuManagerScript, RoomCanvas, _npcCanvas, Room, NPC));
    }

    public void HandleScroll(bool IsDown)
    {
        int nextOption = activeOption + (IsDown ? 1 : -1);
        if (nextOption >= 0 && nextOption < numOptions)
        {
            activeOption = nextOption;
        }
    }

    public void HandleSelection()
    {
        //Try to see if the npc has already decided, if they have, then they can't interact again; If they arent there should be an error when trying
        //to interact with them
        // try{
        //     RoomNPC =  FindAnyObjectByType<RoomSprite>();
        //     currNPCGone = false;
        // }
        // catch(Exception ex){
        //     currNPCGone = true;
        // }
        switch (activeOption)
        {
            case 0:
                if (!Details.NPCResolved)
                {
                    if (CurrentTile.Destroyed)
                    {
                        LoadAction("0001");
                        _roomSprite.RebuildInteract();
                    }
                    else
                    {
                        _roomSprite.StableInteract();
                    }
                }

                break;
            case 1:
                // For NPC, Interact
                if (Details.NPCResolved)
                {
                    EmptyInteract();
                }
                else
                {
                    _roomSprite.Interact();
                }

                break;
            case 2:
                // For NPC, Decide
                // if (RoomNPC.GetComponent<RoomSprite>().hasDecided) {
                if (Details.NPCResolved || Details.RebuildResolved)
                {
                    EmptyInteract();
                }
                else
                {
                    LoadAction("0002");
                    _roomSprite.DecideInteract();
                    // actionManager.DecrementAP();
                }

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

    public void GenerateOptions()
    {
        optionPanels = new Image[numOptions];
        List<string> availableOptions = new List<string>();
        // Put in available option strings here
        availableOptions.Add("Investigate");
        availableOptions.Add("Interact");
        availableOptions.Add("Decide");
        availableOptions.Add("Leave");

        for (int i = 0; i < numOptions; i++)
        {
            GameObject currentObject = GameObject.FindWithTag(availableOptions[i]);
            optionPanels[i] = currentObject.GetComponent<Image>();
        }
    }

    public void SetAPandCostText()
    {
        apText = GameObject.FindWithTag("ActionPoints").GetComponent<TextMeshProUGUI>();
        costText = GameObject.FindWithTag("Cost").GetComponent<TextMeshProUGUI>();
    }

    public void SetCurrentAction(string newAction)
    {
        currentAction = newAction;
    }

    public virtual void EmptyInteract()
    {
        Debug.Log("Empty Interacting with NPC");

        if (!Details.HasInteracted)
        {
            FindAnyObjectByType<DialougeManager>().StartEmptyDialouge(emptyDialouge);
            Details.HasInteracted = true;
        }

        else
        {
            //Check to see if there is a next sentence, if there is display it. Else end the dialouge
            if (FindAnyObjectByType<DialougeManager>().emptySentences.Count <= 0)
            {
                Details.HasInteracted = false;
                FindAnyObjectByType<DialougeManager>().EndDialouge();
                return;
            }
            else
            {
                FindAnyObjectByType<DialougeManager>().DisplayNextEmptySentence();
            }
        }
    }

    void Start()
    {
        GenerateOptions();
        currentAction = "";
        actionBenefit = 0;
    }

    void Update()
    {
        // Updates Main Menu based on the current Key Input, use switch statement to determine.

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleScroll(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleScroll(false);
        }
        else if (Input.GetKeyDown(KeyCode.E) && inMenu)
        {
            Debug.Log("here");
            HandleSelection();
        }


        // Write code to update the outlines for the options in the options panel

        for (int i = 0; i < numOptions; i++)
        {
            optionPanels[i].enabled = (i == activeOption);
        }

        int actionPoints = actionManager.ReturnActionPoints();

        apText.text = "Action Points: " + actionPoints.ToString();

        int apCost = 0;

        if (actionManager.actionList.ContainsKey(currentAction))
        {
            apCost = actionManager.actionList[currentAction][0];
        }

        costText.text = "Cost: " + apCost.ToString();
    }

    public void LoadAction(string actionId)
    {
        currentAction = actionId; // Makes sure NPC/Interactable holds the current action Type
        actionBenefit = actionManager.actionList[currentAction][1]; // Makes sure NPC/Interactable holds reward
    }
}