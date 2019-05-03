using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed;

    void FixedUpdate () {
        // If the player died, no need to follow
        if (target == null) {
            return;
        }

        //  Camera follow character. Written in fixed update to avoid camera lerp break

        // Calculate local offset depending on dodge action
        Vector3 finalOffset = target.transform.right * offset.x + target.transform.up * offset.y + target.transform.forward * offset.z;

        // Update position based on offset
        Vector3 finalPosition = target.transform.position + finalOffset;
        transform.position = Vector3.Lerp(transform.position, finalPosition, Time.fixedDeltaTime * (smoothSpeed));
        transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, Time.fixedDeltaTime * smoothSpeed);

    }
}
