using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public float attackStartingTime;
    public float attackTime;
    public float bulletSpeed;
    public bool willAttack;
    public Transform player;
    public GameObject bullet;
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
    void Update () {
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
                        bulletDirection = player.transform.position - this.transform.position;
                        bulletRotation = Quaternion.LookRotation(bulletDirection);

                        // Create the new bullet using the direction and rotation above
                        GameObject enemyBullet = Instantiate(bullet, this.transform.position, bulletRotation);
                        enemyBullet.GetComponent<Rigidbody>().velocity = (Vector3.Normalize(bulletDirection) * bulletSpeed);
                        rend.material.SetVector("_SecondOutlineColor", originalOutlineColor);  // Reset the outline color
                        timer = 0;  // Reset the attack timer
                        audioSource.Play();  // Play the enemy laser sound
                    }
                }
            }
        }
        timer += Time.deltaTime;

    }
}
