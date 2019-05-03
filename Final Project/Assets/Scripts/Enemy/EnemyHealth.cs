using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float health;
    public Material damageMaterial;
    public Material bodyMaterial;
    public AudioSource deathSound;
    public AudioSource hurtSound;
    public ParticleSystem enemyExplosion;

    GameManager gameManager;
    PlaneMove planeMove;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        planeMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlaneMove>();
    }

    // Update is called once per frame
    void Update () {
        // If enemy dies, decrease number of enemies 
		if (health <= 0) {
            gameManager.DecreaseEnemies(1);
            planeMove.AddBoost(25);
            deathSound.Play();
            enemyExplosion.transform.position = this.transform.position;
            enemyExplosion.Play();
            Destroy(gameObject);
        }
	}

    // Decreases health off of the enemy
    public void DecreaseHealth(float damage) {
        health -= damage;

        // Play a hurt sound if are hit and are still alive
        if (health > 0) {
            hurtSound.Play();
        }
        StartCoroutine(FlashOnHit());
    }

    // Flashes the enemy red on hit
    public IEnumerator FlashOnHit() {
        this.GetComponent<Renderer>().material = damageMaterial;
        yield return new WaitForSeconds(0.05f);
        this.GetComponent<Renderer>().material = bodyMaterial;
    }

}
