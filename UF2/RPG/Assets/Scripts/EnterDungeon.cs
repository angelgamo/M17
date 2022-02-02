using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDungeon : MonoBehaviour
{
    public GameObject manager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.transform.root.tag == "Player") 
        {
            manager.GetComponent<SceneController>().LoadDungeon();
        }
    }
}
