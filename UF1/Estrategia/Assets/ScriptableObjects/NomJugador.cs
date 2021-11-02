using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NomJugador", order = 1)]
public class NomJugador : ScriptableObject
{
    public string nom;
    public int punts = 0;
    public string temps;
}
