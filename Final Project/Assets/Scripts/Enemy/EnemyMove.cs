using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    public float width;
    public float height;
    public float speed;
    public string movementStyle;
    public Transform player;

    float timer;
    float x;
    float y;
    float z;
    Vector3 startPosition;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime * speed;

		if (movementStyle == "horizontal") {
            x = startPosition.x + Mathf.Cos(timer) * width;
            y = startPosition.y;
            z = startPosition.z;
        }
        else if (movementStyle == "vertical") {
            x = startPosition.x;
            y = startPosition.y + Mathf.Sin(timer) * height;
            z = startPosition.z;
        }
        else if (movementStyle == "horizontal circle") {
            x = startPosition.x + Mathf.Cos(timer) * width;
            y = startPosition.y;
            z = startPosition.z + Mathf.Sin(timer) * height;
        }
        else if (movementStyle == "vertical circle") {
            x = startPosition.x + Mathf.Cos(timer) * width;
            y = startPosition.y + Mathf.Sin(timer) * height;
            z = startPosition.z;
        }
        else if (movementStyle == "none") {
            x = startPosition.x;
            y = startPosition.y;
            z = startPosition.z;
        }

        transform.position = new Vector3(x, y, z);

        if (player != null) {
            // Make the enemy look at the player
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
            transform.LookAt(targetPos);
        }
	}
}
