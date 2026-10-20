using UnityEngine;

public class Act_Sleep : UtilityAction
{
    public GameObject bedObject;
    public Transform bedLocation;
    private bool isWalkingtoBed = false;

    [Header("Settings")]
    private float sleepTimer = 0f;
    private float currentSDuration = 0f;
    private bool isAsleep = false;

    void Awake()
    {
        actionName = "Sleep";
        rank = 6;

        cooldownDuration = 30f;
    }

    public override float CalculateWeight(PetState state)
    {
        if (state.isSleeping || isWalkingtoBed) return 100f;

        if (bedObject == null || !bedObject.activeInHierarchy)
        {
            if (isWalkingtoBed)
            {
                isWalkingtoBed = false;
            }

            return 0f;
        }

        if (isWalkingtoBed) return 100f;
        float weight = 0f;

        if (state.energy < 30f)
        {
            weight += (100f - state.energy);
        }

        if (state.energy >= 30f && state.happy < 40f)
        {
            weight += 20f;
        }
        return weight;
    }

    public override void Execute(PetController controller, PetState state)
    {
        base.Execute(controller, state);

        if (!state.isSleeping)
        {
            if (bedObject != null && bedObject.activeInHierarchy && bedLocation != null)
            {
                isWalkingtoBed = true;
            }
        }
        
    }

    void Update()
    {
        PetState state = GetComponent<PetState>();

        if (isWalkingtoBed && bedLocation != null)
        {
            PetController controller = GetComponent<PetController>();

            if (controller != null)
            {
                controller.Move(bedLocation.position);
            }

            float distance = Vector2.Distance(transform.position, bedLocation.position);
            if (distance < 0.5f)
            {
                GetInBed(state);
            }
        }
        else if (isWalkingtoBed && (bedLocation == null || !bedObject.activeInHierarchy))
        {
            //bed toggled off while pet was walking to it
            isWalkingtoBed = false;
        }
        
        if (state.isSleeping)
        {
            if (state.energy >= 100)
            {
                if (!isAsleep)
                {
                    currentSDuration = Random.Range(20f, 45f);
                    sleepTimer = 0f;
                    isAsleep = true;
                    Debug.Log($"Pet is fully rested. Sleeping for {currentSDuration} seconds");
                }

                sleepTimer += Time.deltaTime;
                if (sleepTimer >= currentSDuration) WakeUp(state);
            }
            else { isAsleep = false; }
        }
    }

    private void GetInBed(PetState state)
    {
        isWalkingtoBed = false;
        state.manualSleep = false;
        state.isSleeping = true;
        isAsleep = false;
        state.SaveState();
    }

    private void WakeUp(PetState state)
    {
        state.isSleeping = false;
        isAsleep = false;


        state.SaveState();
        Debug.Log("Pet woke itself up rested");
    }

}
