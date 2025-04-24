using UnityEngine;

public class CameraFollower : MonoBehaviour
{
   public Transform player;
   Vector3 offset;
    void Start()
    {
        offset = transform.position - player.position;
    }

   
    void Update()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.x = 0;
        transform.position = targetPosition;
    }
}
