using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [Header("Grenade Variables")]
    [SerializeField] GameObject body;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject hitbox;

    private void OnEnable()
    {
        StartCoroutine(Grenade());
    }

    IEnumerator Grenade()
    {
        yield return new WaitForSeconds(2f);
        body.SetActive(false);
        explosion.Play();
        hitbox.SetActive(true);
        yield return new WaitForSeconds(.4f);
        hitbox.SetActive(false);
        yield return new WaitForSeconds(1.1f);
        body.SetActive(true);
        gameObject.SetActive(false);
    }
}
