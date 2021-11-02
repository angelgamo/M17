using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public PlayerController pc;

    void Start()
    {
        InvokeRepeating("updateText", 0, 0.1f);
    }

    private void updateText()
    {
        this.GetComponent<TMPro.TextMeshProUGUI>().text = pc.lives.ToString();
    }
}
