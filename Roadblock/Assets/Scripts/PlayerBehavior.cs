using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    
    public float _forwardSpeed = 10.0f;
    public float _sideMultiplier = 1.2f;
    public KeyCode RightDirection = KeyCode.RightArrow;
    public KeyCode LeftDirection = KeyCode.LeftArrow;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (!alive) return;
        
        transform.position += Vector3.forward * _forwardSpeed * Time.deltaTime;
        
        if (Input.GetKey(RightDirection))
        {
            transform.position += Vector3.right * _forwardSpeed * _sideMultiplier * Time.deltaTime;
        }
        if (Input.GetKey(LeftDirection))
        {
            transform.position += Vector3.left * _forwardSpeed * _sideMultiplier * Time.deltaTime; 
        }

        if (transform.position.y < -10)
        {
            ResetGame();
        }
        
    }

    public void ResetGame()
    {
        alive = false;
        SceneManager.LoadScene("Roadblock");
    }
}
