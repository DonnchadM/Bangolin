using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poacher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float detectionRange = 50f; 
    public float projectileSpeed = 2f; 
    public float fireRate = 1f; 

    private Transform playerTransform;
    private float nextFireTime = 0f;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Pangolin");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found in the scene!");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector2 directionToPlayer = playerTransform.position - transform.position;

            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);

            if (directionToPlayer.magnitude <= detectionRange && Time.time >= nextFireTime)
            {
                ShootProjectile(directionToPlayer.normalized);
                nextFireTime = Time.time + fireRate;
            }
        }
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

            playerHealth playerHealthScript = collision.gameObject.GetComponent<playerHealth>();

            if (playerHealthScript != null)
            {
                playerHealthScript.takeHit(1);
            }
        }
    }

    void ShootProjectile(Vector2 direction)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
        else
        {
            Debug.LogError("Projectile prefab or firePoint not assigned!");
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
