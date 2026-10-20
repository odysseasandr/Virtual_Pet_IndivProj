using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 8f;
    private float gOver = -15; //position of x where obstacle is destroyed

    void Update()
    {
        //move to the left
        transform.Translate(Vector2.left*speed*Time.deltaTime);

        //when it leaves the screen, destroy
        if (transform.position.x < gOver)
        {
            Destroy(gameObject);
        }
        
    }
}
