using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour {

    public float attackStartingTime;
    public float attackTime;
    public float bulletSpeed;
    public bool willAttack;
    public Transform player;
    public GameObject bullet;
    public GameObject crossBullet;
    public GameObject squareBullet;
    public GameObject circleBullet;
    public Color shootWarningColor;

    AudioSource audioSource;
    Renderer rend;
    float timer;
    float shootWarningTime;
    Vector3 bulletDirection;
    Quaternion bulletRotation;
    Vector4 originalOutlineColor;

    void Start() {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        timer = attackStartingTime;
        shootWarningTime = 0.5f;
        originalOutlineColor = rend.material.GetVector("_SecondOutlineColor");
    }

    // Update is called once per frame
    void Update() {
        // If the enemy is right about to shoot, change the inner outline to red
        if (timer >= attackTime - shootWarningTime) {
            rend.material.SetVector("_SecondOutlineColor", new Vector4(shootWarningColor.r, shootWarningColor.g, shootWarningColor.b, shootWarningColor.a));
        }

        // If the enemy can attack
        if (timer >= attackTime && willAttack) {
            // If the player is still alive
            if (player != null) {
                RaycastHit hit;
                Vector3 rayDirection = player.position - this.transform.position;

                // If the player is within line of sight of the enemy
                if (Physics.Raycast(this.transform.position, rayDirection, out hit)) {
                    if (hit.transform == player) {
                        // Choose a shape randomly (or chain gun). There are 4 choices in total
                        int randVal = Random.Range(0, 4);
                        if (randVal == 0) {
                            ShootShape(crossBullet);
                        }
                        else if (randVal == 1) {
                            ShootShape(squareBullet);
                        }
                        else if (randVal == 2) {
                            ShootShape(circleBullet);
                        }
                        else if (randVal == 3) {
                            StartCoroutine(ShootChainGun());
                        }

                        rend.material.SetVector("_SecondOutlineColor", originalOutlineColor);  // Reset the outline color
                        timer = 0;  // Reset the attack timer
                    }
                }
            }
        }
        timer += Time.deltaTime;
    }

    // Shoots out a specific shape
    void ShootShape(GameObject bulletShape) {
        // Set the bullet's direction and rotation
        bulletDirection = player.transform.position - this.transform.position;
        bulletRotation = Quaternion.LookRotation(bulletDirection);

        GameObject enemyBullet = Instantiate(bulletShape, this.transform.position, bulletRotation);

        // Cross attack velocity
        Component[] bulletRigidbodies = enemyBullet.GetComponentsInChildren(typeof(Rigidbody));
        foreach (Rigidbody rb in bulletRigidbodies) {
            rb.velocity = (Vector3.Normalize(bulletDirection) * bulletSpeed);
        }

        audioSource.Play();  // Play the enemy laser sound

    }

    // Shoots out a chain of bullets
    IEnumerator ShootChainGun() {
        // Shoot out 15 bullets in 0.15 second intervals
        for (int i = 0; i < 15; i++) {
            // Only shoot when the player is alive (they might die mid attack)
            if (player != null) {
                // Set the bullet's direction and rotation
                bulletDirection = player.transform.position - this.transform.position;
                bulletRotation = Quaternion.LookRotation(bulletDirection);

                GameObject enemyBullet = Instantiate(bullet, this.transform.position, bulletRotation);
                enemyBullet.GetComponent<Rigidbody>().velocity = (Vector3.Normalize(bulletDirection) * bulletSpeed);
                audioSource.Play();  // Play the enemy laser sound
                yield return new WaitForSeconds(0.15f);
            }
        }
        timer = 0f; // Resets the attack timer again since the chain gun attack takes some time
    }
}
