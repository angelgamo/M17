using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //la pool es un singleton
    public static ObjectPool SharedInstance;
    //lista que es la pool
    public List<GameObject> pooledObjects;
    //aqui van los prefabs que queremos poolear
    public GameObject[] objectsToPool;
    //cuantos objetos de cada prefab hay en la pool
    public int amountToPoolEach;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < objectsToPool.Length; i++)
            for (int j = 0; j < amountToPoolEach; j++)
            {
                tmp = Instantiate(objectsToPool[i]);
                //un gameobject que no esta activo no funciona. Existe pero no hace nada
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
