using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Block : MonoBehaviour
{
    (float min, float max) y = (-2f, 7f);
    (float min, float max) x = (0f, 40.25f);
    public float offset;
    public float speed = 5f;

    [Button]
    public void SetColor()
	{
        foreach (Transform block in transform)
		{
            if (!block.gameObject.activeSelf)
                continue;

            float yPos = block.localPosition.y;
            float xPos = block.localPosition.x + offset;
            var odd = Mathf.FloorToInt(xPos / x.max) % 2 != 0;
            xPos = odd ? x.max - xPos % x.max : xPos % x.max;
            float yASC = Mathf.InverseLerp(y.min, y.max, yPos);
            float yDES = Mathf.InverseLerp(y.max, y.min, yPos);
            float xASC = Mathf.InverseLerp(x.min, x.max, xPos);
            float xDES = Mathf.InverseLerp(x.max, x.min, xPos);
            float xASCClamp = Mathf.Clamp(xASC, 0f, .5f);
            float xDESClamp = Mathf.Clamp(xDES, 0f, .5f);
            Color c = new Color(yASC * Mathf.Lerp(.5f, 1f, xASCClamp + xDESClamp), (yDES + xDESClamp * .5f) * xDES, (yDES + xASCClamp * .5f) * xASC);
            block.GetComponent<SpriteRenderer>().color = c;
        }
    }

	private void Update()
	{
        offset += speed * Time.deltaTime;
        SetColor();
    }
}
