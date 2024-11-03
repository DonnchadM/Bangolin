using System.Linq.Expressions;
using UnityEngine;

public class HyenaMovement : MonoBehaviour
{
    public float speed = 2f;
    public float checkDistance = 0.5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isMovingRight = true;
    private Vector2 wallCheckOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            wallCheckOffset = new Vector2(boxCollider.bounds.extents.x + 0.1f, 0);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(isMovingRight ? speed : -speed, rb.velocity.y);
        CheckForObstacles();
    }

    void CheckForObstacles()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y) + (isMovingRight ? wallCheckOffset : -wallCheckOffset);
        RaycastHit2D wallCheck = Physics2D.Raycast(rayOrigin, isMovingRight ? Vector2.right : Vector2.left, checkDistance, groundLayer);
        Debug.DrawRay(rayOrigin, isMovingRight ? Vector2.right * checkDistance : Vector2.left * checkDistance, Color.red);
        RaycastHit2D groundCheckHit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);

        if (wallCheck.collider != null || groundCheckHit.collider == null)
        {
            Flip();
        }
    }

    void Flip()
    {
        isMovingRight = !isMovingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;

            if (collisionNormal.y <= -0.5f)
            {
                jumpedOn(collision.gameObject);
                return;
            }

            Debug.Log("bam!");
            playerHealth playerHealthScript = collision.gameObject.GetComponent<playerHealth>();
            
            if (playerHealthScript != null)
            {
                playerHealthScript.takeHit(1);
                Flip();
            }
        }
    }

    private void jumpedOn(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3000);
        killMe();
    }
    
    private void killMe()
    {
        Destroy(this.gameObject);
    }
}
