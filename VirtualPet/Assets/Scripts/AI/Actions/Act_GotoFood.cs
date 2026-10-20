using UnityEngine;
using System.Collections; 

public class Act_GotoFood : UtilityAction
{
    private GameObject targetFood;
    private bool ischasingFood = false;

    [Header("Audio")]
    public AudioSource pAudio;
    public AudioClip eatSound;

    private void Awake()
    {
        actionName = "Go To Food";
        rank = 7;
    }

    public override float CalculateWeight(PetState state)
    {
        targetFood = GameObject.FindGameObjectWithTag("Food");

        if (targetFood != null)
        {
            //if terrified,it will not eat
            if (state.fear >= 70f && state.trust <= 30f)
            {
                Debug.Log("The pet is too scared of you to eat");
                return 0f;
            }
            return 100f;
        }

        ischasingFood = false;
        return 0f; 
    }

    public override void Execute(PetController controller, PetState state)
    {
        base.Execute(controller, state);

        if (targetFood != null)
        {
            //walk  to food
            controller.Move(targetFood.transform.position);
            ischasingFood = true;
        }
    }

    private void Update()
    {
        //check the distance between pet and food
        if (ischasingFood && targetFood !=null)
        {
            float distance = Vector2.Distance(transform.position, targetFood.transform.position);

            //if close it is eaten
            if (distance < 0.5f)
            {
                ischasingFood = false;
                StartCoroutine(ConsumeFoodRoutine());
            }
        }
    }

    private IEnumerator ConsumeFoodRoutine()
    {
        PetState state = GetComponent<PetState>();
        PetController controller = GetComponent<PetController>();
        FoodItem foodData = targetFood.GetComponent<FoodItem>();

        state.isBusy = true;

        controller.Eat();
        
        if(pAudio != null && eatSound != null) pAudio.PlayOneShot(eatSound);

        yield return new WaitForSeconds(1.5f);

        state.hunger = Mathf.Clamp(state.hunger - foodData.nutriAmount, 0f, 100f);

        if (foodData.isfoodGood)
        {
            state.trust = Mathf.Clamp(state.trust +50f, 0f, 1000f);
            state.dependence = Mathf.Clamp(state.dependence +20f, 0f, 1000f);
            state.fear = Mathf.Clamp(state.fear -5f, 0f, 1000f);
            state.sanity = Mathf.Clamp(state.sanity +50f, 0f, 1000f);
        }
        else
        {
            state.trust = Mathf.Clamp(state.trust -30f, 0f, 1000f);
            state.dependence = Mathf.Clamp(state.dependence -30f, 0f, 1000f);
            state.fear = Mathf.Clamp(state.fear +70f, 0f, 1000f);
            state.sanity = Mathf.Clamp(state.sanity -30f, 0f, 1000f);
        }

        state.SaveState();
        
        Animator petAnimator = GetComponent<Animator>();
        if (petAnimator != null) petAnimator.Play("Idle");

        //delete food item from the scene
        Destroy(targetFood);
        state.isBusy = false;
    }

}
