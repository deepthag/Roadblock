using UnityEngine;

public class GemBehavior : MonoBehaviour
{
    public float _rotationSpeed = 90f;
    void Start()
    {
        
    }
    
    void Update()
    {
        transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }
        
        if (other.gameObject.CompareTag("Player"))
        { 
            Destroy(gameObject);
            GameBehavior.Instance.ScorePoint();
        }
        
    }
}
