using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaCano : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rot;
    public bool marrana;
    void Start()
    {
        marrana = true;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (marrana) {
            if (collision.transform.tag == "Plataforma") {
                marrana = false;
                GameObject clon = Instantiate(rot, new Vector3(this.transform.position.x, this.transform.position.y, 0), this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
