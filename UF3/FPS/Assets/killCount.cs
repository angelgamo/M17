using UnityEngine;

public class killCount : MonoBehaviour
{
    public TMPro.TextMeshProUGUI kills;
	int count;

	public void AddKill()
	{
		count++;
		kills.text = count.ToString();
	}
}
