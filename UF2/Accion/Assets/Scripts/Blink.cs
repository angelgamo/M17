using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f, 1f)]
    public float time;
    [Range(0f, 1f)]
    public float alphaOn;
    [Range(0f, 1f)]
    public float alphaOff;
    bool change = true;
    void Start()
    {
        StartCoroutine("blink");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator blink() {
        while (true)
        {
            Color color = this.GetComponent<SpriteRenderer>().material.color;
            if (change)
            {
                color.a = alphaOff;
            }
            else
            {
                color.a = alphaOn;
            }
            change = !change;
            this.GetComponent<SpriteRenderer>().material.color = color;
            yield return new WaitForSeconds(time);
        }
    }
}
