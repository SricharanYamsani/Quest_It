using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour
{
    public AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();

        SetBackgroundVolume(20);
    }

    public void PlayBackgroundAudio(string audio)
    {
        m_AudioSource.clip = ResourceManager.Instance.bgClips[audio];

        m_AudioSource.Play();
    }

    public void StopBackgroundAudio()
    {
        m_AudioSource.Stop();
    }

    public void SetBackgroundVolume(int volume)
    {
        m_AudioSource.volume = volume / (float)1000;
    }
}
