using UnityEngine;
using System.Collections;

public class PetInteraction : MonoBehaviour
{
    [Header("References")]
    public PetState petState;

    [Header("Animation & AI Links")]
    public Animator petAnimator;
    public UtilityBrain utilitybrain;
    public Act_Hide hidescript;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip catSound;
    public AudioClip purSound;

    [Header("Gesture Settings")]
    [Tooltip("How many pixels the finger have to move to count as swipe")]
    public float minSwipeDistance = 50f;

    [Tooltip("How fast a tap must be (in seconds) to count as hit")]
    public float maxTapTime = 0.3f;

    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isTouchingPet = false;
    private bool hasSwiped = false;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isTouchingPet = true;
                hasSwiped = false;
                touchStartPos = Input.mousePosition;
                touchStartTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0) && isTouchingPet)
        {
            
            float currentDistance = Vector2.Distance(touchStartPos, Input.mousePosition);

            if (currentDistance >= minSwipeDistance)
            {
                hasSwiped = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isTouchingPet)
        {
            float touchDuration = Time.time - touchStartTime;

            if (petState.isSleeping)
            {
                Debug.Log("The pet is sleeping");
                isTouchingPet = false;
                return;
            }

            if (hasSwiped)
            {
                PetTPet();
            }
            else if (touchDuration <= maxTapTime)
            {
                HitPet();
            }
            isTouchingPet = false;
        }
    }

    private void HitPet()
    {
        Debug.Log("Pet was hit, (-trust, +fear)");

        if (audioSource != null)
        {
            if (hitSound != null) audioSource.PlayOneShot(hitSound);
            if (catSound != null) audioSource.PlayOneShot(catSound);
        }

        petState.trust -= 20f;
        petState.fear += 40f;
        petState.happy -= 2f;

        petState.ClampStats();
        petState.SaveState();

        StartCoroutine(HitSequenceRoutine());
    }

    private IEnumerator HitSequenceRoutine()
    {
        petState.isBusy = true;

        if (petAnimator != null) petAnimator.SetTrigger("HBath");

        yield return new WaitForSeconds(2f);

        if (petAnimator != null) petAnimator.Play("idle");

        petState.isBusy = false;
        if (hidescript != null) hidescript.forceHideNextDecision = true;

        if(utilitybrain != null) utilitybrain.ForceDecision();
    }

private void PetTPet()
    {
        Debug.Log("Petted the pet, (+trust, -fear)");
        if (audioSource != null)
        {
            if (purSound != null) audioSource.PlayOneShot(purSound);
        }

        petState.happy += 5f;
        petState.trust += 15f;
        petState.fear -= 5f;

        petState.ClampStats();
        petState.SaveState();

        // Trigger the sequence instead of just playing the animation instantly
        StartCoroutine(PetSequenceRoutine());
    }

private IEnumerator PetSequenceRoutine()
    {
        // Tell the brain we are busy so the Wander script stops trying to move the pet
        petState.isBusy = true; 

        if (petAnimator != null) petAnimator.SetTrigger("SBath");

        // Wait for the petting animation to finish (Adjust this 1.5f if your animation is longer/shorter)
        yield return new WaitForSeconds(3.5f);

        // Tell the brain it is free to start making decisions (like wandering) again
        petState.isBusy = false;
    }
}

    
