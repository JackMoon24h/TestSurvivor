using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Utility 
{
    public static Vector3 V3Round(Vector3 point)
    {
        return new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));
    }

    public static Vector2 V2Round(Vector2 point)
    {
        return new Vector2(Mathf.Round(point.x), Mathf.Round(point.y));
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {

        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    public static float StatFloatRound(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }

    public static int StatIntRound(float value)
    {
        return (int)Mathf.Round(value);
    }
}
