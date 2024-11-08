using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState { UPRIGHT, ROLLING }
    public PlayerState currentState = PlayerState.UPRIGHT;

    public float moveSpeed = 5f;
    public float rollingSpeed = 15f;
    public float acceleration = 10f;
    public float rollingAcceleration = 5f;
    public float deceleration = 15f;
    public float rollingDeceleration = 5f;
    public float turnDeceleration = 20f;
    public float jumpForce = 10f;

    public float rollingDuration = 3f;
    private float rollingTimer = 0f;
    private bool canSwitchState = true;

    public Sprite ball;
    private Sprite defaultSprite;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float moveInput;
    private float currentSpeed = 0f;

    public float rayLength = 0.2f;

    private GameObject GameSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        animator = GetComponent<Animator>();
        GameSystem = GameObject.Find("GameSystem");
    }

    void Update()
    {
        HandleStateSwitching();
        moveInput = Input.GetAxisRaw("Horizontal");

        float maxSpeed = (currentState == PlayerState.ROLLING) ? rollingSpeed : moveSpeed;
        float accelerationRate = (currentState == PlayerState.ROLLING) ? rollingAcceleration : acceleration;
        float decelerationRate = (currentState == PlayerState.ROLLING) ? rollingDeceleration : deceleration;

        if (moveInput != 0)
        {
            if (Mathf.Sign(moveInput) != Mathf.Sign(currentSpeed) && currentSpeed != 0)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, turnDeceleration * Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * moveInput, accelerationRate * Time.deltaTime);
            }
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, decelerationRate * Time.deltaTime);
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            animator.SetBool("isWalking", true);
            animator.speed = Mathf.Abs(rb.velocity.x) / maxSpeed;
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.speed = 0f;
        }

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleStateSwitching()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentState == PlayerState.UPRIGHT && canSwitchState)
        {
            currentState = PlayerState.ROLLING;
            rollingTimer = rollingDuration;
            canSwitchState = false;
            spriteRenderer.sprite = ball;

            if (animator != null)
            {
                animator.enabled = false;
            }
        }

        if (currentState == PlayerState.ROLLING)
        {
            rollingTimer -= Time.deltaTime;

            if (rollingTimer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    currentState = PlayerState.UPRIGHT;
                    canSwitchState = true;
                    spriteRenderer.sprite = defaultSprite;

                    if (animator != null)
                    {
                        animator.enabled = true;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("hihihi");
            Destroy(other.gameObject);
            GameSystem.GetComponent<GameSystem>().addCoin(1);
        }
        else if (other.CompareTag("Star")){
            Debug.Log("Star");
            GameSystem.GetComponent<GameSystem>().beatLevel();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }
}
