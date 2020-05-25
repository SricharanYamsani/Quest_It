using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;

public class BgVideoPlayer : MonoBehaviour
{
    [SerializeField] VideoClip video;
    public StreamVideo videoPlayer;
    [SerializeField] RawImage rawImage;

    //---------------------
    private void OnEnable()
    {
        //videoPlayer.videoPlayer.clip = video;
        videoPlayer.SetVideoTexture(rawImage);
        //videoPlayer.gameObject.SetActive(true);
    }

    //----------------------
    private void OnDisable()
    {
        //videoPlayer.gameObject.SetActive(false);
    }
}
