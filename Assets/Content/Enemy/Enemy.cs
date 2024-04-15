using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public Game game = null;
    public WaypointDatabase waypointDatabase = null;

    public float moveSpeed = 8.0f;
    public float rotateSpeed = 180.0f;
    public int health = 4;
    private int _health = 0;
    public float waypointDistanceThreshold = 1.0f;

    public UnityEvent destroyed;// temp. removing pool

    private SpriteRenderer spriteRenderer = null;
    private new Rigidbody2D rigidbody = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        Respawn();
    }

    private void Respawn()
    {
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

    private Waypoint _waypointTarget = null;
    private void FixedUpdate()
    {
        if (_health <= 0)
        {
            game.IncrementEnemyDestroyed();
            destroyed.Invoke();// Don't invoke in OnDestroy (scene deconstruction)
            Destroy(gameObject);
        }
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, (float)_health / (float)health);

        if (_waypointTarget == null)
        {
            GetNextWaypoint();
        }

        Vector2 targetPosition = new Vector2(_waypointTarget.transform.position.x, _waypointTarget.transform.position.y);
        if ((rigidbody.position - targetPosition).sqrMagnitude < (waypointDistanceThreshold * waypointDistanceThreshold))
        {
            GetNextWaypoint();
        }

        float rotationTarget = Vector2.SignedAngle(Vector2.up, targetPosition - rigidbody.position);
        float rotation = Mathf.MoveTowardsAngle(rigidbody.rotation, rotationTarget, Time.fixedDeltaTime * rotateSpeed);
        rigidbody.MoveRotation(rotation);
        
        float moveRotation = ((Mathf.PI * rotation) / 180.0f) + (0.5f * Mathf.PI);
        Vector2 move = new Vector2(Mathf.Cos(moveRotation), Mathf.Sin(moveRotation));
        Vector2 movePosition = rigidbody.position + (move * moveSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(movePosition);
    }

    private void GetNextWaypoint()
    {
        if (!game.sequentialMode || _waypointTarget == null)
        {
            int randomWaypoint = Random.Range(0, waypointDatabase.waypoints.Count);
            _waypointTarget = waypointDatabase.waypoints[randomWaypoint];
        } else
        {
            int waypointIndex = waypointDatabase.waypoints.IndexOf(_waypointTarget) - 1;
            int waypointIndexCount = waypointDatabase.waypoints.Count;
            waypointIndex = (Mathf.Abs(waypointIndex * waypointIndexCount) + waypointIndex) % waypointIndexCount;
            _waypointTarget = waypointDatabase.waypoints[waypointIndex];
        }
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
