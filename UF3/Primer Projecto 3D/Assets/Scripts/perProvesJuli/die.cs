using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour
{
    [SerializeField]
    float force;
    public int div;
    // Start is called before the first frame update
    void Start()
    {
        force = 1000;
        div = 4;
    }
    private void Awake()
    {
    }
    void OnEnable() {
        div = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Planet")
        {
            for (int i = 0; i < Random.Range(0, div); i++)
            {
                GameObject meteor = Instantiate(this.gameObject, transform.position, transform.rotation);
                meteor.GetComponent<die>().div=this.div--; 
                meteor.transform.localScale = meteor.transform.localScale / 2;
                meteor.GetComponent<Rigidbody>().AddForce(meteor.transform.up * Random.Range(-force, force));
                meteor.GetComponent<Rigidbody>().AddForce(meteor.transform.right * Random.Range(-force, force));
                meteor.GetComponent<Rigidbody>().AddForce(meteor.transform.forward * Random.Range(-force, force));
            }

            StartCoroutine(death());
        }
    }

    IEnumerator death() {
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2f);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        this.gameObject.SetActive(false);
    }

}
