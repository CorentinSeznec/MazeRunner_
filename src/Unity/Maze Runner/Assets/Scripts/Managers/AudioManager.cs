using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip defaultAmbience;
    [SerializeField] float timeToFade = 1.25f; 

    private AudioSource track01, track02;
    private bool isPlayingTrack01;

    public static AudioManager instance;

    private void Awake()
    {   
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        track01 = gameObject.AddComponent<AudioSource>();
        track02 = gameObject.AddComponent<AudioSource>();
        track01.loop = true;
        track02.loop = true;
        isPlayingTrack01 = true;
    }   

    public bool SwapTrack(AudioClip new_clip)
    {
        if((isPlayingTrack01 && track01.clip != new_clip) || (!isPlayingTrack01 && track02.clip != new_clip))
        {
            StopAllCoroutines();

            StartCoroutine(FadeTrack(new_clip));

            isPlayingTrack01 = !isPlayingTrack01;
            return true;
        }
        return false;
    }

    public bool ReturnToDefault()
    {
        return SwapTrack(defaultAmbience);
    }

    private IEnumerator FadeTrack(AudioClip new_clip)
    {
        float timeElapsed = 0;
        if(isPlayingTrack01)
        {
            track02.clip = new_clip;
            track02.Play();

            while(timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track01.Stop();
        }
        else
        {
            track01.clip = new_clip;
            track01.Play();

            while(timeElapsed < timeToFade)
            {
                track01.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track02.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track02.Stop();
        }
    }
}
