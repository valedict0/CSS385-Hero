using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public Game game = null;
    public WaypointDatabase waypointDatabase = null;

    public float distanceToWaypoint = 0.25f;

    public int health = 4;
    private int _health = 0;

    public UnityEvent destroyed;
    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _health = health;
    }

    private void OnEnable()
    {
        game.IncrementEnemyCount();
    }

    private void OnDisable()
    {
        game.DecrementEnemyCount();
    }

    private void FixedUpdate()
    {
        if (_health <= 0)
        {
            game.IncrementEnemyDestroyed();
            destroyed.Invoke();// Don't invoke in OnDestroy (scene deconstruction)
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
        if (collision.CompareTag("Hero"))
        {
            _health = 0;
            game.IncrementEnemyTouched();
        }
    }
}
