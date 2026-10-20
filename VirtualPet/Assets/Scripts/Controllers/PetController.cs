using UnityEngine;

public class PetController : MonoBehaviour
{
    public Animator petAnimator;
    private Vector3 destination;
    public float speed;

    private void Awake()
    {
        
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position,destination) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
        }
    }

    public void Move(Vector3 destination)
    {
        this.destination = destination;
    }

    public void Idle()
    {
        petAnimator.SetTrigger("Idle");
    }
    public void Happy()
    {
        petAnimator.SetTrigger("Happy");
    }
    public void Hungry()
    {
        petAnimator.SetTrigger("Hungry");
    }
    public void Tired()
    {
        petAnimator.SetTrigger("Tired");
    }
    public void Eat()
    {
        petAnimator.SetTrigger("Eat");
    }
    public void HBath()
    {
        petAnimator.SetTrigger("HBath");
    }
    public void SBath()
    {
        petAnimator.SetTrigger("SBath");
    }

}
