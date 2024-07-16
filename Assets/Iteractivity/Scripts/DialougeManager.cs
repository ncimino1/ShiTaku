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

    void Start()
    {
        sentences = new Queue<string>();
        exitSentences = new Queue<string>();
        emptySentences = new Queue<string>();
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

    public void EndDialouge()
    {
        Debug.Log("End of dialouge");
        dialougeBox.SetActive(false);
        sentences.Clear();
        exitSentences.Clear();
    }

    public void StartEmptyDialouge(Dialouge dialouge)
    {
        Debug.Log("Starting dialouge with " + dialouge.name);

        dialougeBox.SetActive(true);

        nameText.text = dialouge.name;

        emptySentences.Clear();

        foreach (string sentence in dialouge.emptySentences)
        {
            emptySentences.Enqueue(sentence);

        }

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

        string sentence = sentences.Dequeue();
        dialougeText.text = sentence;
        Debug.Log(sentence);
    }

}
