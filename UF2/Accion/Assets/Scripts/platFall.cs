using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platFall : MonoBehaviour
{
    Vector3 iniPos;
    // Start is called before the first frame update
    void Start()
    {
        iniPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine("Caure");
        StartCoroutine("Respawn");
    }

    IEnumerator Caure()
    {
        yield return new WaitForSeconds(1f);
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(7f);
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.transform.position = iniPos;
    }
}
