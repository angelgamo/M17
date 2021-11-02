using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkEnemySpawn : MonoBehaviour
{
    public GameObject[] chunks;
    public float spawnDelay;
    public Vector3 spawnPoint;

    void Start()
    {
        InvokeRepeating("spawn", 0, spawnDelay);
    }

    private void spawn()
    {
        Instantiate(chunks[Random.Range(0, this.chunks.Length)], spawnPoint, this.transform.rotation);
    }
}
