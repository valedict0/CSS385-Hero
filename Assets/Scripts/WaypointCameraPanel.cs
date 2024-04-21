using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointCameraPanel : MonoBehaviour
{
    [SerializeField]
    private Game _game = null;

    [SerializeField]
    public RawImage disabledImage = null;

    public float focusTime = 3.0f;
    private float _focusTime = 0.0f;

    private void OnEnable()
    {
        _game.WaypointHit += OnGameWaypointHit;
    }
    private void OnDisable()
    {
        _game.WaypointHit -= OnGameWaypointHit;
    }
    private void FixedUpdate()
    {
        if (_focusTime > 0.0f)
        {
            disabledImage.gameObject.SetActive(false);
            _focusTime -= Time.fixedDeltaTime;
        } else
        {
            disabledImage.gameObject.SetActive(true);
        }
    }

    private void OnGameWaypointHit(Transform waypoint)
    {
        _focusTime = focusTime;
    }
}
