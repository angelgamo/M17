using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class vida : MonoBehaviour
{
    new Collider collider;
	MeshRenderer meshRenderer;

	private void Awake()
	{
		collider = GetComponent<Collider>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Start()
	{
		Sequence mySequence = DOTween.Sequence();
		mySequence
		.Append(transform.DOMoveY(transform.position.y + 1, 1f).SetEase(Ease.InOutFlash))
		.SetLoops(-1, LoopType.Yoyo);
		transform.DORotate(Vector3.up * 180, 1f).SetEase(Ease.Linear).SetLoops(-1);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
            other.GetComponent<HpPlayerManager>().ReceiveHealth(250f);
			StartCoroutine(respawn());
		}
	}

	IEnumerator respawn()
	{
		collider.enabled = false;
		meshRenderer.enabled = false;
        yield return new WaitForSeconds(10f);
		collider.enabled = true;
		meshRenderer.enabled = true;
	}
}
