using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InOut : MonoBehaviour
{
    [SerializeField]
    Vector3 finalPos;
    Vector3 startPos;
    [SerializeField]
    float startSeconds, seconds,transitionDuration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait(startSeconds));
        startPos = this.transform.position;
    }
    private void FixedUpdate()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator wait(float sec) {
        yield return new WaitForSeconds(sec);
        StartCoroutine(MoveLoop());
    }
    IEnumerator MoveLoop() {
        while (true)
        {
            transform.DOMove(finalPos, transitionDuration);
            yield return new WaitForSeconds(seconds+ transitionDuration);
            transform.DOMove(startPos, transitionDuration);
            yield return new WaitForSeconds(seconds+ transitionDuration);
        }
    }
}
