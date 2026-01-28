using UnityEngine;
using UnityEditor;
using System.Collections;

public class MusicTransition : MonoBehaviour
{
    public AudioSource aS1, aS2;
    private string trainState;

    float defaultVolume = 1.0f;
    float transitionTime = 1.25f;

    public void ChangeClip()
    {
        AudioSource nowPlaying = aS1;
        AudioSource target = aS2;
        if (trainState == "stop")
        {
            nowPlaying = aS1;
            target = aS2;
        }
        else if (trainState == "ride")
        {
            nowPlaying = aS2;
            target = aS1;
        }
            StartCoroutine(MixSources(nowPlaying, target));
    }

    IEnumerator MixSources(AudioSource nowPlaying, AudioSource target)
    {
        float percentage = 0;
        while (nowPlaying.volume > 0)
        {
            nowPlaying.volume = Mathf.Lerp(defaultVolume, 0, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }

        nowPlaying.Pause();
        if (target.isPlaying == false)
            target.Play();
        target.UnPause();
        percentage = 0;

        while (target.volume < defaultVolume)
        {
            target.volume = Mathf.Lerp(0, defaultVolume, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }
    }
}

