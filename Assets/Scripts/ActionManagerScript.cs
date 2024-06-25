using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionManagerScript : MonoBehaviour
{
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
    
    /* 
        ActionList is a list of String IDs of actions that are 
        currently available. 
        May want to convert list into a hastable with String Keys for the IDs
        and int values for their respective costs. 
    */
    public List<string> actionList = new List<string>();
    
    // Start is called before the first frame update
    void Start()
    {
        actionCounter = 5;
        GenerateActions();
        // Add elements to ActionList
    }

    // Update is called once per frame
    void Update()
    {
        if (actionCounter == 0 || actionCounter < GetMinActionPoints() || actionList.Count == 0) {
            ResetActionPoints();
            GenerateActions();

            // Do Time Transition
            dayCounter = dayCounter + 1;
            Debug.Log("New Transition.");
        }

    }

    // Two Test Functions to test the functionality of UseAction

    [ContextMenu("UseActionSuccessTest")]

    void UseActionSuccessTester() {
        string topID = actionList[0];
        UseAction(topID, 1);
    }

    [ContextMenu("UseActionFailureTest")]
    void UseActionFailureTest() {
        string topID = "0005";
        UseAction(topID, 1);
    }


     // Function for action point usage
    public void UseAction(string actionID = "0000", int actionPointCost = 1) 
    {
        int idIndex = actionList.FindIndex(x => x.Equals(actionID));

        if (actionPointCost <= actionCounter && idIndex != -1) {
            actionCounter = actionCounter - actionPointCost;
            actionList.Remove(actionID);

            
            // return a message that says action allowed
            Debug.Log("Action Allowed and Successful.");
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

        actionList.Add("0000");
        actionList.Add("0001");
        Debug.Log("New Actions Generated.");
    }

    public int ReturnActionPoints() {
        return actionCounter;
    }

    public int ReturnDays() {
        return dayCounter;
    }
}
