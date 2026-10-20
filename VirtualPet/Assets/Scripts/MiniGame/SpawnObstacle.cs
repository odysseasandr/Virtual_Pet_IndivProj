using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public GameObject obstaclePref;
    public float minTime = 1.2f;
    public float maxTime = 2.5f;
    private float timer = 0f;
    private float currentTime;

    [Header("Spawn Heights")]
    public float groundYPosition = -2.0f; //set this in the Inspector!
    public float highYPosition = 1.0f;    //set this in the Inspector!

    void Start()
    {
        RandomSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentTime)
        {
            //call randomizer method
            SpawnRandomObstacle();
            
            timer = 0f;
            RandomSpawnTime(); // pick a new random time 
        }
    }

    private void SpawnRandomObstacle()
    {
        //pick random number ground or high
        int randomChoice = Random.Range(0, 2); 
        
        //decide the Y height on random
        float chosenY = (randomChoice == 0) ? groundYPosition : highYPosition;
        
        //create the spawn point for the obstacle
        Vector2 spawnPosition = new Vector2(transform.position.x, chosenY);

        //spawn obstacle at chosen position
        Instantiate(obstaclePref, spawnPosition, Quaternion.identity);
    }

    private void RandomSpawnTime()
    {
        currentTime = Random.Range(minTime, maxTime);
    }
}