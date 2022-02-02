using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMeta : MonoBehaviour
{
    Transform player;
    public bool isPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isPlayer = player != null;
    }

    void Update()
    {


        if (isPlayer)
        {
            this.transform.LookAt(player.position);
        }
    }
}
