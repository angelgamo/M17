using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    public int nEnemies;
    public List<GameObject> enemyPool;
    public List<GameObject> enemy_types;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i < nEnemies; i++)
        {
            int index = Random.Range(0, enemy_types.Count);
            GameObject enemy = enemy_types[index];
            enemy.name = "Enemy" + i;
            enemy.transform.position = this.transform.position;
            enemy.GetComponent<IAEnemigo>().player = player;
            enemy.gameObject.SetActive(false);
            Instantiate(enemy, this.transform);
            enemyPool.Add(enemy);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetEnemy()
    {
        GameObject enemy_In_Pool;
        if (enemyPool.Count > 0)
        {
            enemy_In_Pool = enemyPool[enemyPool.Count-1];
            GameObject enemy = this.transform.Find(enemy_In_Pool.name).gameObject;
            enemy.SetActive(true);
            enemyPool.Remove(enemy_In_Pool);
        }
       
    }

    public bool ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Add(enemy);
        return true;
    }
}
