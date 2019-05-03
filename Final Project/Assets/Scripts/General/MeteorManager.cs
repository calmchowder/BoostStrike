using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour {

    public float cellMapSize;
    public float entireSize;
    public float bufferSize;
    public float maxSpawnHeight;
    public float spawnRate;
    public Vector3 offset;
    public GameObject meteor;

    float cellSize;
    float timer;

    // Use this for initialization
    void Start () {
        cellSize = entireSize / cellMapSize;
        timer = spawnRate;  // Makes it spawn instantly

    }
	
	// Update is called once per frame
	void Update () {
        // TODO: If player is dead, stop spawning meteors
        timer += Time.deltaTime;
        if (timer >= spawnRate) {
            SpawnMeteors();
            timer = 0f;
        }
	}

    // Spawn meteors using procedural generation (blue noise)
    void SpawnMeteors() {
        for (int i = 0; i < cellMapSize; i++) {
            // The bufferSize makes it so no two meteors can spawn right next to each other 
            float xMin = -(entireSize / 2) + cellSize * i + bufferSize;
            float xMax = -(entireSize / 2) + cellSize * (i + 1) - bufferSize;
            
            for (int j = 0; j < cellMapSize; j++) {
                float zMin = -(entireSize / 2) + cellSize * j + bufferSize;
                float zMax = -(entireSize / 2) + cellSize * (j + 1) - bufferSize;

                Instantiate(meteor, new Vector3(Random.Range(xMin, xMax) + offset.x, 
                                                Random.Range(0f, maxSpawnHeight) + offset.y, 
                                                Random.Range(zMin, zMax) + offset.z), Quaternion.identity);
            }
        }


    }
}
