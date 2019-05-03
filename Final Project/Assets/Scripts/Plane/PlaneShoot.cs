using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneShoot : MonoBehaviour {

    public float attackTime;
    public float bulletSpeed;
    public GameObject bullet;

    AudioSource audioSource;
    Vector3 bulletDirection;
    Quaternion bulletRotation;
    float timer = 1f;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {

        // Shoot out a raycast if player can shoot and has pressed it
        if (Input.GetButton("Fire1") && timer >= attackTime && !GameManager.gamePaused) { 
            Shoot();
        }

        timer += Time.deltaTime;
    }

    // Create the bullet and send it off
    void Shoot() {
        bulletDirection = transform.forward + transform.up * 0.05f;
        bulletRotation = transform.rotation;

        // Create the new bullet using the direction and rotation above
        GameObject playerBullet = Instantiate(bullet, transform.position + transform.forward, bulletRotation);
        playerBullet.GetComponent<Rigidbody>().velocity = (Vector3.Normalize(bulletDirection) * bulletSpeed);
        timer = 0;  // reset the attack timer
        audioSource.Play(); // Play the laser shoot sound
    }
}
