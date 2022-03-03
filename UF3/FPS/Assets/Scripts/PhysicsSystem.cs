using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PsychroLib;

[CreateAssetMenu(fileName = "New World Physics", menuName = "Weapon/World Physics", order = 1)]
public class PhysicsSystem : ScriptableObject
{
    Psychrometrics physics;

    [Header("Entradas")]
    [Range(0, 100)]
    public float gravedad = 9.81f;
    [Range(0, 5000)]
    public double altura = 0;
    [Range(-100, 200)] 
    public double temperatura = 15;
    [Range(0,1)] 
    public double humedad = .5;

    [Header("Resultados")]
    [ReadOnly] public double presion;
    [ReadOnly] public double densidadDelAire;


	private void OnValidate()
	{
        if (physics == null)
            physics = new Psychrometrics(UnitSystem.SI);

        presion = 101325 * Math.Pow((1 - 0.0000225577 * altura), 5.2559);
        densidadDelAire = physics.GetMoistAirDensity(temperatura, physics.GetHumRatioFromRelHum(temperatura, humedad, presion), presion);
    }
}
