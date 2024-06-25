using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DaysDisplayer : MonoBehaviour
{
    public ActionManagerScript actionManager;

    public TextMeshProUGUI apText;

    public void SetDayText() {
        apText = GameObject.FindWithTag("Days").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int days = actionManager.ReturnDays();
        
        apText.text = "Days: " + days.ToString();
    }
}
