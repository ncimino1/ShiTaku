using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            GoToScene("MainMenuScene");
        }
    }
}
