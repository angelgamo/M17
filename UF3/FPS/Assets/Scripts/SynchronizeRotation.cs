using UnityEngine;

public class SynchronizeRotation : MonoBehaviour
{
    public Transform source;

	private void Update()
	{
		if (source != null)
			transform.rotation = source.rotation;
	}
}
