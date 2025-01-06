using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherBullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 6, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8, true);

        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 0)
        {
            playerHealth playerHealthScript = collision.gameObject.GetComponent<playerHealth>();

            if (playerHealthScript != null)
            {
                playerHealthScript.takeHit(damage);
                Flip();
            }

            Destroy(gameObject);
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
