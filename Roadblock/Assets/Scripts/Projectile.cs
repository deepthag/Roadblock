using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Destroying obstacle");

            Destroy(collision.gameObject); // Destroy the obstacle
            Destroy(gameObject); // Destroy the projectile
        }
        else
        {
            Destroy(gameObject); // Destroy projectile on any collision
        }
    }
}