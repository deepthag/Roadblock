using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;

    public float _forwardSpeed = 10.0f;
    public float _sideMultiplier = 1.2f;
    public float _forwardSpeedIncrease = 0.1f;
    public float _maxForwardSpeed = 20.0f;

    public KeyCode RightDirection = KeyCode.RightArrow;
    public KeyCode LeftDirection = KeyCode.LeftArrow;
    public KeyCode UpDirection = KeyCode.UpArrow;
    
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce = 20f;

    public TMPro.TextMeshProUGUI gameOverText;
    public TMPro.TextMeshProUGUI playAgainText;
    
    [SerializeField] private float jumpForce = 8f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!alive)
        {
            if (isGameOver && Input.GetKeyDown(KeyCode.Return))
            {
                RoadBrick.roadBricksSpawned = 0;
                GameBehavior.Instance.State = Utilities.GameplayState.Play;
                SceneManager.LoadScene("Roadblock");
            }
            return;
        }

        // Handle movement and jumping
        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            // Increase forward speed over time
            _forwardSpeed += _forwardSpeedIncrease * Time.deltaTime;
            _forwardSpeed = Mathf.Clamp(_forwardSpeed, 10.0f, _maxForwardSpeed);
            transform.position += Vector3.forward * _forwardSpeed * Time.deltaTime;

            // Side movement (left/right)
            if (Input.GetKey(RightDirection))
                transform.position += Vector3.right * _forwardSpeed * _sideMultiplier * Time.deltaTime;

            if (Input.GetKey(LeftDirection))
                transform.position += Vector3.left * _forwardSpeed * _sideMultiplier * Time.deltaTime;

            // Jumping mechanic
            if (Input.GetKeyDown(UpDirection) && isGrounded)
            {
                // Reset vertical velocity before jump to ensure a clean jump
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); 

                // Add force upwards to simulate the jump
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // Mark the player as not grounded after jumping
                isGrounded = false;
            }
            
            // Shooting
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootProjectile();
            }

            // Fall below death line (trigger game over)
            if (transform.position.y < -10)
                TriggerGameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            TriggerGameOver(); 
        }

        // Check if the player is on the ground (touching road brick)
        if (collision.gameObject.CompareTag("RoadBrick"))
        {
            isGrounded = true; // The player is grounded if they collide with a road brick
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Player is no longer grounded if they leave the road brick
        if (collision.gameObject.CompareTag("RoadBrick"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TriggerGameOver(); 
        }
    }

    void TriggerGameOver()
    {
        GameBehavior.Instance.State = Utilities.GameplayState.GameOver;
        alive = false;
        isGameOver = true;
        
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
        
        if (playAgainText != null)
            playAgainText.gameObject.SetActive(true);
    }
    
    void ShootProjectile()
    {
        if (projectilePrefab == null || projectileSpawnPoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();

        if (projRb != null)
        {
            projRb.AddForce(Vector3.forward * projectileForce, ForceMode.Impulse);
        }
    }

}
