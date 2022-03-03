using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
public class Platform : MonoBehaviour
{
    List<GameObject> platforms;

	private void OnValidate()
	{
        platforms = new List<GameObject>();
        foreach (Transform child in transform)
            platforms.Add(child.gameObject);
    }

    void Start()
    {
        DO();
    }

	void DO()
	{
        for (int i = 0; i < platforms.Count; i++)
        {
			MeshRenderer meshRenderer = platforms[i].GetComponent<MeshRenderer>();
			meshRenderer.material.color = Color.red;
			Sequence mySequence = DOTween.Sequence();
            mySequence
            .SetDelay(i * .5f, false)
            .AppendInterval(.5f)
            .Join(platforms[i].transform.DORotate(new Vector3(180, 0, 0), 1).SetEase(Ease.Linear))
			.Append(platforms[i].transform.DOLocalMoveX(5, 1).SetEase(Ease.InOutQuad))
            .Join(platforms[i].transform.DORotate(new Vector3(0, -90, 180), 1).SetEase(Ease.Linear))
			.Join(meshRenderer.material.DOColor(Color.green, 1))
			.AppendInterval(.5f)
            .Join(platforms[i].transform.DOScaleY(1, .5f))
            .SetLoops(-1, LoopType.Yoyo);
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
	public void Reset()
	{
        DOTween.PauseAll();
        DOTween.RewindAll();
        DOTween.KillAll();
        DO();
    }
}
