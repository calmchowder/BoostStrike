using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour {

    public float speed;
    public float boostSpeed;
    public float superBoostSpeed;
    public float boostExitDuration;
    public float boostMeterDrain;
    public GameObject cautionLights;
    public ParticleSystem normalBoost;
    public ParticleSystem superBoost;
    public AudioSource normalBoostSound;
    public AudioSource superBoostSound;

    GameManager gameManager;
    Rigidbody rb;
    Vector3 horizontalRotation;
    Vector3 verticalRotation;
    Vector3 angleRotation;
    bool inBoostRange;
    bool boostPressed;
    float currSpeed;
    float timer;
    float boostMeter;
    float isInverted;
    float normalAudioFadeTime;
    float superAudioFadeTime;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        horizontalRotation = new Vector3(0, 100, 0);
        verticalRotation = new Vector3(130, 0, 0);
        angleRotation = new Vector3(0, 0, 150);
        inBoostRange = false;
        boostPressed = false;
        timer = 1f;
        normalAudioFadeTime = 1f;
        superAudioFadeTime = 0.5f;
        isInverted = gameManager.IsInvertedControls();
        
        if (PlayerPrefs.GetFloat("IsLightsOn", 1) == 1) {
            cautionLights.gameObject.SetActive(true);
        }
        else {
            cautionLights.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        Quaternion rotateQuaternion = new Quaternion(0, 0, 0, 0);

        if (Input.GetButton("Boost") && boostMeter > 0) {
            boostPressed = true;
            DecreaseBoost();
            gameManager.UpdateBoostMeter(boostMeter);
        }
        else {
            boostPressed = false;
        }

        // Move the plane normally if it isn't in boost range and previous boost has ended 
        if (!inBoostRange && !boostPressed && timer >= boostExitDuration) {
            currSpeed = speed;

            // Don't display any boost particles when flying normally
            if (normalBoost.isPlaying) {
                normalBoost.Stop();
                StartCoroutine(AudioFadeOut(normalBoostSound, normalAudioFadeTime));
            }
            if (superBoost.isPlaying) {
                superBoost.Stop();
                StartCoroutine(AudioFadeOut(superBoostSound, normalAudioFadeTime));
            }
        }
        // Pressing the boost key along with the normal boost goes into a super boost
        else if ((inBoostRange || timer < boostExitDuration) && boostPressed) {
            currSpeed = superBoostSpeed;

            if (normalBoost.isPlaying) {
                normalBoost.Clear();
                normalBoost.Stop();
                normalBoostSound.Stop();
            }
            if (!superBoost.isPlaying) {
                superBoost.Play();
                StartCoroutine(AudioFadeIn(superBoostSound, superAudioFadeTime));
            }
        }
        // This case covers the extra boost duration from a normal boost, and pressing it normally
        else {
            currSpeed = boostSpeed;
            if (!normalBoost.isPlaying) {
                normalBoost.Play();
                StartCoroutine(AudioFadeIn(normalBoostSound, superAudioFadeTime));
            }
            if (superBoost.isPlaying) {
                superBoost.Stop();
                StartCoroutine(AudioFadeOut(superBoostSound, superAudioFadeTime));
            }
        }

        // Move the rigid body forward
        rb.MovePosition(transform.position + transform.forward * currSpeed * Time.deltaTime);
        timer += Time.deltaTime;
   
        // Modify the horizontal input
        if (Input.GetAxis("Horizontal") > 0) {
            rotateQuaternion = Quaternion.Euler(horizontalRotation * Input.GetAxis("Horizontal") * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotateQuaternion);
        }
        else if (Input.GetAxis("Horizontal") < 0) {
            rotateQuaternion = Quaternion.Euler(-horizontalRotation * -Input.GetAxis("Horizontal") * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotateQuaternion);
        }

        // Modify the vertical input
        if (Input.GetAxis("Vertical") > 0) {
            rotateQuaternion = Quaternion.Euler(verticalRotation * isInverted * -Input.GetAxis("Vertical") * Time.deltaTime);
            rotateQuaternion.z = 0;
            rb.MoveRotation(rb.rotation * rotateQuaternion);
        }
        else if (Input.GetAxis("Vertical") < 0) {
            rotateQuaternion = Quaternion.Euler(verticalRotation * isInverted * -Input.GetAxis("Vertical") * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotateQuaternion);
        }

        // Modify the tilt
        if (Input.GetAxis("Left Trigger") != 0 || Input.GetAxis("Left Trigger PC") != 0) {
            rotateQuaternion = Quaternion.Euler(angleRotation * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotateQuaternion);
        }

        if (Input.GetAxis("Right Trigger") != 0 || Input.GetAxis("Right Trigger PC") != 0) {
            rotateQuaternion = Quaternion.Euler(-angleRotation * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotateQuaternion);
        }
    }

    // Checks if the plane is in range of a wall to apply a speed boost
    void OnTriggerStay(Collider other) {
        // 9 is the layer for a Wall or Meteor
        if (other.gameObject.layer == 9 || other.gameObject.layer == 15) {
            inBoostRange = true;
        }
    }

    // Checks if the plane has left the range. If so, then take off the boost after 0.5 seconds
    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 15) {
            inBoostRange = false;
            timer = 0;
        }
    }

    // Decrease the boost. Cannot go under 0
    void DecreaseBoost() {
        boostMeter -= boostMeterDrain * Time.deltaTime;
        if (boostMeter <= 0) {
            boostMeter = 0;
        }
    }

    // Add the boost from a kill. Max is 100 
    public void AddBoost(float num) {
        boostMeter += num;
        if (boostMeter > 100) {
            boostMeter = 100;
        }
        gameManager.UpdateBoostMeter(boostMeter);
    }

    IEnumerator AudioFadeOut(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    IEnumerator AudioFadeIn(AudioSource audioSource, float fadeTime) {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 0.9f) {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }

}
