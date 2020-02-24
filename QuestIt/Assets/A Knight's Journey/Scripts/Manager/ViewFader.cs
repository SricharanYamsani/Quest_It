using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ViewFader : Singleton<ViewFader>
{
    public Image background;

    public void SetFader(bool fade, Action callback = null)
    {
        if (!fade)
        {
            background.gameObject.SetActive(true);
        }

        background.DOFade(fade ? 0 : 1, 0.4f).OnComplete(() =>
        {
            callback?.Invoke();

            if (fade)
            {
                background.gameObject.SetActive(false);
            }
        });
    }
}
