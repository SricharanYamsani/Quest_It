using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    private BackgroundSoundManager bsm;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayBGMusic(string musicName)
    {
        if (bsm == null)
        {
            bsm = Instantiate(Resources.Load<BackgroundSoundManager>("Prefabs/BSM"));
        }

        bsm.PlayBackgroundAudio(musicName);
    }

    public void StopBGMusic()
    {
        if (bsm == null)
        {
            bsm = Instantiate(Resources.Load<BackgroundSoundManager>("Prefabs/BSM"));
        }

        bsm.StopBackgroundAudio();
    }

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

                mySource.PlayOneShot(soundClips[sound]);

                Destroy(soundObject, soundClips[sound].length);
            }
        }
    }
}