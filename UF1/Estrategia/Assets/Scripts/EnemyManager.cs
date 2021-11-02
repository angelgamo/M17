using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public float spawnRate;

    void Start()
    {
        spawnRate = 10f;
        StartCoroutine(UpdateRate());
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator UpdateRate() {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            spawnRate *= .75f;
        }
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            //GameObject clon = Instantiate(enemy, new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z), Quaternion.identity);
            GameObject clon = Instantiate(enemy, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            //clon.GetComponent<Pathfinding.AIDestinationSetter>().target = GameObject.Find("TargetEnemic").transform;
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
