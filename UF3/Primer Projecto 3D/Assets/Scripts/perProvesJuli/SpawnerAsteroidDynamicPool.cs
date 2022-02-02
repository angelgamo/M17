using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroidDynamicPool : MonoBehaviour
{
    #region Singleton
    public static SpawnerAsteroidDynamicPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public Vector3 spawnVariation;
    public float minForce;
    public float maxForce;
    public float minDelay;
    public float maxDelay;
    public float minRotation;
    public float maxRotation;
    public GameObject[] asteroidModels;
    private Queue<GameObject> queueAsteroids = new Queue<GameObject>();

    void Start()
    {
        StartCoroutine(Spawn());
    }

    public GameObject GetAsteroid()
    {
        GameObject asteroid;
        if (queueAsteroids.Count > 0)
		{
            asteroid = queueAsteroids.Dequeue();
            asteroid.SetActive(true);

            DelayedDespawn(asteroid);
            return asteroid;
        }
        
        int index = Random.Range(0, asteroidModels.Length);
        asteroid = Instantiate(asteroidModels[index]);

        DelayedDespawn(asteroid);
        return asteroid;
    }

    public bool ReturnAsteroid(GameObject asteroid)
	{
        asteroid.SetActive(false);
        queueAsteroids.Enqueue(asteroid);
        return true;
	}

    IEnumerator Spawn()
	{
		for(;;)
		{
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnVariation.x, spawnVariation.x), Random.Range(-spawnVariation.y, spawnVariation.y), Random.Range(-spawnVariation.z, spawnVariation.z));
            GameObject asteroid = SpawnAsteroid(position, direction);
            asteroid.transform.localScale = Vector3.one;

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
	}

    public GameObject SpawnAsteroid(Vector3 position, Vector3 direction)
	{
        Vector3 rotaion = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        GameObject asteroid = GetAsteroid();
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();

        asteroid.transform.position = position;
        rb.velocity.Set(0f, 0f, 0f);
        rb.AddForce(direction * Random.Range(minForce, maxForce));
        rb.AddTorque(rotaion * Random.Range(minRotation, maxRotation));

        return asteroid;
    }

    IEnumerator DelayedDespawn(GameObject asteroid)
	{
        yield return new WaitForSeconds(10);
        ReturnAsteroid(asteroid);
    }
}
