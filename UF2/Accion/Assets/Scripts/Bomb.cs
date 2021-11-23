using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool Explotar;
    [Range(0f,1f)]
    public float offset;
    public GameObject explosion;

    void Awake()
    {
        Explotar = false;  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (Explotar) {
            if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9) {
                Explode();
                
            }
            
        }
    }
    public void Explode(){
        Vector3 offsetExp = new Vector3( 0, -offset, 0);
        GameObject NewExplosio = Instantiate(explosion,this.transform.position+offsetExp,Quaternion.identity);
        Destroy(this.gameObject);
    }

}
