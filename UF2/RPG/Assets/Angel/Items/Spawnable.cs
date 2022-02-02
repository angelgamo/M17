using System.Collections;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [HideInInspector]
    public Transform player;

    [HideInInspector] public bool follow;
    [SerializeField] float duration = 1f;
    [SerializeField] AnimationCurve curve;
    bool start;

    public void DestroyCoin()
    {
        SpawnManager.istance.QueueCoin(this.gameObject);
        start = false;
    }

    public void Go2Player()
    {
        if (!start && follow)
        {
            StartCoroutine(MoveCurve());
            start = true;
        }
    }

    IEnumerator MoveCurve()
    {
        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, player.position, curve.Evaluate(time / duration));

            yield return null;
        }

        transform.position = player.position;
    }
}
