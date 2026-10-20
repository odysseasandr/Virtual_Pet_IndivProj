using UnityEngine;

public class Act_Idle : UtilityAction
{
    private void Awake()
    {
        actionName = "Idle";
        rank = 1;
        cooldownDuration = 0f;
    }

    public override float CalculateWeight(PetState state)
    {
        //return a low constant weight to be out-weigh from more impoerant actions
        return Random.Range(5f,15f);
    }

    public override void Execute(PetController controller, PetState state)
    {
        //alwaus calll base execute first to record execusion time 
        base.Execute(controller, state);

        //play idle animation from animator
        controller.Move(controller.transform.position);
        controller.Idle();
    }
}
