using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public Game game = null;
    public WaypointDatabase waypointDatabase = null;

    public Sprite planeTexture = null;
    public Sprite eggTexture = null;

    public float moveSpeed = 8.0f;
    public float rotateSpeed = 180.0f;
    public int health = 3;
    private int _health = 0;
    public float waypointDistanceThreshold = 1.0f;

    public UnityEvent destroyed;// temp. removing pool

    public float stunnedRotationSpeed = 120.0f;

    private SpriteRenderer spriteRenderer = null;
    private new Rigidbody2D rigidbody = null;

    private Transform _heroTransform = null;
    private bool _heroTouched = false;
    private bool _heroTrack = false;
    public float heroTrackTimeStartup = 2.0f;
    public float heroTrackRotationSpeed = 4800.0f;
    private float _heroTrackTimeStartup = 0.0f;
    public float heroTrackRadius = 20.0f;

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

    // eye sore below; scroll if you dare
    private void FixedUpdate()
    {
        if (_health <= 0)
        {
            game.IncrementEnemyDestroyed();
            destroyed.Invoke();// Don't invoke in OnDestroy (scene deconstruction)
            Destroy(gameObject);
        }
        Color color = spriteRenderer.color;
        Vector2 targetPosition = Vector2.zero;
        float speedMultiplier = 0.0f;
        float targetRotation = 0.0f;

        spriteRenderer.sprite = planeTexture;
        if (_health == 2)
        {
            targetRotation = rigidbody.rotation + stunnedRotationSpeed;
        }
        else if (_health == 1)
        {
            spriteRenderer.sprite = eggTexture;
        }
        else
        {
            if (_heroTouched && !_heroTrack)
            {
                _heroTouched = false;
                _heroTrack = true;
                _heroTrackTimeStartup = heroTrackTimeStartup;
            }

            if (_heroTrackTimeStartup > 0.0f)
            {
                speedMultiplier = 0.0f;
                // rotate counter clockwise, then clockwise
                if ((_heroTrackTimeStartup / heroTrackTimeStartup) > 0.5f)
                {
                    targetRotation = rigidbody.rotation + heroTrackRotationSpeed;
                }
                else
                {
                    targetRotation = rigidbody.rotation + -heroTrackRotationSpeed;
                }
                _heroTrackTimeStartup -= Time.fixedDeltaTime;
            }
            else if (_heroTrack)
            {
                if (_heroTransform == null)
                {
                    ContactFilter2D contactFilter = new ContactFilter2D();
                    contactFilter.useTriggers = true;
                    contactFilter.ClearDepth();
                    contactFilter.SetLayerMask(LayerMask.GetMask("Default"));
                    contactFilter.ClearNormalAngle();
                    RaycastHit2D[] results = new RaycastHit2D[16];
                    Physics2D.CircleCast(rigidbody.position, heroTrackRadius, Vector2.zero, contactFilter, results);
                    _heroTrack = false;
                    foreach (RaycastHit2D result in results)
                    {
                        if (result.transform != null && result.transform.CompareTag("Hero"))
                        {
                            _heroTransform = result.transform;
                            _heroTrack = true;
                            break;
                        }
                    }
                }
                else
                {
                    if ((transform.position - _heroTransform.position).magnitude > heroTrackRadius)
                    {
                        _heroTransform = null;
                        _heroTrack = false;
                    }
                    else
                    {
                        targetPosition = _heroTransform.position;
                        speedMultiplier = (transform.position - _heroTransform.position).magnitude / moveSpeed;
                    }
                }
                targetRotation = Vector2.SignedAngle(Vector2.up, targetPosition - rigidbody.position);
            }
            else
            {
                speedMultiplier = 1.0f;
                // waypoint stuff
                if (_waypointTarget == null)
                {
                    GetNextWaypoint();
                }
                targetPosition = new Vector2(_waypointTarget.transform.position.x, _waypointTarget.transform.position.y);
                if ((rigidbody.position - targetPosition).sqrMagnitude < (waypointDistanceThreshold * waypointDistanceThreshold))
                {
                    GetNextWaypoint();
                }
                targetRotation = Vector2.SignedAngle(Vector2.up, targetPosition - rigidbody.position);
            }
        }

        float rotation = Mathf.MoveTowardsAngle(rigidbody.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        rigidbody.MoveRotation(rotation);

        float moveRotation = ((Mathf.PI * rotation) / 180.0f) + (0.5f * Mathf.PI);
        Vector2 move = new Vector2(Mathf.Cos(moveRotation), Mathf.Sin(moveRotation));
        Vector2 movePosition = rigidbody.position + (speedMultiplier * move * moveSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(movePosition);

        spriteRenderer.color = new Color(color.r, color.g, color.b);//, (float)_health / (float)health);
    }

    private void GetNextWaypoint()
    {
        if (!game.sequentialMode || _waypointTarget == null)
        {
            int randomWaypoint = Random.Range(0, waypointDatabase.waypoints.Length);
            _waypointTarget = waypointDatabase.waypoints[randomWaypoint];
        }
        else
        {
            int waypointIndex = 0;
            for (; waypointIndex < waypointDatabase.waypoints.Length; ++waypointIndex)
            {
                if (waypointDatabase.waypoints[waypointIndex] == _waypointTarget)
                {
                    break;
                }
            }
            waypointIndex += 1;
            int waypointIndexCount = waypointDatabase.waypoints.Length;
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
            game.IncrementEnemyTouched();
            _heroTouched = true;
        }
    }
}
