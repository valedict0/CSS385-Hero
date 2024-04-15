using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool pause = false;
    private bool pauseToggleOnce = false;
    private void Update()
    {
        bool pauseToggle = !Mathf.Approximately(Input.GetAxisRaw("Pause"), 0.0f);
        if (!pauseToggle)
        {
            pauseToggleOnce = false;
        } else if (!pauseToggleOnce)
        {
            pauseToggleOnce = true;
            pause = !pause;
            Time.timeScale = pause ? 0.0f : 1.0f;
        }
    }
}
