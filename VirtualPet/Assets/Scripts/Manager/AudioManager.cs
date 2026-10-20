using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    public PetState petState;
    public AudioSource mainMusic;
    public AudioSource absenceMusic;

    [Header("Setting")]
    public float fearThreshold = 800f;
    public float fadeDuration = 5f;

    private bool isScaryMode = false;

    void Start()
    {
        mainMusic.volume = 1f;
        mainMusic.Play();

        absenceMusic.volume = 0f;
        absenceMusic.Stop(); //ensuring it is off
    }
    
    void Update()
    {
        if (petState.fear >= fearThreshold && !isScaryMode)
        {
            isScaryMode = true;
            StopAllCoroutines();
            StartCoroutine(Crossfade(mainMusic,absenceMusic));
            Debug.Log("Fear threshold reached. Absence of music");
        }
        else if (petState.fear < fearThreshold && isScaryMode)
        {
            isScaryMode = false;
            StopAllCoroutines();
            StartCoroutine(Crossfade(absenceMusic, mainMusic));
            Debug.Log("Fear subsided. Main song");
        }
    }

    private IEnumerator Crossfade(AudioSource fadingOut, AudioSource fadingIn)
    {
        if (!fadingIn.isPlaying)
        {
            fadingIn.Play();
        }

        float currentTime = 0f;
        float startFadeOut = fadingOut.volume;
        float startFadeIn = fadingIn.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;

            fadingOut.volume = Mathf.Lerp(startFadeOut, 0f, currentTime);
            fadingIn.volume = Mathf.Lerp(startFadeIn, 1f, currentTime);

            yield return null;
        }

        fadingOut.volume = 0f;
        fadingIn.volume = 1f;

        //silent track paused, save resources
        fadingOut.Pause();
    }
}
