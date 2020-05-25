using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ViewFader : MonoBehaviour
{
    public FaderCanvas m_Canvas = null;

    private Sequence m_Sequence;

    public void SetFader(float fade, float time = 1f, Action callback = null)
    {
        if (m_Sequence != null)
        {
            m_Sequence.Kill((true));
        }

        m_Sequence = DOTween.Sequence();

        m_Sequence.Append(m_Canvas.background.DOFade(fade, time));

        m_Sequence.OnComplete(() => { callback?.Invoke(); });
    }
}
