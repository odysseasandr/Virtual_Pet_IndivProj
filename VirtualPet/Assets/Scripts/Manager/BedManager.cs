using UnityEngine;

public class BedManager : MonoBehaviour
{
    public PetState petState;

    [Header("Visuals")]
    public GameObject bedObject;
    public GameObject sleepEffect;

    private SpriteRenderer petSprite;
    private Animator petAnimator;
    private Vector3 originalScale; // We will use this to remember how big your pet is!

    void Start()
    {
        if (petState != null)
        {
            petSprite = petState.GetComponent<SpriteRenderer>();
            petAnimator = petState.GetComponent<Animator>();
            
            //the pet's original size so it can be restored later
            if (petState.transform.localScale != Vector3.zero)
            {
                originalScale = petState.transform.localScale;
            }
            else
            {
                originalScale = new Vector3(0.32f, 0.32f, 0.32f); // Safe fallback
            }
            //add. if the pet sleep and b/O show bed
            if (petState.isSleeping && bedObject != null) bedObject.SetActive(true);
        }

        if (sleepEffect != null)
        {
            ParticleSystemRenderer psr = sleepEffect.GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                psr.sortingOrder = 10;
            }
        }
    }

    public void ToggleSleep() 
    {
        if (bedObject == null) return;

        bool isBedNowActive = !bedObject.activeSelf;
        bedObject.SetActive(isBedNowActive);

        // Wake the pet up if you pull the bed away while it's sleeping
        if (!isBedNowActive && petState.isSleeping) 
        { 
            petState.isSleeping = false;
            petState.SaveState();
        }
    }

    void LateUpdate()
    {
        if (petState == null) return;

        if (petState.isSleeping)
        {
            //freeze animator
            if (petAnimator != null) petAnimator.enabled = false;
            
            //remove pixels
            if (petSprite != null) 
            {
                petSprite.enabled = false;
                petSprite.color = new Color(1f, 1f, 1f, 0f); // 100% Transparent
            }
            
            //shrink to zero
            petState.transform.localScale = Vector3.zero;
            
            if (sleepEffect != null) sleepEffect.SetActive(true);
        }
        else
        {
            //unfreeze animator
            if (petAnimator != null) petAnimator.enabled = true;
            
            //restore pixels
            if (petSprite != null) 
            {
                petSprite.enabled = true;
                petSprite.color = new Color(1f, 1f, 1f, 1f); // 100% Solid
            }
            
            //restore size
            petState.transform.localScale = originalScale;
            
            if (sleepEffect != null) sleepEffect.SetActive(false);
        }
    }
}