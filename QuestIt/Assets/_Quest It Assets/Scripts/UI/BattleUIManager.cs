using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BattleUIManager : Singleton<BattleUIManager>
{
    public GameObject choiceUI;

    public RectTransform slider = null;

    public RectTransform sliderPanel = null;

    public TextMeshProUGUI mText;

    bool isSliderOn = false;

    public BattlePlayer ourPlayer;

    public GameObject gameOverScreen;

    [SerializeField]
    List<BaseUIChoice> t_Choices = new List<BaseUIChoice> ( ); 

    protected override void  Awake ( )
    {
        base.Awake ( );

        gameOverScreen.SetActive ( false );

        BattleManager.Instance.GameInit += SetPlayer;

        BattleManager.Instance.GameOver += GameOverScreen;
        
        if ( !isSliderOn )
        {
            slider.sizeDelta = new Vector2 ( 0 , 250 );

            slider.gameObject.SetActive ( false );
        }
    }
    private void SetPlayer ( )
    {
        ourPlayer = BattleManager.Instance.currentPlayers [ 0 ];

        for ( int i = 0 ; i < t_Choices.Count ; i++ )
        {
            if ( i < ourPlayer.validChoices.Count )
            {
                t_Choices [ i ].m_Choice = ourPlayer.validChoices [ i ];

                t_Choices [ i ].SetupChoice ( );

                t_Choices [ i ].gameObject.SetActive ( true );
            }
            else
            {
                t_Choices [ i ].gameObject.SetActive ( false );
            }
        }
    }

    public void ToggleSlider()
    {
        if(!isSliderOn)
        {
            slider.gameObject.SetActive ( true );

            slider.DOSizeDelta ( new Vector2 ( 1500 , 250 ) , 0.35f ).OnKill ( ( ) => { slider.sizeDelta = new Vector2 ( 1500 , 250 ); } );

            isSliderOn = true;
        }
        else
        {
            // Temp OnKill
            slider.DOSizeDelta ( new Vector2 ( 0 , 250 ) , 0.35f ).OnKill ( ( ) => { slider.sizeDelta = new Vector2 ( 0 , 250 ); slider.gameObject.SetActive ( false ); } );

            isSliderOn = false;
        }
    }

    private void FixedUpdate ( )
    {
        mText.text = BattleManager.Instance.mTimer.ToString ( "00" );
    }

    private void GameOverScreen()
    {
        choiceUI.gameObject.SetActive ( true );

        gameOverScreen.SetActive ( true );
    }
}
