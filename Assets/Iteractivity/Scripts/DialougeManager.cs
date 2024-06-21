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

    void Start()
    {
        sentences = new Queue<string>();
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
    }
}
