using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }
    
}
