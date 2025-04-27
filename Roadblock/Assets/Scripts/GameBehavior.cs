using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    public Utilities.GameplayState State;
    
    private float _score;
    public float _gemBonus;
    public int DisplayedScore
    {
        get => Mathf.FloorToInt(_score);
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

    private void Start()
    {
        State = Utilities.GameplayState.Play;
    }

    public void CollectGem ()
    {
        if (State == Utilities.GameplayState.Play) 
        {
            _score += _gemBonus; 
            UpdateScoreUI(); 
        }
    }

    void Update()
    {
        if (State == Utilities.GameplayState.Play) 
        {
            _score += Time.deltaTime * 10;
            UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        if (State == Utilities.GameplayState.Play)
        { 
            _scoreUI.text = "Score: " + DisplayedScore;
        }
    }
    
}