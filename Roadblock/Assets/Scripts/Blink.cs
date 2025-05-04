using System.Collections;
using UnityEngine;
using TMPro;

public class Blink : MonoBehaviour
{
    private TextMeshProUGUI _message;
    private bool _inCoroutine = false; 
    
    public float _blinkRate = 0.5f;
    
    private Coroutine _coroutine;
    
    void Start()
    {
        _message = GetComponent<TextMeshProUGUI>(); 
    }
    
    void Update()
    {
        if (!_inCoroutine)
        {
            _coroutine = StartCoroutine(Flicker());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StopCoroutine(_coroutine);
            _inCoroutine = false;
        }
    }

    IEnumerator Flicker()
    {
        _inCoroutine = true; 
        _message.enabled = !_message.enabled;

        // interrupt execution for one frame only
        // yield return null;
        // Debug.Log("I skipped a frame!");
        
        yield return new WaitForSeconds(_blinkRate);
        Debug.Log("I skipped one second!");
        
        _inCoroutine = false;
    }
}