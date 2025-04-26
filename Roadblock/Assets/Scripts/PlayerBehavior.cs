using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    
    public float _forwardSpeed = 10.0f;
    public float _sideMultiplier = 1.2f;
    
    public KeyCode RightDirection = KeyCode.RightArrow;
    public KeyCode LeftDirection = KeyCode.LeftArrow;
    
    [SerializeField] private float _resetTime = 0.5f;
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
            StartCoroutine(ResetGame());
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            StartCoroutine(ResetGame());
        }
    }
    public IEnumerator ResetGame()
    {
        alive = false;
        yield return new WaitForSeconds(_resetTime);
        RoadBrick.roadBricksSpawned = 0;
        SceneManager.LoadScene("Roadblock");
    }
}
