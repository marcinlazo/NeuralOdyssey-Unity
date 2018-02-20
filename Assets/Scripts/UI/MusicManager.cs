using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;

    AudioSource source;
    public AudioClip music_start;
    public AudioClip music_room;
    public AudioClip music_openDoor;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void Music_Start(float targetVolume = 1)
    {
        StartCoroutine(FadeAndPlay(targetVolume, music_start));
    }
    public void Music_Room(float targetVolume = 1)
    {
        StartCoroutine(FadeAndPlay(targetVolume, music_room));
    }
    public void Music_OpenDoor(float targetVolume = 1)
    {
        StartCoroutine(FadeAndPlay(targetVolume, music_openDoor));
    }

    IEnumerator FadeAndPlay(float targetVolume, AudioClip newClip)
    {
        if (source.isPlaying)
        {
            float timer = 0, timeMax = 5;
            float startVol = source.volume, endVol = 0;
            while(timeMax > timer)
            {
                source.volume = Mathf.Lerp(startVol, endVol, timer / timeMax);
                timer += Time.deltaTime;

                yield return null;
            }
            source.Stop();
            source.volume = startVol;
        }
        source.volume = targetVolume;
        source.clip = newClip;
        source.Play();
    }
}
