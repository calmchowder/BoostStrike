using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBulletHit : MonoBehaviour {

    // Checks if the bullet hit the player/wall/etc
    void OnCollisionEnter(Collision collision) {
        // Body shot damage = 25
        if (collision.gameObject.layer == 11) {
            print("HIT");
            collision.gameObject.GetComponent<EnemyHealth>().DecreaseHealth(25);
        }

        // Delete the bullet
        Destroy(gameObject);
    }

}
