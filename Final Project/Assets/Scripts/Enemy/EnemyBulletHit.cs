using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHit : MonoBehaviour {

    // Checks if the bullet hit the player/wall/etc
    void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
