using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState { UPRIGHT, ROLLING, SLIDING }
    public PlayerState currentState = PlayerState.UPRIGHT;

    public float moveSpeed = 5f;
    public float rollingSpeed = 15f;
    public float acceleration = 10f;
    public float rollingAcceleration = 5f;
    public float deceleration = 15f;
    public float rollingDeceleration = 5f;
    public float turnDeceleration = 20f;
    public float jumpForce = 10f;
    public float slidingSpeed = 4f;
    public float rollingDuration = 3f;
    public float wallJumpHeight = 6f;

    private float rollingTimer = 0f;
    private bool canSwitchState = true;
    private bool willWallJump = false;
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
    private GameSystem systemScript;
    private playerHealth healthScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        animator = GetComponent<Animator>();
        GameSystem = GameObject.Find("GameSystem");
        systemScript = GameSystem.GetComponent<GameSystem>();
        systemScript.enabled = true;
        healthScript = GetComponent<playerHealth>();
    }

    void Update()
    {
        if (currentState != PlayerState.SLIDING)
        {
            HandleStateSwitching();
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (currentState == PlayerState.SLIDING)
        {
            spriteRenderer.sprite = ball;
            HandleSlidingState();
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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

    private void HandleSlidingState()
    {
        if (spriteRenderer.sprite != ball)
        {
            spriteRenderer.sprite = ball;
        }

        rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * slidingSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            willWallJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength);
        if (isGrounded && !OnIce())
        {
            currentState = PlayerState.UPRIGHT;
            willWallJump = false;
            spriteRenderer.sprite = defaultSprite;
        }
    }

    private bool OnIce()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength);
        if (hit.collider != null && hit.collider.CompareTag("Ice"))
        {
            return true;
        }
        return false;
    }

    private void HandleStateSwitching()
    {
        if (OnIce())
        {
            currentState = PlayerState.SLIDING;
            return;
        }

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
            Destroy(other.gameObject);
            systemScript.addCoin(1);
        }
        else if (other.CompareTag("Star"))
        {
            systemScript.beatLevel();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == PlayerState.SLIDING)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y))
                {
                    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);

                    float newScaleX = transform.localScale.x * -1;
                    transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
                    if (willWallJump == true && !isGrounded)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, wallJumpHeight);
                        willWallJump = false;
                        break;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }

    private void powerUpSelect(int option)
    {
        string powerFound = systemScript.getPowerPos(option);

        if (!systemScript.powerUpInventory.ContainsKey(powerFound))
        {
            return;
        }

        int quantity = systemScript.getPowerUpQuantity(powerFound);

        if (quantity >= 1)
        {
            switch (powerFound)
            {
                case "Zoom":
                    Zoom();
                    break;
                case "'NotherShell":
                    healthScript.addHit(1);
                    break;
                case "Zap":
                    Zap();
                    break;
            }

            systemScript.usePowerUp(powerFound);
        }
    }

    private void Zoom()
    {
        rb.velocity = new Vector2(rb.velocity.x, 12);
    }

    private void Zap()
    {
        float teleportDistance = 5f;
        Vector3 teleportDirection = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        transform.position += teleportDirection * teleportDistance;
    }
}
