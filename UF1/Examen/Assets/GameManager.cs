using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;

    public Vector3 spawnPoint;
    public Vector2 randomField;
    public float spawnDelay;

    EnemyController enemyController;

    public Data data;

    //public int points;
    public int combo;
    public string lastDeath = null;

    bool difficult1;
    bool difficult2;

    private void Start()
    {
        spawnDelay = 3f;
        StartCoroutine(SpawnEnemy());
        data.points = 0;
        //points = 0;
        combo = 0;

        difficult1 = true;
        difficult2 = true;
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject enemyClone = Instantiate(enemy, spawnPoint + new Vector3(Random.Range(-randomField.x, randomField.x), Random.Range(-randomField.y, randomField.y), 0), Quaternion.identity);
            enemyController = enemyClone.GetComponent<EnemyController>();
            enemyController.isWhite = GetRandomBool();
            enemyController.onEnemyDeath += EnemyDeath;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    bool GetRandomBool()
    {
        int randomNumber = Random.Range(0, 100);
        return (randomNumber % 2 == 0) ? true : false;
    }

    void EnemyDeath(string color)
    {
        if (lastDeath == null) lastDeath = color;
        else if (lastDeath == color) combo++;
        else
        {
            combo = 0;
            lastDeath = color;
        }

        data.points += 100 * (combo + 1); 

        if (data.points >= 10000 && difficult2)
        {
            //print("win");
            StopCoroutine(SpawnEnemy());
            spawnDelay = 0;
            difficult2 = false;
        }
        else if (data.points >= 5000 && difficult1)
        {
            //print("difficult augmented");
            StopCoroutine(SpawnEnemy());
            spawnDelay = 1.5f;
            StartCoroutine(SpawnEnemy());
            difficult1 = false;
        }
    }

}
