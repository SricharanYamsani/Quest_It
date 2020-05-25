using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    Coroutine videoCoroutine;       

    // Use this for initialization
    //-------------
    void Start()
    {
        videoCoroutine = StartCoroutine(PlayVideo());
    }

    //---------------------
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        //rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();        
    }

    //--------------------------------------------
    public void SetVideoTexture(RawImage rawImage)
    {
        rawImage.texture = videoPlayer.texture;
    }

    //----------------------
    private void OnDisable()
    {
        StopCoroutine(videoCoroutine);
    }
}