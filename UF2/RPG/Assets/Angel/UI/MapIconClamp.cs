using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapIconClamp : MonoBehaviour
{
	public Transform MinimapCam;
	public float MinimapRadius;
	Vector3 TempV2;
	bool isOut;

	SpriteRenderer sr;
	Sprite inIcon;
	public Sprite outIcon;

    private void Start()
    {
		sr = GetComponent<SpriteRenderer>();
		inIcon = sr.sprite;
		MinimapCam = GameObject.Find("MinimapCam").transform;
	}

    void Update()
	{
		TempV2 = transform.parent.transform.position;
		transform.position = TempV2;
	}

	void LateUpdate()
	{
		// Camera localPosition
		Vector2 centerPosition = MinimapCam.transform.localPosition;

		// Distance from the icon to Minimap
		float Distance = Vector2.Distance(transform.position, centerPosition);

		// If the Distance is less than MinimapSize, it is within the Minimap view and we don't need to do anything
		// But if the Distance is greater than the MinimapSize, then do this
		if (Distance > MinimapRadius)
		{
			// Gameobject - Minimap
			Vector2 fromOriginToObject = (Vector2)transform.position - centerPosition;

			// Multiply by MinimapSize and Divide by Distance
			fromOriginToObject *= MinimapRadius / Distance;

			// Minimap + above calculation
			transform.position = centerPosition + fromOriginToObject;

			if (!isOut)
            {
				isOut = true;
				sr.sprite = outIcon;
			}

			float angle = GetAngle(centerPosition, transform.position);
			transform.rotation = Quaternion.Euler(0, 0, angle);

			return;
		}

		if (isOut){
			isOut = false;
			sr.sprite = inIcon;

			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	float GetAngle(Vector2 point1, Vector2 point2)
	{
		Vector2 delta = point1 - point2;
		return ((Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 180);
	}
}
