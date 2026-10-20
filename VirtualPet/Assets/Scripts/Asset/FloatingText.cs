using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float lifetime = 2f;
    
    // THE FIX: Changed to TMP_Text so it works with ANY kind of TextMeshPro!
    private TMP_Text textMesh; 
    private Color originalColor;
    private float timer = 0f;
    private float randomDrift;

    void Start()
    {
        // Find the component using the universal tag
        textMesh = GetComponent<TMP_Text>();
        
        // THE SAFETY NET: If it still can't find it, stop the script safely!
        if (textMesh == null)
        {
            Debug.LogError("No TextMeshPro component found on " + gameObject.name + "! Turning off script to prevent crash.");
            this.enabled = false; // Turns off the Update loop
            return;
        }

        originalColor = textMesh.color;
        randomDrift = Random.Range(-0.5f, 0.5f);
        
        float randomScale = Random.Range(0.8f, 1.5f);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 1. Move Up and Sideways
        transform.Translate(new Vector3(randomDrift, floatSpeed, 0) * Time.deltaTime);

        // 2. Fade Out slowly (This was Line 36!)
        float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        // 3. Destroy when time is up
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
