using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static bool gamePaused;
    public Canvas uiCanvas;
    public Canvas pauseCanvas;
    public Canvas finishCanvas;
    public Canvas failCanvas;
    public GameObject finishPortal;  // Initialized here because it is deactivated at the beginning
    public GameObject portalArrow;  // Initialized here because it is deactivated at the beginning
    public Text timeSpentText;
    public Text enemyLeftText;
    public Text goTeleporterText;
    public Text bestTimeText;
    public Text boostText;
    public Text boostNumberText;
    public Image boostBackgroundMeter;
    public Image currentBoostMeter;
    public String levelName;

    InGameMenuManager inGameMenuManager;
    bool levelFinished;
    bool levelFailed;
    bool canBePaused;
    float isInverted;  // 1 is false, -1 is true
    float enemiesLeft;
    float timer;
    float bestTime;

	void Awake () {
        Cursor.visible = false;
        gamePaused = false;
        canBePaused = true;
        levelFinished = false;
        inGameMenuManager = GameObject.FindGameObjectWithTag("InGameMenuManager").GetComponent<InGameMenuManager>();
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyLeftText.text = "Enemies Left: " + enemiesLeft.ToString();
        Time.timeScale = 1;  // This ensures the game runs with no bugs

        // If the controls are inverted, then set isInverted to -1 to reverse the axis
        if (PlayerPrefs.GetFloat("IsInverted", 0) == 1) {
            isInverted = -1;
        }
        else {
            isInverted = 1;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        // Stop time when game is paused, and bring up the pause menu
		if (!gamePaused && canBePaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))) {
            PauseGame();
        }
        else if (gamePaused && canBePaused && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))) {
            UnpauseGame();
        }

        // If the game isn't paused and we haven't finished the level, then continue incrementing the time
        if (!gamePaused && !levelFinished && !levelFailed) {
            timer += Time.deltaTime;
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timer);
            timeSpentText.text = "Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }
    }

    // Decreases enemy count by one
    // If the enemies are less than zero, then handle it here (so it runs once)
    public void DecreaseEnemies(float num) {
        enemiesLeft -= num;
        enemyLeftText.text = "Enemies Left: " + enemiesLeft.ToString();
        
        // TODO: Show an arrow that points to the teleporter location
        // Once there are no enemies left, enable the teleporter and modify UI elements
        if (enemiesLeft <= 0) {
            enemyLeftText.gameObject.SetActive(false);
            goTeleporterText.gameObject.SetActive(true);
            finishPortal.SetActive(true);
            portalArrow.SetActive(true);
        }
    }

    // Performs level finish related tasks
    public void LevelFinish() {
        canBePaused = false;
        TurnOffUIExceptTime();
        Cursor.visible = true;
        finishCanvas.gameObject.SetActive(true);
        inGameMenuManager.TurnOnFinishSystem();
        levelFinished = true;

        // Read in the previous best time
        bestTime = PlayerPrefs.GetFloat(levelName, 0);

        // If current finish time is better, replace it
        if (bestTime == 0 || timer < bestTime) {
            bestTime = timer;
            PlayerPrefs.SetFloat(levelName, bestTime);
            DisplayNewBestTime();
        }
    }

    // Performs level fail related tasks (plane crash)
    public void LevelFail() {
        canBePaused = false;
        levelFailed = true;
        TurnOffUIExceptTime();
        Cursor.visible = true;
        failCanvas.gameObject.SetActive(true);
        inGameMenuManager.TurnOnFailSystem();
    }

    // Updates the boost meter in the UI
    public void UpdateBoostMeter(float boostMeter) {
        currentBoostMeter.rectTransform.localScale = new Vector3(boostMeter / 100, 1, 1);
        boostNumberText.text = ((int)boostMeter).ToString();
    }

    public float IsInvertedControls() {
        return isInverted;
    }

    // Unpauses the game
    public void UnpauseGame() {
        Time.timeScale = 1;
        gamePaused = false;
        uiCanvas.gameObject.SetActive(true);
        Cursor.visible = false;
        pauseCanvas.gameObject.SetActive(false);
    }

    // Brings up the pause menu
    void PauseGame() {
        Time.timeScale = 0;
        gamePaused = true;
        Cursor.visible = true;
        pauseCanvas.gameObject.SetActive(true);
        inGameMenuManager.TurnOnPauseSystem();
    }

    // Displays the new best time
    void DisplayNewBestTime() {
        bestTimeText.gameObject.SetActive(true);
    }
    

    // Turns off all UI except for the Time
    void TurnOffUIExceptTime() {
        portalArrow.SetActive(false);
        goTeleporterText.gameObject.SetActive(false);
        boostNumberText.gameObject.SetActive(false);
        boostText.gameObject.SetActive(false);
        boostBackgroundMeter.gameObject.SetActive(false);
        currentBoostMeter.gameObject.SetActive(false);
    }

}
