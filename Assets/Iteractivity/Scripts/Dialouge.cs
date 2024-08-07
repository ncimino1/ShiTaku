using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialouge
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    [TextArea(3, 10)]
    public string[] exitSentences;

    [TextArea(3, 10)] 
    public string[] rebuildSentences;

    [TextArea(3, 10)] 
    public string[] rebuildDoneSetences;

    [TextArea(3, 10)]
    public string[] emptySentences;
}
