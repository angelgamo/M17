using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public void startEx(float sec) {
        StartCoroutine(Explode(sec));
    }
    IEnumerator Explode(float sec) {
        yield return new WaitForSeconds(sec);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);
        Explosion();
        //ExplosionCollider.enabled = true;
        this.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        this.GetComponent<MeshRenderer>().enabled= false;
        this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    void Explosion()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 20);
        foreach (Collider c in hits)
        {
            Debug.Log("hit "+ c.transform.tag);
            if (c.transform.tag == "enemigo")
            {
                c.GetComponent<HpManager>().ReceiveDamage(50);
            }
        }
    }
}
