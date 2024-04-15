using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaypointDatabase", menuName = "Waypoint/Waypoint Database")]
public class WaypointDatabase : ScriptableObject
{
    [NonSerialized]
    public Waypoint[] waypoints = new Waypoint[6];
    private void OnEnable()
    {
        waypoints = new Waypoint[6];
    }
}
