using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGoombas : MonoBehaviour
{
    public static PoolGoombas instance;
    public List<GameObject> llistaGoombas;
    public GameObject[] prefabsAPoolear;
    public int nElementsDeCada;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        llistaGoombas = new List<GameObject>();
        GameObject goTemporal;
        for (int i = 0; i < prefabsAPoolear.Length; i++)
            for (int j = 0; j < nElementsDeCada; j++)
            {
                goTemporal = Instantiate(prefabsAPoolear[i]);
                goTemporal.SetActive(false);
                llistaGoombas.Add(goTemporal);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < llistaGoombas.Count; i++)
        {
            if (!llistaGoombas[i].activeInHierarchy)
            {
                return llistaGoombas[i];
            }
        }
        return null;
    }

}
