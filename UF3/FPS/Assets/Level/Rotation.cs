using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    float rotAmount;
    [SerializeField]
    float limit;
    [SerializeField]
    bool hasLimit;
    float startRot;
    bool direction;
    // Start is called before the first frame update
    void Start()
    {
        startRot = this.transform.rotation.y;
        direction = true;
        if (hasLimit)
        {
            StartCoroutine(changedir());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLimit)
        {
            if (direction)
            {
                this.transform.Rotate(0, rotAmount, 0, Space.Self);
            }
            else
            {
                this.transform.Rotate(0, -rotAmount, 0, Space.Self);
            }
        }
        else {
            this.transform.Rotate(0, rotAmount, 0, Space.Self);
        }
        
    }
    IEnumerator changedir() {
        while (true)
        {
            yield return new WaitForSeconds(limit);
            direction = !direction;
        }
    }
}
