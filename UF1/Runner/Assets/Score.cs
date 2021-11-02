using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public PlayerController pc;

    void Start()
    {
        InvokeRepeating("updateText", 0, 0.1f);
    }

    private void updateText() // actualiza score, segun la puntuacion del player
    {
        this.GetComponent<TMPro.TextMeshProUGUI>().text = pc.score.ToString();
    }
}
