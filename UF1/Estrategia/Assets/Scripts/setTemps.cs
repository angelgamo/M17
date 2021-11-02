using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTemps : MonoBehaviour
{
    int segons = 0;
    int minuts = 0;
    int hores = 0;
    public NomJugador nj;
    void Start()
    {
        StartCoroutine(ContarTemps());
    }

    IEnumerator ContarTemps()
    {
        while (true)
        {
            segons++;
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(segons >= 60)
        {
            minuts++;
            segons = 0;
        }
        if(minuts >= 60)
        {
            hores++;
            minuts = 0;
        }
        string temps = "00:00:00";
        string s = segons + "";
        if(segons < 10)
        {
            s = "0" + s;
        }
        string m = minuts + "";
        if (minuts < 10)
        {
            m = "0" + m;
        }
        string h = hores + "";
        if (hores < 10)
        {
            h = "0" + h;
        }

        temps = h + ":" + m + ":" + s;

        this.GetComponent<TMPro.TextMeshProUGUI>().text = "Temps: " + temps;
        nj.temps = temps;
    }
}
