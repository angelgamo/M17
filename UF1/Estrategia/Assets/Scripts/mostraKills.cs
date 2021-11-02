using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mostraKills : MonoBehaviour
{
    public NomJugador data;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TMPro.TextMeshProUGUI>().text = "Kills: " + data.punts;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
