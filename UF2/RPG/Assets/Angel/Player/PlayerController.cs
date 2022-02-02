using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController current;

    private void Awake()
    {
        current = this;
    }
    #endregion

    [Header("Keybinds")]
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode dash;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float dashForce;
    float speedValue;
    float maxSpeedValue;
    float dashForceValue;

    [Header("Extras")]
    Animator anim;
    Rigidbody2D rb;
    Vector2 movement;
    bool isFlipped;
    bool changingDirectionX;
    bool changingDirectionY;
    PlayerCharacterStats playerCharacterStats;
    [HideInInspector] public bool lookAtMouse = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCharacterStats = PlayerCharacterStats.instance;
    }

    private void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(left)) movement.x = -1;
        else if (Input.GetKey(right)) movement.x = 1;

        if (Input.GetKey(up)) movement.y = 1;
        else if (Input.GetKey(down)) movement.y = -1;

        if (Input.GetKeyDown(dash))
        {
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(left)) direction += Vector2.left;
            else if (Input.GetKey(right)) direction += Vector2.right;

            if (Input.GetKey(up)) direction += Vector2.up;
            else if (Input.GetKey(down)) direction += Vector2.down;

            if (direction == Vector2.zero)
                return;

            if (playerCharacterStats.UseStamina(20))
                Dash(direction);
        }
    }

    private void FixedUpdate()
    {
        changingDirectionX = (rb.velocity.x > 0f && movement.x < 0f || rb.velocity.x < 0f && movement.x > 0f);
        changingDirectionY = (rb.velocity.y > 0f && movement.y < 0f || rb.velocity.y < 0f && movement.y > 0f);

        if (Mathf.Abs(Vector2.SqrMagnitude(rb.velocity)) < maxSpeedValue && movement != Vector2.zero || changingDirectionX || changingDirectionY) 
            rb.AddForce(movement * speedValue * Time.deltaTime);

        // Sprite things
        anim.SetBool("isRunning", (movement != Vector2.zero));
        
        if (lookAtMouse)
            LookAtMouse();
    }

    void LookAtMouse()
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= -1f;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (transform.position.x > mouse.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        else if (transform.position.x < mouse.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
    }

    void Dash(Vector2 direction)
    {
        rb.AddForce(direction.normalized * dashForceValue, ForceMode2D.Impulse);
    }

    public void UpdateMovement(float speed)
    {
        speedValue = speed * 60;
        maxSpeedValue = speed;
        dashForceValue = speed * 3/4;
    }
}
