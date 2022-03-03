using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;

	void Awake()
	{
		mainCamera = Camera.main;
	}

	void Update()
	{
		ray.origin = mainCamera.transform.position;
		ray.direction = mainCamera.transform.forward;
		if (Physics.Raycast(ray, out hitInfo))
			transform.position = hitInfo.point;
		else
			transform.position = mainCamera.transform.forward * 1000f;
	}
}
