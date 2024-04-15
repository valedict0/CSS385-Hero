using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Game game = null;

    private bool _sequentialToggleOnce = false;
    private bool _pauseToggleOnce = false;
    private bool _hideToggleOnce = false;
    private void Update()
    {
        // Pause
        if (Mathf.Approximately(Input.GetAxisRaw("Pause"), 0.0f))
        {
            _pauseToggleOnce = false;
        }
        else if (!_pauseToggleOnce)
        {
            _pauseToggleOnce = true;
            game.pauseMode = !game.pauseMode;
            Time.timeScale = game.pauseMode ? 0.0f : 1.0f;
        }

        // Sequential
        if (Mathf.Approximately(Input.GetAxisRaw("Sequential"), 0.0f))
        {
            _sequentialToggleOnce = false;
        }
        else if (!_sequentialToggleOnce)
        {
            _sequentialToggleOnce = true;
            game.sequentialMode = !game.sequentialMode;
        }

        // Hide
        if (Mathf.Approximately(Input.GetAxisRaw("Hide"), 0.0f))
        {
            _hideToggleOnce = false;
        }
        else if (!_hideToggleOnce)
        {
            _hideToggleOnce = true;
            game.hideMode = !game.hideMode;
        }
    }
}
