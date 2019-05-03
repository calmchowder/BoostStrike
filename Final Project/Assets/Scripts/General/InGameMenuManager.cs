using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InGameMenuManager : MonoBehaviour {

    public EventSystem pauseSystem;
    public EventSystem finishSystem;
    public EventSystem failSystem;
    public EventSystem mouseKeyboardSystem;

    GameManager gameManager;
    bool controlledPluggedIn;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        // If we have a controller plugged in, then use the controller event system
        if (Input.GetJoystickNames().Length <= 0) {
            mouseKeyboardSystem.gameObject.SetActive(true);
        }
    }

    void Update() {
        if (Input.GetJoystickNames().Length > 0) {
            controlledPluggedIn = true;
        }
        else {
            controlledPluggedIn = false;
        }
    }

    // Resumes the game (same as pressing start)
    public void ResumeGame() {
        gameManager.UnpauseGame();
    }

    // Restarts the current level
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Returns to the main menu
    public void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    // Goes to Level 2
    public void GoToLevel2() {
        SceneManager.LoadScene("Level2");
    }

    // Goes to Level 3
    public void GoToLevel3() {
        SceneManager.LoadScene("Level3");
    }

    // Goes to Level 4
    public void GoToLevel4() {
        SceneManager.LoadScene("Level4");
    }

    // Turns on the pause event system
    public void TurnOnPauseSystem() {
        TurnOffAllSystems();
        if (controlledPluggedIn) {
            pauseSystem.gameObject.SetActive(true);
        }
    }

    // Turns on the finish event system
    public void TurnOnFinishSystem() {
        TurnOffAllSystems();
        if (controlledPluggedIn) {
            finishSystem.gameObject.SetActive(true);
        }
    }

    // Turns on the fail event system
    public void TurnOnFailSystem() {
        TurnOffAllSystems();
        if (controlledPluggedIn) {
            failSystem.gameObject.SetActive(true);
        }
    }

    // Turns off all event systems
    void TurnOffAllSystems() {
        pauseSystem.gameObject.SetActive(false);
        finishSystem.gameObject.SetActive(false);
        failSystem.gameObject.SetActive(false);
    }

	
}
