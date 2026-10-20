using UnityEngine;

public class Act_Wander : UtilityAction
{
    public Transform[] Cpoints; 

    private void Awake()
    {
        actionName = "Wander";
        rank = 1;
        cooldownDuration = 0f;
    }

    public override float CalculateWeight(PetState state)
    {
        if (state.fear >= 700f)
        {
            return Random.Range(3f, 10f);
        }
        
        return Random.Range(5f,15f);
    }

    public override void Execute(PetController controller, PetState state)
    {
        base.Execute(controller,state);

        //pick random check point to move
        int randomCpoint = Random.Range(0,Cpoints.Length);
        controller.Move(Cpoints[randomCpoint].position);
    }
}
