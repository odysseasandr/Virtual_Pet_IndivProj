using UnityEngine;

public class UtilityBrain : MonoBehaviour
{
    [Header("Core")]
    public PetController controller;
    public PetState state;

    [Header("Settings")]
    [Tooltip("How often choices are evaluated in (seconds)")]
    public float decisionInt = 1f;
    private float timer;

    //array holding all action scripts attached to pet
    private UtilityAction[] availableActions;

    private void Start()
    {
        controller = GetComponent<PetController>();
        state = GetComponent<PetState>();
        
        availableActions = GetComponents<UtilityAction>();
        timer = decisionInt;
    }

    private void Update()
    {
        //simple efficiency timer
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            MakeDecision();
            timer = decisionInt;
        }
    }

    private void MakeDecision()
    {
        if (state.isSleeping || state.isBusy) return;
        
        UtilityAction bestAction = null;
        int highestRank = -1;
        float highestWeight = -1f;

        //loop through every action script attached to pet
        foreach (UtilityAction action in availableActions)
        {
            if (!action.IsAvailable()) continue;

            //calculate the desirability of an action
            float currentWeight = action.CalculateWeight(state);

            if (currentWeight <= 0) continue;

            //is there a higher rank than the current?
            if (action.rank > highestRank)
            {
                highestRank = action.rank;
                highestWeight = currentWeight;
                bestAction = action;        
            }
            //if ranks are equal, check weight
            else if (action.rank == highestRank)
            {
                //removed the redundant double if-statement here
                if (currentWeight > highestWeight)
                {
                    highestWeight = currentWeight;
                    bestAction = action;
                }
            }
        }
        // execute the single best fitting action after evaluating all options
        if (bestAction != null)
        {
            bestAction.Execute(controller, state);
        }
    }
    public void ForceDecision()
    {
        //forces countdown timer to zero and acts immediately
        timer = 0f; 
    }
}