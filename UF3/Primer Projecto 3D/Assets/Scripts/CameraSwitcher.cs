using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    static List<CinemachineVirtualCameraBase> cameras = new List<CinemachineVirtualCameraBase>();

    public static CinemachineVirtualCameraBase activeCamera = null;

    public static bool isActiveCamera(CinemachineVirtualCameraBase camera)
	{
        return activeCamera == camera;
	}

    public static void SwitchCamera(CinemachineVirtualCameraBase camera)
	{
        if (isActiveCamera(camera))
            return;

        camera.Priority = 10;
        activeCamera = camera;

        foreach (var cam in cameras)
            if (cam != camera)
                cam.Priority = 0;
	}

    public static void Register(CinemachineVirtualCameraBase camera)
	{
        cameras.Add(camera);
	}

    public static void Unregister(CinemachineVirtualCameraBase camera)
	{
        cameras.Remove(camera);
	}
}
