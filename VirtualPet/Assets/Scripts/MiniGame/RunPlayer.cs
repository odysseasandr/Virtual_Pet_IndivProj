using UnityEngine;

public class RunPlayer : MonoBehaviour
{
    public float jumpF = 12f;

    public GameObject confirmPopup;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpF);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //when grounded it can jump again
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle. Game Over");
            //freeze game
            Time.timeScale = 0f;

            confirmPopup.SetActive(true);

            //later, save coins
        }
        else if(collision.gameObject.CompareTag("Coin"))
        {
            //add coin

            FindFirstObjectByType<InGameManager>().AddCoins();
            //destroy coin
            Destroy(collision.gameObject);
        }
    }
}
