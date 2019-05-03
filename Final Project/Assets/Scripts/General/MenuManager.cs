using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Canvas mainMenuCanvas;
    public Canvas instructionsCanvas;
    public Canvas levelListCanvas;
    public Canvas controlsCanvas;
    public Canvas creditsCanvas;
    public RawImage invertCheckBox;
    public RawImage lightCheckBox;
    public Text level1Time;
    public Text level2Time;
    public Text level3Time;
    public Text level4Time;
    public EventSystem mainMenuSystem;
    public EventSystem instructionsSystem;
    public EventSystem levelListSystem;
    public EventSystem controlsSystem;
    public EventSystem creditsSystem;
    public EventSystem mouseKeyboardSystem;

    bool controllerPluggedIn;
    bool isInverted;
    bool isLightsOn;

    void Start() {
        // If we have a controller plugged in, then use the controller event system
        if (Input.GetJoystickNames().Length > 0) {
            mainMenuSystem.gameObject.SetActive(true);
        }
        else {
            mouseKeyboardSystem.gameObject.SetActive(true);
        }

        // Checks if invert controls are on
        if (PlayerPrefs.GetFloat("IsInverted", 0) == 1) {
            isInverted = true;
            invertCheckBox.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetFloat("IsLightsOn", 1) == 1) {
            isLightsOn = true;
            lightCheckBox.gameObject.SetActive(true);
        }
        
    }

    void Update() {
        if (Input.GetJoystickNames().Length > 0) {
            controllerPluggedIn = true;
        }
        else {
            controllerPluggedIn = false;
        }

    }

    // Displays the main menu
    public void OpenMainMenu() {
        TurnOffAllCanvas();
        mainMenuCanvas.gameObject.SetActive(true);
        if (controllerPluggedIn) {
            mainMenuSystem.gameObject.SetActive(true);
        }
    }

    // Displays the instructions
    public void OpenInstructions() {
        TurnOffAllCanvas();
        instructionsCanvas.gameObject.SetActive(true);
        if (controllerPluggedIn) {
            instructionsSystem.gameObject.SetActive(true);
        }
    }

    // Displays the levels
    public void OpenLevelList() {
        TurnOffAllCanvas();
        UpdateTimes();
        levelListCanvas.gameObject.SetActive(true);
        if (controllerPluggedIn) {
            levelListSystem.gameObject.SetActive(true);
        }
    }

    // Displays the controls
    public void OpenControls() {
        TurnOffAllCanvas();
        controlsCanvas.gameObject.SetActive(true);
        if (controllerPluggedIn) {
            controlsSystem.gameObject.SetActive(true);
        }
    }

    // Displays the credits
    public void OpenCredits() {
        TurnOffAllCanvas();
        creditsCanvas.gameObject.SetActive(true);
        if (controllerPluggedIn) {
            creditsSystem.gameObject.SetActive(true);
        }
    }

    // Quits the game
    public void ExitGame() {
        Application.Quit();
    }

    // Loads the scene for level 1
    public void LoadLevel1() {
        SceneManager.LoadScene("Level1");
    }

    // Loads the scene for level 2
    public void LoadLevel2() {
        SceneManager.LoadScene("Level2");
    }

    // Loads the scene for level 3
    public void LoadLevel3() {
        SceneManager.LoadScene("Level3");
    }

    // Loads the scene for level 4
    public void LoadLevel4() {
        SceneManager.LoadScene("Level4");
    }

    // Updates the best times on the Level List
    void UpdateTimes() {
        level1Time.text = "Level 1\n" + GiveBestTime(PlayerPrefs.GetFloat("Level1", 0));
        level2Time.text = "Level 2\n" + GiveBestTime(PlayerPrefs.GetFloat("Level2", 0));
        level3Time.text = "Level 3\n" + GiveBestTime(PlayerPrefs.GetFloat("Level3", 0));
        level4Time.text = "Level 4\n" + GiveBestTime(PlayerPrefs.GetFloat("Level4", 0));
    }

    string GiveBestTime(float bestTime) {
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(bestTime);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
    }

    // Changes the invert controls and saves them to invert.txt
    public void ChangeInvertControls() {
        // Modifies the check box
        if (isInverted) {
            invertCheckBox.gameObject.SetActive(false);
            isInverted = false;
        }
        else {
            invertCheckBox.gameObject.SetActive(true);
            isInverted = true;
        }

        // Save the setting for inverted controls
        if (isInverted) {
            PlayerPrefs.SetFloat("IsInverted", 1f);
        }
        else {
            PlayerPrefs.SetFloat("IsInverted", 0f);
        }
    }

    public void ChangeCautionLights() {
        if (isLightsOn) {
            lightCheckBox.gameObject.SetActive(false);
            isLightsOn = false;
        }
        else {
            lightCheckBox.gameObject.SetActive(true);
            isLightsOn = true;
        }

        if (isLightsOn) {
            PlayerPrefs.SetFloat("IsLightsOn", 1f);
        }
        else {
            PlayerPrefs.SetFloat("IsLightsOn", 0f);
        }
    }

    // Turns off all canvases to reset the UI
    void TurnOffAllCanvas() {
        mainMenuCanvas.gameObject.SetActive(false);
        instructionsCanvas.gameObject.SetActive(false);
        levelListCanvas.gameObject.SetActive(false);
        controlsCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(false);
        mainMenuSystem.gameObject.SetActive(false);
        instructionsSystem.gameObject.SetActive(false);
        controlsSystem.gameObject.SetActive(false);
        levelListSystem.gameObject.SetActive(false);
        creditsSystem.gameObject.SetActive(false);
    }
}
