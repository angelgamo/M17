using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRespawnUI : MonoBehaviour
{
    public FoodRespawn food;
	public TMPro.TMP_InputField foodText;

	private void Start()
	{
		foodText.text = food.timeToRespawn.ToString();
	}

	public void FoodRespawn(string time)
	{
		food.timeToRespawn = float.Parse(time);
	}
}
