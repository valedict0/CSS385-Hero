using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public WaypointDatabase database;

    public int health = 3;
    private int _health = 3;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _health = health;
    }

    private void OnEnable()
    {
        database.waypoints.Add(this);
    }

    private void OnDisable()
    {
        database.waypoints.Remove(this);
    }

    private void FixedUpdate()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, (float)_health / (float)health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            --_health;
        }
    }
}
