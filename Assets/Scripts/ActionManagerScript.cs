using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionManagerScript : MonoBehaviour
{
    public CityGridGenerator cityGen;
    public PlayerManager playerManager;
    /* 
        ActionCounter is the variable that holds the current number
        of action points available.
    */
    public int actionCounter;


    /* 
        DayCounter keeps track of actual time. A new day signifies a 
        transition in time.
    */
    public int dayCounter = 1;

    private bool _actionsLoaded = false;
    
    /* 
        ActionList is a list of String IDs of actions that are 
        currently available. 
        May want to convert list into a hastable with String Keys for the IDs
        and int values for their respective costs. 
    */

    /* New actionList variable, has the ID as a string key. The value is an integer array
       with 3 values: Cost, Benefit, and Detriment. Benefit is the points you get for 
       choosing to do the action. Deteriment is the points you lose for choosing to not
       do the action.
    */
    public SortedDictionary<string, int[]> actionList = new SortedDictionary<string, int[]>();
    
    // Start is called before the first frame update
    void Start()
    {
        actionCounter = 5;
        // Initialize all types of actions
        GenerateActions();
        // Initialize Count for all types of Actions
    }

    // Update is called once per frame
    void Update()
    {
        if (actionCounter == 0 || actionCounter < GetMinActionPoints() || actionList.Count == 0) {
            ResetActionPoints();

            // Do Time Transition
            dayCounter = dayCounter + 1;
            Debug.Log("New Transition.");
        }

        if (!_actionsLoaded && cityGen.genDone)
        {
            LoadAllActions();
            _actionsLoaded = true;
        }
    }

    // Two Test Functions to test the functionality of UseAction

    [ContextMenu("UseActionSuccessTest")]
    void UseActionSuccessTester() {
        string topID = "0000";
        UseAction(topID);
    }

    [ContextMenu("UseActionFailureTest")]
    void UseActionFailureTest() {
        string topID = "0005";
        UseAction(topID);
    }


     // Function for action point usage
    public void UseAction(string actionID = "0000") 
    {

        if (actionList.ContainsKey(actionID)) {
            if (actionList[actionID][0] <= actionCounter) {
                actionCounter -= actionList[actionID][0];

                playerManager.accumScore.Push(actionList[actionID][1]);
                // return a message that says action allowed
                Debug.Log("Action Allowed and Successful.");
            } else {
                Debug.Log("Action not Successful");
                // return a message that says action NOT allowed
            }
        } else {

            Debug.Log("Action not Successful");
            // return a message that says action NOT allowed
        }
    }

    // Function that resets action points when in time transistion
    [ContextMenu("ResetActionPoints")]
    public void ResetActionPoints() {
        Debug.Log("Actions resetted.");
        actionCounter = 5;
    }

    // Function that finds the smallest cost among all actions available
    // For the demo, we will have it return 1.
    int GetMinActionPoints() {
        
        // Need to implement a min value search algorithm post demo 
        return 1;
    }

    // Function that Generates a new set of action IDs for actions during the next day
    /* 
        Just adds in two predefined actions for the demo sake, will be updated to generate
        a new random set.
    */
    [ContextMenu("GenerateActions")]
    public void GenerateActions() {
        // Flush out all the remaining actions from ActionList
        actionList.Clear();

        actionList.Add("0000", new int[] {1, 2, 0}); // Talk 
        actionList.Add("0001", new int[] {3, 2, 0}); // House Rebuild
        actionList.Add("0002", new int[] {2, 2, 0}); // House Evacuate
        actionList.Add("0003", new int[] {4, 4, 0}); // Skyscraper Rebuild
        actionList.Add("0004", new int[] {4, 2, 0}); // Skyscraper Evacuate
        actionList.Add("0005", new int[] {2, 1, 0}); // Shrine Rebuild
        actionList.Add("0006", new int[] {2, 2, 0}); // Shrine Evacuate
        actionList.Add("0007", new int[] {3, 2, 0}); // Hardware Rebuild
        actionList.Add("0008", new int[] {2, 2, 0}); // Hardware Evacuate
        actionList.Add("0009", new int[] {3, 2, 0}); // City Hall Rebuild
        actionList.Add("0010", new int[] {2, 2, 0}); // City Hall Evacuate
        actionList.Add("0011", new int[] {3, 5, 0}); // Fire Station Rebuild
        actionList.Add("0012", new int[] {2, 0, 0}); // Fire Station Evacuate
        actionList.Add("0013", new int[] {3, 3, 0}); // Police Rebuild
        actionList.Add("0014", new int[] {2, 0, 0}); // Police Evacutate
        Debug.Log("New Actions Generated.");
    }

    

    public int ReturnActionPoints() {
        return actionCounter;
    }

    public int ReturnDays() {
        return dayCounter;
    }

    public void DecrementAP() {
        actionCounter -= 1;
    }

    public void LoadAllActions() { // All methods in cityGen should return an int
        
        actionList["0000"][2] = cityGen.GetTalkNpcCount();

        actionList["0001"][2] = cityGen.GetHouseRebuildCount();

        actionList["0002"][2] = cityGen.GetHouseCount();

        actionList["0003"][2] = cityGen.GetSkyscraperRebuildCount();

        actionList["0004"][2] = cityGen.GetSkyscraperCount();

        actionList["0005"][2] = cityGen.GetShrineRebuildCount();

        actionList["0006"][2] = cityGen.GetShrineCount();

        actionList["0007"][2] = cityGen.GetHardwareRebuildCount();

        actionList["0008"][2] = cityGen.GetHardwareCount();

        actionList["0009"][2] = cityGen.GetCityHallRebuildCount();

        actionList["0010"][2] = cityGen.GetCityHallCount();

        actionList["0011"][2] = cityGen.GetFireStationRebuildCount();

        actionList["0012"][2] = cityGen.GetFireStationCount();

        actionList["0013"][2] = cityGen.GetPoliceStationRebuildCount();

        actionList["0014"][2] = cityGen.GetPoliceStationCount();
    }
}
