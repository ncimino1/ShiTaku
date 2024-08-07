using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialougeManager : MonoBehaviour
{

    //UI Elements to display dialouge
    public TextMeshProUGUI nameText; //Name of the NPC
    public TextMeshProUGUI dialougeText; //Dialouge text

    public GameObject dialougeBox; //UI element that contains the dialouge

    public Queue<string> sentences; //Queue to store the dialouge sentences

    public Queue<string> exitSentences; //Queue to store the dialouge sentences for evacuation

    public Queue<string> emptySentences; //Queue to store the dialouge sentences for emptying

    public Queue<string> rebuildSentences;

    public Queue<string> rebuildDoneSentences;

    void Start()
    {
        sentences = new Queue<string>();
        exitSentences = new Queue<string>();
        emptySentences = new Queue<string>();
        rebuildSentences = new Queue<string>();
        rebuildDoneSentences = new Queue<string>();
    }

    public void StartEndDialouge(Dialouge dialouge)
    {
        Debug.Log("Starting end dialouge with " + dialouge.name);

        dialougeBox.SetActive(true);

        nameText.text = dialouge.name;

        exitSentences.Clear();

        foreach (string sentence in dialouge.exitSentences)
        {
            exitSentences.Enqueue(sentence);
        }

        DisplayNextEndSentence();
    }

    public void DisplayNextEndSentence()
    {
        if (exitSentences.Count == 0)
        {
            Debug.Log("End of dialouge");
            EndDialouge();
            dialougeBox.SetActive(false);
            return;
        }

        string sentence = exitSentences.Dequeue();
        dialougeText.text = sentence;
        Debug.Log(sentence);
    }

    public void StartDialouge(Dialouge dialouge)
    {
        Debug.Log("Starting dialouge with " + dialouge.name);

        dialougeBox.SetActive(true);

        nameText.text = dialouge.name;

        sentences.Clear();

        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);

        }

         DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            Debug.Log("End of dialouge");
            EndDialouge();
            dialougeBox.SetActive(false);
            return;
        }

        string sentence = sentences.Dequeue();
        dialougeText.text = sentence;
        Debug.Log(sentence);
    }

    public void StartRebuildDialogue(Dialouge dialouge)
    {
        dialougeBox.SetActive(true);

        nameText.text = dialouge.name;
        
        rebuildSentences.Clear();

        foreach (var sentence in dialouge.rebuildSentences)
        {
            rebuildSentences.Enqueue(sentence);
        }
        
        DisplayNextRebuildSentence();
    }

    public void DisplayNextRebuildSentence()
    {
        if (rebuildSentences.Count == 0)
        {
            EndDialouge();
            dialougeBox.SetActive(false);
            return;
        }

        string sentence = rebuildSentences.Dequeue();
        dialougeText.text = sentence;
    }

    public void StartBuildDoneDialogue(Dialouge dialouge)
    {
        dialougeBox.SetActive(true);

        nameText.text = "You Whisper To Yourself";
        
        rebuildDoneSentences.Clear();

        foreach (var sentence in dialouge.rebuildDoneSetences)
        {
            rebuildDoneSentences.Enqueue(sentence);
        }
        
        DisplayNextDoneSentence();
    }

    public void DisplayNextDoneSentence()
    {
        if (rebuildDoneSentences.Count == 0)
        {
            EndDialouge();
            dialougeBox.SetActive(false);
            return;
        }

        string sentence = rebuildDoneSentences.Dequeue();
        dialougeText.text = sentence;
    }

    public void TurnOffBox()
    {
        dialougeBox.SetActive(false);
    }

    public void TurnOnBox()
    {
        dialougeBox.SetActive(true);
    }

    public void EndDialouge()
    {
        Debug.Log("End of dialouge");
        TurnOffBox();
        sentences.Clear();
        exitSentences.Clear();
    }

    public void StartEmptyDialouge(Dialouge dialouge)
    {        
        emptySentences = new Queue<string>();

        Debug.Log("Starting dialouge with " + dialouge.name);

        dialougeBox.SetActive(true);

        nameText.text = dialouge.name;

        emptySentences.Clear();

        foreach (string sentence in dialouge.emptySentences)
        {
            Debug.Log("Adding sentence to emptySentences");
            emptySentences.Enqueue(sentence);

        }

        Debug.Log("Displaying next empty sentence");    
         DisplayNextEmptySentence();
    }

    public void DisplayNextEmptySentence()
    {
        if (emptySentences.Count == 0)
        {
            Debug.Log("End of dialouge");
            EndDialouge();
            dialougeBox.SetActive(false);
            return;
        }

        string sentence = emptySentences.Dequeue();
        dialougeText.text = sentence;
        Debug.Log(sentence);
    }

}
