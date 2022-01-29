using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil
{

    public static Vector3[] FindPointsInLine(Vector3 start, Vector3 end, int points)
    {
        Vector3 line = end - start;
        Vector3[] pointList = new Vector3[points];
        points -= 1;
        for (int i = 0; i <= points; i++)
        {
            float percent = ((float)i) / (float)points;
            pointList[i] = start + ( percent * line );
            // Debug.Log("point" + i + "=" + start + ( percent * line ) + "\npercent=" + percent);
        }
        // Debug.Log("start=" + start + "\nend=" + end + "\npoints=" + points);
        return pointList;
    }

    public static Vector3 LerpByDistance(Vector3 start, Vector3 end, float percent)
    {
        Vector3 point = start + percent * (end - start);
        // Debug.Log("start=" + start + "\nend=" + end + "\npercent=" + percent + "\npoint=" + point);
        return point;
    }
}
