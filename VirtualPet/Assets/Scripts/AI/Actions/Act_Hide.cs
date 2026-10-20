using UnityEngine;

public class Act_Hide : UtilityAction
{
    [Header("Hiding Setup")]
    public Transform[] hidingSpot;
    private Transform chosenSpot;
    private SpriteRenderer petSprite;

    private bool isHiding = false;
    private float hideTime = 0f;
    private float maxHideTime = 9f;
    
    // NEW: A failsafe timer just in case it gets stuck on a wall
    private float stuckFailsafeTimer = 0f; 

    [HideInInspector] public bool forceHideNextDecision = false;
    
    private void Start()
    {
        petSprite = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        actionName = "Hide";
        rank = 10;
        cooldownDuration = 60f;
    }

    public override float CalculateWeight(PetState state)
    {
        if (isHiding) return 1000;
        if (forceHideNextDecision) return 2000f;

        if (state.fear >= 700f)
        {
            float hideChance = Mathf.InverseLerp(500f, 0f, state.sanity);
            if (Random.value < hideChance) return 500f;
        }
        return 0f;
    }

    public override void Execute(PetController controller, PetState state)
    {
        base.Execute(controller, state);
        forceHideNextDecision = false;

        if (!isHiding)
        {
            isHiding = true;
            state.isBusy = true;
            hideTime = 0f;
            stuckFailsafeTimer = 0f; // Reset failsafe

            chosenSpot = hidingSpot[Random.Range(0, hidingSpot.Length)];
            Debug.Log("Pet is terrified and went to hide");
        }
    }
    
    void Update()
    {
        if (isHiding && chosenSpot != null)
        {
            PetController controller = GetComponent<PetController>();
            PetState state = GetComponent<PetState>();

            controller.Move(chosenSpot.position);
            petSprite.sortingOrder = 2;

            // Failsafe: If it takes longer than 5 seconds to reach the spot, force it to stop
            stuckFailsafeTimer += Time.deltaTime;
            if (stuckFailsafeTimer > 5f && Vector2.Distance(transform.position, chosenSpot.position) > 0.5f)
            {
                Debug.LogWarning("Pet got stuck while trying to hide! Forcing brain unlock.");
                StopHiding(state);
                return;
            }

            if (Vector2.Distance(transform.position, chosenSpot.position) < 0.5f)
            {
                hideTime += Time.deltaTime;
                if (hideTime >= maxHideTime)
                {
                    StopHiding(state);
                }
            }
        }
    }

    private void StopHiding(PetState state)
    {
        isHiding = false;
        state.isBusy = false;
        petSprite.sortingOrder = 10;
        Debug.Log("Pet stopped hiding");
    }
}