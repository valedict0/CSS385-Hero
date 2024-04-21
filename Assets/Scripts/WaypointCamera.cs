using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCamera : MonoBehaviour
{
    [SerializeField]
    private Game _game = null;

    private void OnEnable()
    {
        _game.WaypointHit += OnGameWaypointHit;
    }
    private void OnDisable()
    {
        _game.WaypointHit -= OnGameWaypointHit;
    }

    private void OnGameWaypointHit(Transform waypoint)
    {
        transform.position = new Vector3(waypoint.position.x, waypoint.position.y, transform.position.z);
    }
}
