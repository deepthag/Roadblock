using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopAllCoroutines();
            SceneManager.LoadScene("Roadblock");
        }
    }
}
