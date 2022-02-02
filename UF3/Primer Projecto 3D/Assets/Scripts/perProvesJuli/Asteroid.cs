using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    SpawnerAsteroidDynamicPool pool;
    [SerializeField, Range(10f,2500f)] float force = 1000f;
    [SerializeField] int divisions = 4;
    public int division;

	private void Awake()
	{
        pool = SpawnerAsteroidDynamicPool.Instance;
	}

	private void OnEnable()
	{
        division = divisions;
    }

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Planet")
        {
            int subDivisions = Random.Range(0, division);

            for (int i = 0; i < subDivisions; i++)
            {
                Vector3 random = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                //direction += collision.GetContact(0).normal;
                Vector3 direction = (GetComponent<Rigidbody>().velocity.normalized + collision.GetContact(0).normal) /3 + (random * 0.3f);
                GameObject asteroid  = pool.SpawnAsteroid(transform.position, direction);
                asteroid.GetComponent<Asteroid>().division = subDivisions - 1;
                asteroid.transform.localScale = asteroid.transform.localScale / 2;
            }
            StartCoroutine(death());
        }else if (collision.transform.tag == "Player")
        {
            collision.transform.position =new Vector3(-366.2f,187.9f,81.4f);
        }
    }

    IEnumerator death()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2f);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
        pool.ReturnAsteroid(gameObject);
    }
}
