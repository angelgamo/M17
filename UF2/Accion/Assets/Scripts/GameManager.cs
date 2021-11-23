using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] int allyLayer;
    [SerializeField] int enemyLayer;

    [Header("Layer Setter")]
    [SerializeField] Damage[] ally;
    [SerializeField] Damage[] enemy;

    [Header("Damage Setter")]
    [SerializeField, Range(0,100)] int damage1;
    [SerializeField] Damage[] list1;
    [SerializeField, Range(0, 100)] int damage2;
    [SerializeField] Damage[] list2;
    [SerializeField, Range(0, 100)] int damage3;
    [SerializeField] Damage[] list3;
    [SerializeField, Range(0, 100)] int damage4;
    [SerializeField] Damage[] list4;

    private void Start()
    {
        foreach (Damage d in ally) d.enemyLayer = enemyLayer;
        foreach (Damage d in enemy) d.enemyLayer = allyLayer;

        foreach (Damage d in list1) d.damage = damage1;
        foreach (Damage d in list2) d.damage = damage2;
        foreach (Damage d in list3) d.damage = damage3;
        foreach (Damage d in list4) d.damage = damage4;
    }
}
