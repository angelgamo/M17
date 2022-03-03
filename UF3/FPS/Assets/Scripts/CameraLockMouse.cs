using UnityEngine;

public class CameraLockMouse : MonoBehaviour
{
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
}
