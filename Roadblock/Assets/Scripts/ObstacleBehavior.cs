using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.ResetGame();
        }
    }
}
