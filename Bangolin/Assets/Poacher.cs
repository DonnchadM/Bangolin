using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poacher : MonoBehaviour
{
    private Transform playerTransform;

    void Start()
    {
        
    }

    void Update()
    {

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
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;

            Vector2 directionToPlayer = playerTransform.position - transform.position;

            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
