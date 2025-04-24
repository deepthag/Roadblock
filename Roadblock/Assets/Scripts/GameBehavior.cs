using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    private int _score;
    public int Score
    {
        get => _score;
        
        set
        {
            _score = value;
            _scoreUI.text = "Score: " + Score;
        }
    }

    [SerializeField] private TextMeshProUGUI _scoreUI;
    
    public static GameBehavior Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ScorePoint()
    {
        Score++;
    }
    void Update()
    {
        
    }
}
