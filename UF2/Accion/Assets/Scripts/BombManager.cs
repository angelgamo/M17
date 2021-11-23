using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public GameObject bomb;
    public GameObject warn;
    private Transform player;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bomb")
        {
            Destroy(collision.gameObject);
            StartCoroutine("bomb2");
        }
    }

   

    IEnumerator bomb2() {
        Vector3 offsetWarn = new Vector3(this.player.position.x, -6, 0);
        Vector3 offsetBomb = new Vector3(this.player.position.x, -1.5f, 0);
        GameObject NewWarn = Instantiate(warn, this.transform.position + offsetWarn, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(NewWarn);
        GameObject NewBomb = Instantiate(bomb, this.transform.position + offsetBomb, Quaternion.identity);
        NewBomb.GetComponent<Bomb>().Explotar = true;
        NewBomb.transform.rotation = Quaternion.Euler(0, 0, -73);
        Destroy(NewBomb, 3f);
    }
}
