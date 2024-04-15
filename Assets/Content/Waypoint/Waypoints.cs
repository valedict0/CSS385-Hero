using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Game game;

    private void Update()
    {
        if (!game.hideMode)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        } else
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
