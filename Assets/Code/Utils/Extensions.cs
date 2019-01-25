using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{
    public static Vector3 FindCenterPoint(this IEnumerable<Vector3> points)
    {
        var count = points.Count();
        switch (count)
        {
            case 0:
                return Vector3.zero;
            default:
                return points.Aggregate((s, v) => s + v) / count;
        }
    }

    public static Vector2 ToSceenPosition(this Vector3 worldPosition)
    {
        return new Vector2(worldPosition.x, worldPosition.z);
    }

    public static Vector3 ToWorldPosition(this Vector2 screenPosition)
    {
        return new Vector3(screenPosition.x, 0, screenPosition.y);
    }
}
