using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTriangleScript : MonoBehaviour
{
    public int currentOption;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-70, 90, 0);
        currentOption = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int nextOption = currentOption;
        if (Input.GetKeyDown(KeyCode.W)) {
            // Call NPCMenu.Scroll with IsDown = false
        } else if (Input.GetKeyDown(KeyCode.S)) {
            // Call NPCMenu.Scroll with IsDown = true
        }
        if (nextOption != currentOption) {
            currentOption = nextOption;
            transform.position = new Vector3(-70, 90 - 30*currentOption, 0);
        }
    }
}
