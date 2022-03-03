using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnerActivao());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnerActivao()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            this.GetComponent<EnemyPool>().GetEnemy();
        }
    }
}
