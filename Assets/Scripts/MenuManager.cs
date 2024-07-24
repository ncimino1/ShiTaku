using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // finalDay holds the day number when Game Over is reached
    public int finalDay;

    // References to all the menus to manipulate

    public NPCMenu npcMenu;
    public PauseMenu pauseMenu;

    public GameOverMenu gameOverMenu;

    public DaysDisplayer daysDisplayer;

    public CameraFollow cameraFollow;

    // References to the managers needed

    public ActionManagerScript actionManager;

    public PlayerManager playerManager;

    private bool _calculatedScore;
    
    public Transform waveSprite;

    public GameObject waveObject;

    public GameObject Player;

    public bool moveWave = false;
    public bool waveDone = false;

    //Audio
    public AudioSource source;
    public AudioClip menu_appear_clip;
    public AudioClip wave;
    public bool isPaused = false;
    public bool gameOver = false;
    
    // We will need a way to get a string ID from the NPC in question.
    // For now I'll have a default function.
    // May need to make use of the player manager.

    public string GetNewAction() {
        string newAction = "0000";
        return newAction;
    }


    // Start is called before the first frame update

    public bool EnteredRoom = false;

    public void setNPCMenu(bool set) {
        npcMenu.gameObject.SetActive(set);
        if(set == true){
            EnteredRoom = true;
        }
        else{
            EnteredRoom = false;
        }
    }
    
    //Start is called before the first frame update
    void Start()
    {
        npcMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        daysDisplayer.gameObject.SetActive(false);

        _calculatedScore = false;
        isPaused = false;
        gameOver = false;
    }
    

    void updateWave()
    {
        waveSprite.position = new Vector2(Time.deltaTime * 30.0f + waveSprite.position.x, waveSprite.position.y);
        if (waveSprite.position.x > 200)
        {
            moveWave = false;
            waveDone = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Cityscape") {
            daysDisplayer.gameObject.SetActive(true);
            daysDisplayer.SetDayText();

        // Conditions for determining whether npcmenu is active can be determined after connection of parts
        // For Javi and Andrew, this is where you write your condition for the npcMenu to appear after interaction
            // if (Input.GetKeyDown(KeyCode.Q)) {
            //     npcMenu.gameObject.SetActive(true);
            //     npcMenu.SetAPText();
            // } else
            if (Input.GetKeyDown(KeyCode.Z) && !npcMenu.inMenu) {
                pauseMenu.gameObject.SetActive(true);
                source.PlayOneShot(menu_appear_clip);
                isPaused = true;
                pauseMenu.SetAPText();
            } else if (actionManager.ReturnDays() >= finalDay) {
                moveWave = true;
                waveObject.SetActive(true);
                Player.SetActive(false);
                cameraFollow.zoomOut();
            }

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift) && !moveWave)
            {
                actionManager.dayCounter = finalDay;
                playerManager.highestScore = 0;
            }

            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift) && !moveWave)
            {
                actionManager.dayCounter = finalDay;
                playerManager.highestScore = 1000;
            }

            if (moveWave)
            {
                updateWave();
            }

            if (waveDone)
            {
                npcMenu.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(false);
                gameOver = true;
                if (!_calculatedScore)
                {
                    gameOverMenu.FinishGame();
                    _calculatedScore = true;
                }
            }
        }     
    }

}
