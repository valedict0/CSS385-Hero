using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Game game = null;
    public WaypointDatabase database;

    public int index = 0;
    public int health = 4;
    private int _health = 4;
    public float respawnOffset = 2.0f;
    public float shakeStrength = 0.125f;
    public float shakeTime = 1.0f;
    private float _shakeStrengthMultiplier = 1.0f;
    private float _shakeTime = 0.0f;
    

    private SpriteRenderer spriteRenderer = null;

    private Vector3 _positionStart = Vector3.zero;
    private Vector3 _positionSpawned = Vector3.zero;

    public void Respawn()
    {
        _health = health;
        float randomAngle = Random.Range(0.0f, Mathf.PI * 2.0f);
        float randomDistance = Random.Range(0.0f, 1.0f) * respawnOffset;
        Vector3 randomOffset = randomDistance * new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0.0f);
        transform.position = _positionStart + randomOffset;
        _positionSpawned = transform.position;
        _justHit = false;
        _shakeTime = 0.0f;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        database.waypoints[index] = this;
        _positionStart = transform.position;
        _positionSpawned = transform.position;
        _health = health;
    }

    private void OnDestroy()
    {
        database.waypoints[index] = null;
    }

    private bool _justHit = false;
    private void FixedUpdate()
    {
        if (_shakeTime > 0.0f)
        {
            // randomize angle, but keep magnitude consistent
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            float shakeTaper = _shakeTime / (shakeTime * _shakeStrengthMultiplier);// taper off with time
            Vector3 shakeOffset = shakeTaper * shakeStrength * _shakeStrengthMultiplier * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
            transform.position = _positionSpawned + shakeOffset;
            _shakeTime -= Time.fixedDeltaTime;
        }
        if (_health <= 0)
        {
            Respawn();
        } else if (_justHit)
        {
            float shakeStrengthMultiplier = 1.0f / ((float)_health / (float)health);
            _shakeStrengthMultiplier = shakeStrengthMultiplier;
            _shakeTime = shakeTime * shakeStrengthMultiplier;
            _justHit = false;
        }
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, (float)_health / (float)health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            --_health;
            _justHit = true;
            game.InvokeWaypointHit(transform);
        }
    }
}
