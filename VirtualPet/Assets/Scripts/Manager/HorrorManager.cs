using UnityEngine;
using System.Collections;

public class HorrorManager : MonoBehaviour
{
    [Header("References")]
    public PetState petState;
    
    public PetController petController; 
    public Animator petAnimator;
    public DepthScaler depthScaler;

    public SpriteRenderer backgroundSprite;
    public SpriteRenderer pillarSprite;
    public SpriteRenderer petSprite; 
    public AudioSource audioSource;

    [Header("Thresholds")]
    public float fearThreshold = 600f;

    [Header("Lighting Settings")]
    public Color normalColor = Color.white;
    public Color creepyColor = new Color(0.6f, 0.6f, 0.6f); 
    public Color flickerColor = new Color(0.1f, 0.1f, 0.1f); 
    public AudioClip flickerSound;

    [Header("Shadow Settings")]
    public GameObject[] shadowObjects; 
    public AudioClip shadowSound; 

    [Header("Obsession Event Settings")]
    public float obsessionThreshold = 700f; 
    public AudioClip obsessionSound;
    public Vector3 hugeScale = new Vector3(0.7f, 0.7f, 0.7f); 
    
    public float obsessionCooldownMin = 120f;
    public float obsessionCooldownMax = 180f;
    private float nextAvailableObsessionTime = 0f;

    private bool isCreepyMode = false;
    private bool isObsessionEventPlaying = false;
    private Vector3 originalPetScale;

    private void Start()
    {
        HideAllShadows(); 
        
        if (petState != null)
        {
            originalPetScale = petState.transform.localScale;
        }
    }

    private void Update()
    {
        if (petState.fear >= fearThreshold)
        {
            if (!isCreepyMode)
            {
                isCreepyMode = true;
                pillarSprite.color = creepyColor;
                backgroundSprite.color = creepyColor;
                petSprite.color = creepyColor;
                
                StartCoroutine("FlickerRoutine");
                StartCoroutine("ShadowRoutine");
            }
        }
        else
        {
            if (isCreepyMode)
            {
                isCreepyMode = false;
                
                StopCoroutine("FlickerRoutine");
                StopCoroutine("ShadowRoutine");
                
                petSprite.color = normalColor;
                pillarSprite.color = normalColor;
                backgroundSprite.color = normalColor;
                HideAllShadows();
            }
        }

        if ((petState.trust >= obsessionThreshold || petState.dependence >= obsessionThreshold))
        {
            if (!isObsessionEventPlaying && !petState.isSleeping && Time.time >= nextAvailableObsessionTime)
            {
                StartCoroutine(ObsessionRoutine());
            }
        }
    }

    private IEnumerator ObsessionRoutine()
    {
        isObsessionEventPlaying = true;
        
        nextAvailableObsessionTime = Time.time + Random.Range(obsessionCooldownMin, obsessionCooldownMax);

        yield return new WaitForSeconds(Random.Range(2f, 5f));

        //lock the AI brain
        petState.isBusy = true;

        if (petController != null) petController.Move(petState.transform.position); 
        if (petAnimator != null) petAnimator.Play("Idle");

        if (depthScaler != null) depthScaler.enabled = false;

        petSprite.sortingOrder = 15; 
        petState.transform.localScale = hugeScale;

        if (audioSource != null && obsessionSound != null)
        {
            audioSource.PlayOneShot(obsessionSound);
        }

        yield return new WaitForSeconds(Random.Range(4f, 7f));


        petState.transform.localScale = originalPetScale;
        petSprite.sortingOrder = 10; 
        petState.isBusy = false; 

        isObsessionEventPlaying = false;
    }

    private IEnumerator FlickerRoutine()
    {
        while (petState.fear >= fearThreshold)
        {
            yield return new WaitForSeconds(Random.Range(15f, 45f));

            if (audioSource != null && flickerSound != null)
            {
                audioSource.PlayOneShot(flickerSound);
            }

            int flickers = Random.Range(3, 8);
            for (int i = 0; i < flickers; i++)
            {
                petSprite.color = flickerColor;
                pillarSprite.color = flickerColor; 
                backgroundSprite.color = flickerColor;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f)); 
                
                petSprite.color = creepyColor;
                backgroundSprite.color = creepyColor;
                pillarSprite.color = creepyColor; 
                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));  
            }

            pillarSprite.color = flickerColor;
            petSprite.color = flickerColor;
            backgroundSprite.color = flickerColor;
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            
            backgroundSprite.color = creepyColor; 
            pillarSprite.color = creepyColor;
            petSprite.color = creepyColor;
        }
    }

    private IEnumerator ShadowRoutine()
    {
        yield return new WaitForSeconds(Random.Range(45f, 90f));

        while (petState.fear >= fearThreshold)
        {
            if (shadowObjects != null && shadowObjects.Length > 0)
            {
                int index = Random.Range(0, shadowObjects.Length);
                
                if (shadowSound != null)
                {
                    audioSource.PlayOneShot(shadowSound);
                }

                if (shadowObjects[index] != null)
                {
                    shadowObjects[index].SetActive(true);
                    yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
                    shadowObjects[index].SetActive(false);
                }
            }
            yield return new WaitForSeconds(Random.Range(40f, 100f));
        }
    }

    private void HideAllShadows()
    {
        if (shadowObjects == null) return;
        foreach (GameObject shadow in shadowObjects)
        {
            if (shadow != null) shadow.SetActive(false);
        }
    }
}