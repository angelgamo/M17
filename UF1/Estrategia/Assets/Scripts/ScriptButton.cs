using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptButton : MonoBehaviour
{
    public GameObject text;
    public NomJugador nj;
   

    public void Click()
    {
        string n = text.GetComponent<TMPro.TextMeshProUGUI>().text;
        nj.nom = n;

        SceneManager.LoadScene(1);
    }
}
