using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public PlayerController player;

    [Header("UI")]
    public TextMeshProUGUI speed;
    public TextMeshProUGUI speedVer;

    void Update()
	{
        speed.text = "Speed: " + Round(new Vector2(player.rb.velocity.x, player.rb.velocity.z).magnitude);
        speedVer.text = "Speed Ver: " + Round(player.rb.velocity.y);
    }

    float Round(float value)
	{
        return Mathf.Round(value * 100.0f) * 0.01f;
	}
}
