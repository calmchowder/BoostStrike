using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFinish : MonoBehaviour {

    public Canvas finishCanvas;
    public ParticleSystem finishEffects;

    GameManager gameManager;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other) {
        print(other.gameObject.layer);
        // Check that the player enters (and not it's speed boost trigger hitbox)
        if (other.gameObject.layer == 10 && !other.isTrigger) {
            finishEffects.gameObject.transform.position = other.gameObject.transform.position;
            finishEffects.gameObject.transform.rotation = other.gameObject.transform.rotation;

            // Play the finish effects and sound
            finishEffects.Play();
            finishEffects.gameObject.GetComponent<AudioSource>().Play();

            // Destroy the plane and call other Level Finishing related tasks
            Destroy(other.gameObject);
            gameManager.LevelFinish();
        }
    }
}
