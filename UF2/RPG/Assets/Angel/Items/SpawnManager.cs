using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Singleton
    public static SpawnManager istance;

    private void Awake()
    {
        istance = this;
    }
    #endregion

    Queue<GameObject> coinPool = new Queue<GameObject>();

    Transform player;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject spawnablePrefab;
    [SerializeField] float minOffset = 1f;
    [SerializeField] float maxOffset = 2f;
    [SerializeField] float duration = 1f;
    [SerializeField] AnimationCurve curve;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnObject(Item item, bool autoCollect, int ammount = 1, Vector3 position = default(Vector3))
    {
        for (int i = 0; i < ammount; i++)
		{
            GameObject go = Instantiate(spawnablePrefab, position, Quaternion.identity);
            go.GetComponent<ItemPickUp>().item = item;
            go.GetComponent<ItemPickUp>().UpdateEditor(); ;
            go.GetComponent<Spawnable>().player = player;
            go.GetComponent<Spawnable>().follow = autoCollect;
            Vector2 randomPosition = getRandomPosition(position);
            StartCoroutine(MoveCurve(go.transform, randomPosition));
        }
    }

    public void SpawnCoin(bool autoCollect, int ammount = 1, Vector3 position = default(Vector3))
    {
        for (int i = 0; i < ammount; i++)
        {
            GameObject coin = GetCoin();
            coin.transform.position = position;
            coin.GetComponent<Spawnable>().follow = autoCollect;
            Vector2 randomPosition = getRandomPosition(position);
            StartCoroutine(MoveCurve(coin.transform, randomPosition));
        }
    }

    IEnumerator MoveCurve(Transform go, Vector3 endPosition)
    {
        float time = 0;
        Vector2 startPosition = go.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            go.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(time / duration));
            yield return null;
        }
        go.position = endPosition;
        go.GetComponent<Spawnable>().Go2Player();
        go.GetComponent<Interactable>().StartCoroutine(go.GetComponent<Interactable>().Check());
    }

    GameObject GetCoin()
    {
        if (coinPool.Count > 0)
        {
            GameObject coin = coinPool.Dequeue();
            coin.transform.position = transform.position;
            coin.GetComponent<Interactable>().Restart();
            coin.SetActive(true);
            return coin;
        }
        else
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.GetComponent<Spawnable>().player = player;
            return coin;
        }
    }

    public void QueueCoin(GameObject coin)
    {
        coin.SetActive(false);
        coinPool.Enqueue(coin);
    }

    Vector2 getRandomPosition(Vector2 position)
    {
        float angle = Random.Range(0f, 360f);
        float radians = angle * Mathf.Deg2Rad;

        Vector2 random = new Vector2((float)Mathf.Sin(radians), (float)Mathf.Cos(radians));

        random *= Random.Range(minOffset, maxOffset);

        return position + random;
    }
}
