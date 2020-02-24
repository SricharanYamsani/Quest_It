using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public AudioSource settings;

    public void PlaySound(string sound)
    {
        if (!(soundClips.Count > 0))
        {
            soundClips = ResourceManager.Instance.soundClips;
        }

        if (soundClips != null)
        {
            if (soundClips.ContainsKey(sound))
            {
                GameObject soundObject = new GameObject(sound + " SFX", typeof(AudioSource));

                AudioSource mySource = soundObject.GetComponent<AudioSource>();

                if (settings)
                {
                    mySource.volume = settings.volume;
                }

                mySource.PlayOneShot(soundClips[sound]);

                Destroy(soundObject, soundClips[sound].length);
            }
        }
    }
}