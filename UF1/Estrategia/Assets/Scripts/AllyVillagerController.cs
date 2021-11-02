using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyVillagerController : MonoBehaviour
{
    Pathfinding.AIDestinationSetter aidestination;

    private void Start()
    {
        aidestination = GetComponent<Pathfinding.AIDestinationSetter>();
    }

    private void Update()
    {
        if (aidestination.target != null)
        {
            if (Vector2.Distance(transform.position, aidestination.target.position) < .5f)
            {
                aidestination.target = null;
                GetComponent<Pathfinding.AIDestinationSetter>().enabled = false;
                GetComponent<Pathfinding.AILerp>().enabled = false;
                GetComponent<Pathfinding.Seeker>().enabled = false;
            }
        }else
        {
            GetComponent<AI>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        try
        {
            GameObject.Find("Player").GetComponent<PlayerManager>().player.villagersCount--;
        }
        catch
        {

        }
    }
}
