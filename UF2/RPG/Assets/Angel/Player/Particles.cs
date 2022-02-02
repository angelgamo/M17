using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] TrailRenderer trail;

    public void TrailOn()
    {
        trail.enabled = true;
    }

    IEnumerator TrailOff()
    {
        yield return new WaitForSeconds(.2f);
        trail.enabled = false;
    }
}
