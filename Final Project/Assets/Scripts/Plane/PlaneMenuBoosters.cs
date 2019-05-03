using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMenuBoosters : MonoBehaviour {

    public ParticleSystem normalBoost;
    public ParticleSystem superBoost;
    public float delay;

    // Use this for initialization
    void Start () {
        // Loop between no boost, first boost, and second boost with 2 second delays
        Time.timeScale = 1;
        StartCoroutine(BoosterNone());
	}
	
    // Disable all boosters
    IEnumerator BoosterNone() {
        if (normalBoost.isPlaying) {
            normalBoost.Stop();
        }
        if (superBoost.isPlaying) {
            superBoost.Stop();
        }
        yield return new WaitForSeconds(delay + 0.5f);
        StartCoroutine(BoosterOne());
    }

    // Start booster 1
    IEnumerator BoosterOne() {
        if (!normalBoost.isPlaying) {
            normalBoost.Play();
        }
        yield return new WaitForSeconds(delay + 0.5f);
        StartCoroutine(BoosterTwo());
    }

    // Start booster 2
    IEnumerator BoosterTwo() {
        if (normalBoost.isPlaying) {
            normalBoost.Clear();
            normalBoost.Stop();
        }
        if (!superBoost.isPlaying) {
            superBoost.Play();
        }
        yield return new WaitForSeconds(delay);
        StartCoroutine(BoosterNone());
    }
}
