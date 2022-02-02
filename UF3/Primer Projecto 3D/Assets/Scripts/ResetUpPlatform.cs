using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUpPlatform : MonoBehaviour
{
    public Transform source;

	public void Reset()
	{
		Vector3 direction = -(source.position - transform.position).normalized;
		transform.up = direction;
	}
}
