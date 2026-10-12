using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float _life = 1.0f;
    [SerializeField] private Collider2D _collider;

    private float _timeLeft;

    private void OnEnable()
    {
        _timeLeft = _life;
        _collider.enabled = true;
    }

    private void Update()
    {
        // STEP 1 -------------------------------------------------------------
        // Write a line of code to SUBTRACT the value of 'time' from '_timeLeft'.
        float time = Time.deltaTime;
        _timeLeft -= time;
        
        // STEP 1 -------------------------------------------------------------

        // STEP 2 -------------------------------------------------------------
        // Uncomment and fix the if statement.
        if (_timeLeft <= 0.0f)
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
        // STEP 2 -------------------------------------------------------------
    }
}
