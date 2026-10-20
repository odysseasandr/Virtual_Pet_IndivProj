using UnityEngine;

public abstract class UtilityAction : MonoBehaviour
{
    [Header("Action Identity")]
    public string actionName;

    [Header("Dual Utility Metrics")]
    [Tooltip("Higher rank overrides lower rank")]
    public int rank;

    [Header("Pacing Guardrails")]
    [Tooltip("Seconds to wait before this action can happen again.")]
    public float cooldownDuration = 0f;
    protected float lastExecutionTime = -1000f;

    [Tooltip("If true, this action will only ever happen one time.")]
    public bool isDoOnce = false;
    protected bool hasBeenExecuted = false;

    public virtual bool IsAvailable()
    {
        //enforce the "Do Once" pattern for major jump scares or narrative reveals
        if (isDoOnce && hasBeenExecuted)
        {
            return false;
        }

        //enforce the "Cooldown" pattern to prevent it being repetitive
        if (Time.time < (lastExecutionTime + cooldownDuration))
        {
            return false;
        }

        //if it passes all checks, it is available to be chosen
        return true;
    }

    /// <summary>
    /// Calculates the Relative Utility (Weight) based on the pet's current state.
    /// </summary>
    public abstract float CalculateWeight(PetState state);

    /// <summary>
    /// Executes the action and registers the safety constraints so it cannot fire again too soon.
    /// </summary>
    public virtual void Execute(PetController controller, PetState state)
    {
        //log the time and execution status the moment the action fires
        lastExecutionTime = Time.time;
        hasBeenExecuted = true;
        
        Debug.Log($"AI Decision Engine Executed: {actionName}");
    }
}