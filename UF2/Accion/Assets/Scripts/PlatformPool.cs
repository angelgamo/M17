using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    public static PlatformPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject[] objectToPool;
    public int quantity;

    private void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < quantity; i++)
        {
            tmp = Instantiate(objectToPool[0]);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }  
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
