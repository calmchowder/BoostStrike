using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCrash : MonoBehaviour {

    public ParticleSystem crashOne;
    public ParticleSystem crashTwo;

    GameManager gameManager;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        crashOne.Stop();
        crashTwo.Stop();
    }

    void OnCollisionEnter(Collision collision) {
        Vector3 deadPosition = transform.position;
        Quaternion deadRotation = transform.rotation;
        Destroy(gameObject);

        // Play the first crash animation
        crashOne.gameObject.transform.position = deadPosition;
        crashOne.gameObject.transform.rotation = deadRotation;
        crashOne.Simulate(6.3f);
        crashOne.Play();

        // Play the second crash animation
        crashTwo.gameObject.transform.position = deadPosition;
        crashTwo.gameObject.transform.rotation = deadRotation;
        crashTwo.Play();

        // Play the crash sound (located on crash animation 1)
        crashOne.gameObject.GetComponent<AudioSource>().Play();

        gameManager.LevelFail();
    }
}
