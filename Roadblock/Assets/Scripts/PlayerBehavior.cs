using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    public KeyCode Shoot = KeyCode.Space;
    
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce = 20f;
    
    private bool isSlowed = false;
    private float originalForwardSpeed;
    private Coroutine slowDownCoroutine;
    public float _slowFactor = 0.5f;


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

        // movement and jumping
        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            _forwardSpeed += _forwardSpeedIncrease * Time.deltaTime;
            _forwardSpeed = Mathf.Clamp(_forwardSpeed, 10.0f, _maxForwardSpeed);
            transform.position += Vector3.forward * _forwardSpeed * Time.deltaTime;
            
            if (Input.GetKey(RightDirection))
                transform.position += Vector3.right * _forwardSpeed * _sideMultiplier * Time.deltaTime;

            if (Input.GetKey(LeftDirection))
                transform.position += Vector3.left * _forwardSpeed * _sideMultiplier * Time.deltaTime;
            
            if (Input.GetKeyDown(UpDirection) && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); 
                
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                
                isGrounded = false;
            }
            
            if (Input.GetKeyDown(Shoot))
            {
                ShootProjectile();
            }
            
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
        
        if (collision.gameObject.CompareTag("RoadBrick"))
        {
            isGrounded = true; 
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoadBrick"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TriggerGameOver(); 
        }
        
        if (collision.gameObject.CompareTag("Freeze"))
        {
            if (!isSlowed)
            {
                ApplySlow(0.5f, 5f);
            }
            Destroy(collision.gameObject); 
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
        
        Collider projCollider = projectile.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (projCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(projCollider, playerCollider);
        }
    }
    
    public void ApplySlow(float slowFactor, float duration)
    {
        if (isSlowed) return;
        
        if (slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);

        slowDownCoroutine = StartCoroutine(SlowDownCoroutine(slowFactor, duration));
    }

    private IEnumerator SlowDownCoroutine(float slowFactor, float duration)
    {
        isSlowed = true;
        originalForwardSpeed = _forwardSpeed;
        _forwardSpeed *= _slowFactor;

        yield return new WaitForSeconds(duration);

        _forwardSpeed = originalForwardSpeed;
        isSlowed = false;
    }



}
