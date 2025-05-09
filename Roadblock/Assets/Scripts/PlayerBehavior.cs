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

    public TMPro.TextMeshProUGUI gameOverText;
    public TMPro.TextMeshProUGUI playAgainText;

    private bool isGameOver = false;

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

        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            _forwardSpeed += _forwardSpeedIncrease * Time.deltaTime;
            _forwardSpeed = Mathf.Clamp(_forwardSpeed, 10.0f, _maxForwardSpeed);

            transform.position += Vector3.forward * _forwardSpeed * Time.deltaTime;

            if (Input.GetKey(RightDirection))
                transform.position += Vector3.right * _forwardSpeed * _sideMultiplier * Time.deltaTime;

            if (Input.GetKey(LeftDirection))
                transform.position += Vector3.left * _forwardSpeed * _sideMultiplier * Time.deltaTime;

            if (transform.position.y < -10)
                TriggerGameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
            TriggerGameOver();
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RoadBrick")
            TriggerGameOver();
    }

    void TriggerGameOver()
    {
        GameBehavior.Instance.State = Utilities.GameplayState.GameOver;
        alive = false;
        isGameOver = true;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }
        
        if (playAgainText != null)
        {
            playAgainText.gameObject.SetActive(true);
        }
    }
}
