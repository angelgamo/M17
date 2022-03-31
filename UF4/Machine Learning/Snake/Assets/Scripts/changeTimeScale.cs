using UnityEngine;

public class changeTimeScale : MonoBehaviour
{
	public TMPro.TextMeshProUGUI scale;

    public void TimeScale(float scale)
	{
		Time.timeScale = scale;
		this.scale.text = scale.ToString("0.##");
	}
}
