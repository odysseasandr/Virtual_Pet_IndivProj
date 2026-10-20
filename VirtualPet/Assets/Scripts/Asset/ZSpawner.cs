using UnityEngine;

public class ZSpawner : MonoBehaviour
{
    public GameObject zPrefab;
    public float spawnRate = 1.2f;

    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= spawnRate)
        {
            Instantiate(zPrefab, transform.position, Quaternion.identity);
            timer = 0f;
        }
    }
}
