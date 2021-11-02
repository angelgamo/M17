using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public float time;
    private SpriteRenderer spriteRenderer;
    private Color newColor;
    private PlayerController.ShootType shootType;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        Invoke("colorYellow", 0);
        spriteRenderer.color = Color.cyan;
    }

    private void Update() // cambio de color
    {
        float pingPong = Mathf.PingPong(Time.time, 5)/3 - .33f;
        float pingPongClamp = Mathf.Clamp(pingPong, 0, 1);
        spriteRenderer.color = Color.Lerp(Color.cyan, Color.yellow, pingPongClamp);
        shootType = pingPongClamp >= 0.5f ? PlayerController.ShootType.laser : PlayerController.ShootType.ball;
    }

    public PlayerController.ShootType getShootType()
    {
        return shootType;
    }
}
