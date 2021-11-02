using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarCoste : MonoBehaviour
{
    public BuildingController bc;
    void Start()
    {
        bc.onBuildCost += MostrarCost;
        bc.onClearBuildCost += ClearCost;
    }

    void MostrarCost(int cLog, int cGold, int cStone)
    {
        GameObject text = transform.GetChild(1).gameObject;
        text.GetComponent<TMPro.TextMeshProUGUI>().text = "Logs: " + cLog + "\n" +
                                                          "Gold: " + cGold + "\n" +
                                                          "Rock: " + cStone;
    }

    void ClearCost()
    {
        GameObject text = transform.GetChild(1).gameObject;
        text.GetComponent<TMPro.TextMeshProUGUI>().text = "Logs: " + "\n" +
                                                          "Gold: " + "\n" +
                                                          "Rock: ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
