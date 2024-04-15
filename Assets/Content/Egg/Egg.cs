using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Egg : MonoBehaviour
{
    public Game game = null;
    public float speed = 40.0f;

    private new Rigidbody2D rigidbody = null;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get Orthographic Camera bounds in World Space. If outside bounds, destroy self.
        Vector2 boundsOrigin = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        Vector2 boundsSize = Camera.main.orthographicSize * 2.0f * new Vector2(aspectRatio, 1.0f);
        Rect bounds = new Rect(boundsOrigin - (boundsSize / 2.0f), boundsSize);
        if (!bounds.Contains(rigidbody.position))
        {
            Destroy(gameObject);
        }

        // Move linearly at constant speed in direction of rotation.
        float rotation = ((Mathf.PI * rigidbody.rotation) / 180.0f) + (0.5f * Mathf.PI);
        Vector2 move = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
        Vector2 movePosition = rigidbody.position + (move * speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(movePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Waypoint"))
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (game != null)
        {
            game.IncrementEggCount();
        }
    }

    private void OnDisable()
    {
        if (game != null)
        {
            game.DecrementEggCount();
        }
    }
}
