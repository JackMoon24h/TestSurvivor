using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
