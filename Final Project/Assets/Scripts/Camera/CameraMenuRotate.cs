using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuRotate : MonoBehaviour {

    public Transform target;
    public float speed;
    public float width;
    public float height;

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

        x = startPosition.x + Mathf.Cos(timer) * width;
        y = startPosition.y;
        z = startPosition.z + Mathf.Sin(timer) * height;

        transform.position = new Vector3(x, y, z);
        transform.LookAt(target.position);
    }
}
