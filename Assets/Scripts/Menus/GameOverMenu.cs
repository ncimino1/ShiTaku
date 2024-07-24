using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameOverMenu : MonoBehaviour
{
    public PlayerManager playerManager;


    public TextMeshProUGUI scoreText;
    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void SetScoreText() {
        scoreText = GameObject.FindWithTag("FinalScore").GetComponent<TextMeshProUGUI>();
    }

    void Start() {
        SetScoreText();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            GoToScene("MainMenuScene");
        }
    }

    public void FinishGame() {
        int finalScore = playerManager.GetFinalScore();
        int possible = playerManager.highestScore;
        
        float percent = 1.0f;
        if (possible != 0)
        {
            percent = finalScore / (float)possible;
        }

        if (percent > .70)
        {
            SceneManager.LoadScene("Pass Scene");
            // result = "Pass ";
        }
        else
        {
            SceneManager.LoadScene("Fail Scene");
            // result = "Fail ";
        }

        // scoreText.text = result + "Score: " + finalScore.ToString();

    }
}
