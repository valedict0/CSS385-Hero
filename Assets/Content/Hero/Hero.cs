using UnityEngine;

public class Hero : MonoBehaviour
{
    public Game game = null;
    public Egg eggPrefab = null;
    public float fireEggCooldown = 0.2f;
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 16.0f;

    private new Rigidbody2D rigidbody = null;

    private bool fireEgg = false;
    private float fireEggCooldownTime = 0.0f;

    private float rotate = 0.0f;

    private enum InputMode { Mouse, Keyboard }
    private InputMode inputMode = InputMode.Mouse;
    private bool inputModeToggle = false;
    private bool inputModeTogglePrev = false;
    private bool move = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move = !Mathf.Approximately(Input.GetAxis("Move"), 0.0f);
        fireEgg = !Mathf.Approximately(Input.GetAxis("Fire"), 0.0f);
        bool inputModeToggleCurr = !Mathf.Approximately(Input.GetAxis("ToggleInputMode"), 0.0f);
        if (inputModeToggleCurr != inputModeTogglePrev)
        {
            if (inputModeToggleCurr)
            {
                inputModeToggle = true;
            }
            inputModeTogglePrev = inputModeToggleCurr;
        }
        rotate = Input.GetAxis("Rotate");
    }

    private void FixedUpdate()
    {
        if (inputModeToggle)
        {
            // Invert inputMode.
            inputMode = inputMode == InputMode.Keyboard ? InputMode.Mouse : InputMode.Keyboard;
            inputModeToggle = false;
        }

        if (inputMode == InputMode.Mouse)
        {
            game.heroStatus = "Mouse";
            //Camera.main.ViewportToWorldPoint(Input.)
            Vector3 move_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            move_position.z = 0.0f;
            rigidbody.MovePosition(move_position);
        }
        else
        {
            game.heroStatus = "Keyboard";
            if (move)
            {
                float rotation = ((Mathf.PI * rigidbody.rotation) / 180.0f) + (0.5f * Mathf.PI);
                Vector2 move = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
                Vector2 movePosition = rigidbody.position + (move * moveSpeed * Time.fixedDeltaTime);
                rigidbody.MovePosition(movePosition);
            }
        }

        rigidbody.MoveRotation(rigidbody.rotation + (rotate * rotateSpeed * Time.fixedDeltaTime));

        if (fireEggCooldownTime > 0.0f)
        {
            fireEggCooldownTime -= Time.fixedDeltaTime;
            fireEggCooldownTime = Mathf.Max(0.0f, fireEggCooldownTime);
        } else if (fireEgg)
        {
            fireEggCooldownTime = fireEggCooldown;
            Egg egg = Instantiate(eggPrefab);
            egg.transform.position = transform.position;
            egg.transform.rotation = transform.rotation;
        }
        game.eggCooldown = fireEggCooldownTime;
    }
}
