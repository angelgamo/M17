using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RaySensor2D : MonoBehaviour
{
    [Header("Observations")]
    [Range(1, 50), OnValueChanged("UpdateDirections")] public int raysPerDirection = 1;
    [Range(0, 360), OnValueChanged("UpdateDirections")] public int maxRayDegrees = 0;
    [Range(1, 1000)] public int rayLenght = 10;
    [Range(1, 50)] public int stackedRaycasts = 1;
    public LayerMask layerMask;
    [Space]
    public LayerMask obstacleMask;
    public LayerMask segmentMask;
    public LayerMask foodMask;

    [Header("Debug Gizmos")]
    public Color obstacleColor = Color.blue;
    public Color segmentColor = Color.cyan;
    public Color foodColor = Color.green;

    [Header("Results")]
    [ReadOnly] public Vector2[] angles;
    [ReadOnly] public float[] observations;

    void UpdateDirections()
    {
        angles = GetRayDirections();
    }

    private void OnValidate()
	{
		if (angles == null)
            angles = GetRayDirections();
	}

	private void Start()
	{
        angles = GetRayDirections();
    }

	float[] GetRayAngles()
    {
        var anglesOut = new float[raysPerDirection];
        var delta = maxRayDegrees / raysPerDirection;
        for (var i = 0; i < raysPerDirection; i++)
            anglesOut[i] = 90 - i * delta;
        return anglesOut;
    }

    Vector2[] GetRayDirections()
	{
        var angles = GetRayAngles();
        var anglesOut = new Vector2[angles.Length];
        for (var i = 0;i < angles.Length;i++)
            anglesOut[i] = MathHelpers.DegreeToVector2(angles[i]);

        return anglesOut;
	}

    RaycastHit2D[] hit;

    public float[] GetObservations()
	{
        var observations = new float[angles.Length * 3];
        for (var i = 0; i < angles.Length; i++)
		{
            hit = Physics2D.RaycastAll(transform.position, angles[i], rayLenght, layerMask);

            float segment = -1f;
            float food = -1f;

            foreach (var hitt in hit)
			{
                var hitLayer = 1 << hitt.transform.gameObject.layer;

                if ((hitLayer & obstacleMask.value) != 0)
                    observations[i * 3] = hitt.distance;
                    
                if ((hitLayer & segmentMask.value) != 0)
                    if (segment == -1f || segment > hitt.distance)
                        segment = hitt.distance;
                    
                if ((hitLayer & foodMask.value) != 0)
                    if (food == -1f || food > hitt.distance)
                        food = hitt.distance;
            }

            observations[i * 3 + 1] = segment;
            observations[i * 3 + 2] = food;
        }

        return observations;
	}

    private void OnDrawGizmosSelected()
	{
        observations = GetObservations();

        for (var i = 0;i < angles.Length; i++)
		{
            if (observations[i * 3 + 2] != -1f)
			{
                Gizmos.color = foodColor;
                Gizmos.DrawRay(transform.position, angles[i] * observations[i * 3 + 2]);
            }
            else if (observations[i * 3 + 1] != -1f)
			{
                Gizmos.color = segmentColor;
                Gizmos.DrawRay(transform.position, angles[i] * observations[i * 3 + 1]);
            }
			else
			{
                Gizmos.color = obstacleColor;
                Gizmos.DrawRay(transform.position, angles[i] * observations[i * 3]);
            }
		}
	}
}
