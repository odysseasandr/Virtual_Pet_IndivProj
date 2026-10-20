using UnityEngine;

public class ScrollLoop : MonoBehaviour
{
    public float speed = 5f;
    
    // We manually tell it the exact width so Unity doesn't mess up the math!
    public float width = 60f; 

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= -width)
        {
            transform.position = new Vector2(transform.position.x + (width * 2f), transform.position.y);
        }
    }
}