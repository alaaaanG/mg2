using UnityEngine;
using TMPro;

public class ColorChange : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int health = 4;

    private void Start()
    {
        _healthText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Here, we are going to change the value of 'health',
        // and, as a result, change the color of the prop and the text above the prop.

        // STEP 3 -------------------------------------------------------------
        // Write a line of code to subtract ONE from the value of 'health'.
        // You don't need to declare 'health' again - just change the value.
        health -= 1;
        
        // STEP 3 -------------------------------------------------------------

        // STEP 4 -------------------------------------------------------------
        // DECLARE a new float value named 'r' with a value of 1.
        float r = 1.0f;
        
        // STEP 4 -------------------------------------------------------------

        // STEP 5 -------------------------------------------------------------
        // Add three more else/if statements to this. 
        // IF health is 3, set the value of 'r' to 1.0.
        // IF health is 2, set the value of 'r' to 0.5.
        // IF health is 1, set the value of 'r' to 0.0.
        if (health == 0)
        {
            gameObject.SetActive(false);
        }
        else if (health == 3)
        {
            r = 1.0f;
        }
        else if (health == 2)
        {
            r = 0.5f;
        }
        else if (health == 1)
        {
            r = 0.0f;
        }
        
        // When you're done, uncomment the line below.
        _spriteRenderer.color = new Color(r, 0.2f, 0.2f);
        // STEP 5 -------------------------------------------------------------

        _healthText.gameObject.SetActive(true);

        // STEP 6 -------------------------------------------------------------
        // Add the value of heatlh to this string, so that the heatlh text
        //      displays the prop's current health value.
        _healthText.text = "h = " + health;
        // STEP 6 -------------------------------------------------------------
    }
}
