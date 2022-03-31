using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelpers
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector3 RadianToVector3(float radian)
    {
        return new Vector3(Mathf.Cos(radian), 0f, Mathf.Sin(radian));
    }

    public static Vector3 DegreeToVector3(float degree)
	{
        return RadianToVector3(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(float radian, float length)
    {
        return RadianToVector2(radian) * length;
    }

    public static Vector2 DegreeToVector2(float degree, float length)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad) * length;
    }

    public static Vector3 RadianToVector3(float radian, float length)
    {
        return RadianToVector3(radian) * length;
    }

    public static Vector3 DegreeToVector3(float degree, float length)
    {
        return RadianToVector3(degree * Mathf.Deg2Rad) * length;
    }
}
