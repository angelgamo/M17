using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setKills : MonoBehaviour
{
    public NomJugador data;
    int kills = 0;
    void Start()
    {
        data.punts = kills;
    }

    // Update is called once per frame
    void Update()
    {
        kills = data.punts;
        this.GetComponent<TMPro.TextMeshProUGUI>().text = "Kills: " + kills;
    }
}
