using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public float spd;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawnPlatform");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnPlatform() {
        while (true) {
            GameObject plat = PlatformPool.SharedInstance.GetPooledObject();
            
            if (plat != null) {
                plat.transform.position = this.transform.position;
                plat.transform.position = this.transform.position;
                plat.SetActive(true);
                plat.GetComponent<Rigidbody2D>().velocity = new Vector3(spd, plat.GetComponent<Rigidbody2D>().velocity.y, 0);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
