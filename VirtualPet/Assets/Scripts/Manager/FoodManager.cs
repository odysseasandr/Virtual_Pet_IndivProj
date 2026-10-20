using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [Header("References")]
    public CoinManager coinManager;

    [Header("UI Menu")]
    public GameObject foodmenuPanel;

    
    [Header("Food Prefabs")]
    //slots for 4 food items
    public GameObject gSteakPrefab;
    public GameObject gFishPrefab;
    public GameObject bApplePrefab;
    public GameObject bBreadPrefab;

    
    [Header("Spawn Settings")]
    
    //empty GameObject in scene where food falls
    public Transform[] dropPoints; 

    //variable for current_food
    private GameObject currentFood;


    //main button
    public void ToggleFoodMenu()
    {
        if (foodmenuPanel != null) foodmenuPanel.SetActive(!foodmenuPanel.activeSelf);   
    }

    //sub-menu buttons, int = cost
    public void DropGSteak() { SpawnFood(gSteakPrefab, 5); }
    public void DropGFish() { SpawnFood(gFishPrefab, 4); }
    public void DropBApple() { SpawnFood(bApplePrefab, 0); }
    public void DropBBread() { SpawnFood(bBreadPrefab, 1); }

    private void SpawnFood(GameObject foodPrefab, int cost)
    {
        //check if food has already spawned
        if (currentFood != null)
        {
            Debug.Log("There is still uneaten food");

            if (foodmenuPanel != null) foodmenuPanel.SetActive(false);
            return;
        }

        if (coinManager != null & coinManager.CanSpendCoins(cost))
        {
            Debug.Log("Bought " + foodPrefab.name);
            if (foodmenuPanel != null) foodmenuPanel.SetActive(false);

            int randomIndex = Random.Range(0, dropPoints.Length);
            Transform rdropPoint = dropPoints[randomIndex];
            currentFood = Instantiate(foodPrefab, rdropPoint.position, Quaternion.identity);     
        }
        else if (coinManager == null) Debug.LogError("FoodManager is missing CoinManager");
    }
}
