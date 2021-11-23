using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerGoomba : MonoBehaviour
{
    public float spawnRate;
    void Start()
    {
        InvokeRepeating("GoombaSpawn", 1f, spawnRate);
    }

    void GoombaSpawn()
    {
        GameObject goombaPre = PoolGoombas.instance.GetPooledObject();

        if (goombaPre != null)
        {
            goombaPre.transform.position = this.transform.position;
            goombaPre.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
