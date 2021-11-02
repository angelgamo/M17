using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mostraTemps : MonoBehaviour
{
    public NomJugador data;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TMPro.TextMeshProUGUI>().text = "Temps: " + data.temps;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
