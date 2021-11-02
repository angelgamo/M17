using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDespawn : MonoBehaviour
{
    public int offsetX;
    public int offsetY;
    public GameObject impact;

    void Update() // si sale de los margenes marcados se elmina bullet
    {
        if(this.transform.position.x > offsetX || this.transform.position.x < -offsetX)
        {
            Destroy(this.gameObject);
        }
        if (this.transform.position.y > offsetY || this.transform.position.y < -offsetY)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy() // si se elimina bullet salta animacion impacto
    {
        Instantiate(impact, transform.position, transform.rotation);
    }
}
