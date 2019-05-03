using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour {

    public float width;
    public float height;
    public float speed;
    public Transform player;

    float timer;
    float moveTimer;
    float x;
    float y;
    float z;
    float startingHealth;
    Vector3 startPosition;
    EnemyHealth enemyHealth;

    // Use this for initialization
    void Start() {
        startPosition = transform.position;
        enemyHealth = GetComponent<EnemyHealth>();
        startingHealth = enemyHealth.health;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime * speed;
        moveTimer += Time.deltaTime;

        // If the boss is still above 50% hp, then don't move
        if (enemyHealth.health >  (startingHealth / 2)) {
            x = startPosition.x;
            y = startPosition.y;
            z = startPosition.z;
        }
        // Once they are below 50% hp, move in a horizontal circle
        else {
            x = startPosition.x + Mathf.Cos(timer) * width;
            y = startPosition.y;
            z = startPosition.z + Mathf.Sin(timer) * height;
        }

        transform.position = new Vector3(x, y, z);

        if (player != null) {
            // Make the enemy look at the player
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
            transform.LookAt(targetPos);
        }
    }

}
