using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float _forwardSpeed = 1.0f;
    public float _sideMultiplier = 2.0f;
    public KeyCode RightDirection = KeyCode.RightArrow;
    public KeyCode LeftDirection = KeyCode.LeftArrow;
    void Start()
    {
        
    }
    
    void Update()
    {
        transform.position += Vector3.forward * _forwardSpeed * Time.deltaTime;
        
        if (Input.GetKey(RightDirection))
        {
            transform.position += Vector3.right * _forwardSpeed * _sideMultiplier * Time.deltaTime;
        }
        if (Input.GetKey(LeftDirection))
        {
            transform.position += Vector3.left * _forwardSpeed * _sideMultiplier * Time.deltaTime; 
        }
        
    }
}
