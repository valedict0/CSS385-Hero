using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaypointDatabase", menuName = "Waypoint/Waypoint Database")]
public class WaypointDatabase : ScriptableObject
{
    [NonSerialized]
    public List<Waypoint> waypoints = new List<Waypoint>();
    private void OnEnable()
    {
        waypoints.Clear();
    }
}
