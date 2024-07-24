using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameOverMenu : MonoBehaviour
{
    public PlayerManager playerManager;

    private static int finalScore;

    public TextMeshProUGUI scoreText;
    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void SetScoreText() {
        scoreText = GameObject.FindWithTag("FinalScore").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + finalScore.ToString();
    }

    void Start() {
        SetScoreText();
    }

    public void FinishGame() {
        finalScore = playerManager.GetFinalScore();
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

        scoreText.text = "Score: " + finalScore.ToString();
    }
}
