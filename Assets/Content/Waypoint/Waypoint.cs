using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Game game = null;
    public WaypointDatabase database;

    public int index = 0;
    public int health = 4;
    private int _health = 4;
    public float respawnOffset = 2.0f;

    private SpriteRenderer spriteRenderer = null;

    private Vector3 _positionStart = Vector3.zero;

    public void Respawn()
    {
        _health = health;
        float randomAngle = Random.Range(0.0f, Mathf.PI * 2.0f);
        float randomDistance = Random.Range(0.0f, 1.0f) * respawnOffset;
        Vector3 randomOffset = randomDistance * new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0.0f);
        transform.position = _positionStart + randomOffset;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        database.waypoints[index] = this;
        _positionStart = transform.position;
        _health = health;
    }

    private void OnDestroy()
    {
        database.waypoints[index] = null;
    }

    private void FixedUpdate()
    {
        if (_health <= 0)
        {
            Respawn();
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
