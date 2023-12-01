using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Transform[] waypoints = GetWaypoints();

        if (waypoints.Length < 2)
        {
            Debug.LogWarning("Not enough waypoints to draw a line.");
            return;
        }

        Gizmos.color = Color.blue;

        for (int i = 1; i < waypoints.Length; i++)
        {
            Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
        }

        // Connect the last waypoint to the first one to complete the loop
        Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
    }

    Transform[] GetWaypoints()
    {
        // Assumes waypoints are direct children of this GameObject
        Transform[] waypoints = new Transform[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }

        return waypoints;
    }
}
