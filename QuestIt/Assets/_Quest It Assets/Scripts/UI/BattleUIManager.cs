using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUIManager : MonoBehaviour
{
    public RectTransform slider = null;

    public RectTransform sliderPanel = null;

    bool isSliderOn = false;

    private void Awake ( )
    {
        if ( !isSliderOn )
        {
            slider.sizeDelta = new Vector2 ( 0 , 250 );

            sliderPanel.gameObject.SetActive ( false );

            slider.gameObject.SetActive ( false );
        }
    }

    public void ToggleSlider()
    {
        if(!isSliderOn)
        {
            slider.gameObject.SetActive ( true );

            slider.DOSizeDelta ( new Vector2 ( 1500 , 250 ) , 0.35f ).OnKill ( ( ) => { sliderPanel.gameObject.SetActive ( true ); slider.sizeDelta = new Vector2 ( 1500 , 250 ); } );

            isSliderOn = true;
        }
        else
        {
            sliderPanel.gameObject.SetActive ( false );

            // Temp OnKill
            slider.DOSizeDelta ( new Vector2 ( 0 , 250 ) , 0.35f ).OnKill ( ( ) => { slider.sizeDelta = new Vector2 ( 0 , 250 ); slider.gameObject.SetActive ( false ); } );

            isSliderOn = false;
        }
    }
}
