using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMove : MonoBehaviour {

    public float meteorSpeed;

    void Start() {
        GetComponent<Rigidbody>().velocity = Vector3.down * meteorSpeed;
    }

    void OnCollisionEnter(Collision collision) {
        // If the meteor hits a wall, then destroy it
        if (collision.gameObject.layer == 9) {
            Destroy(gameObject);
        }
    }
}
