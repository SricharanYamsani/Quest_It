using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class BattlePlayer : MonoBehaviour
{
    public PlayerAttributes attributes = new PlayerAttributes ( );

    public List<BattleChoice> validChoices = new List<BattleChoice> ( );

    public BattleChoice currentChoice = null;

    public List<BattlePlayer> targetEnemies = new List<BattlePlayer> ( );

    public List<BattlePlayer> teamPlayers = new List<BattlePlayer> ( );

    public List<BattlePlayer> target = new List<BattlePlayer> ( );

    public Transform WorldUI = null;

    // Transforms for spawning Objects

    public Transform rightHandSpawnInside;

    public Transform rightHandSpawnOutside;

    public Transform headSpawn;

    public Transform faceSpawn;

    public Transform leftHandSpawnInside;

    public Transform leftHandSpawnOutside;

    public Transform meleeAttackSpawn;

    public RectTransform reactionLabel;

    [Header ( "UI OBJECTS" )]
    public Image fillingHpBar;

    public TextMeshProUGUI reactionText;

    public bool isPlayer = false;

    public Animator mPlayerController;

    private int fakeCurrentHealth;

    private Coroutine mCoroutine;

    private Sequence m_Sequence = DOTween.Sequence ( );

    private delegate void onComplete ( );

    onComplete coroutineComplete = null;

    private Color objectcolor = new Color ( 255 , 255 , 255 , 255 );

    private void Awake ( )
    {
        BattleManager.Instance.GameInit += SetArenaTargets;

        fakeCurrentHealth = attributes.curHealth;

        if ( validChoices.Count > 0 )
        {
            currentChoice = validChoices [ 0 ];
        }

        fakeCurrentHealth = attributes.curHealth;
    }
    private void SetArenaTargets ( )
    {
        if ( isPlayer )
        {
            targetEnemies = BattleManager.Instance.currentEnemies;

            teamPlayers = BattleManager.Instance.currentPlayers;
        }
        else
        {
            targetEnemies = BattleManager.Instance.currentPlayers;

            teamPlayers = BattleManager.Instance.currentEnemies;
        }
    }

    public void SetTargets ( )
    {
        if ( currentChoice.AttackStyle == ChoiceStyle.ATTACK )
        {
            if ( currentChoice.AttackValue == AttackRange.ALLENEMY )
            {
                target.InsertRange ( 0 , targetEnemies );
            }
            else if ( currentChoice.AttackValue == AttackRange.ONEENEMY )
            {
                target.Add ( targetEnemies [ 0 ] );
            }
            else if ( currentChoice.AttackValue == AttackRange.TWOENEMY )
            {

            }
        }
        else
        {
            //target = teamPlayers [ 0 ];
        }
    }
    public void CommitChoice ( )
    {
        if ( currentChoice == null )
        {
            if ( validChoices.Count > 0 )
            {
                currentChoice = validChoices [ 0 ];
            }
        }

        currentChoice.MoveWork ( );

        StartCoroutine ( GetBackToIdle ( currentChoice.endTime ) );

        target.Clear ( );
    }

    public void ShowReaction ( )
    {
        if ( currentChoice.AttackStyle == ChoiceStyle.DEFEND )
        {
            mPlayerController.SetTrigger ( AnimationType.BLOCK.ToString ( ) );

            reactionText.text = "MISS!";
        }
        else if ( currentChoice.AttackStyle == ChoiceStyle.ATTACK )
        {
            mPlayerController.SetTrigger ( AnimationType.HIT.ToString ( ) );

            reactionText.text = "HIT";
        }
        else
        {
            // Heal animation
        }

        if ( m_Sequence != null )
        {
            m_Sequence.Kill ( );
        }
        reactionLabel.gameObject.SetActive ( true );

        m_Sequence.Append ( reactionLabel.DOAnchorPos ( new Vector2 ( 0 , 3 ) , 0.5f ).SetEase ( Ease.Linear ) ).Join ( reactionText.DOFade ( 0.2f , 0.45f ) ).OnKill ( ( ) =>

        {
            reactionLabel.anchoredPosition = Vector2.one;

            reactionText.color = objectcolor;

            reactionLabel.gameObject.SetActive ( false );
        } );
    }

    private IEnumerator GetBackToIdle (float waitTime)
    {
        yield return new WaitForSeconds ( waitTime );
    }

    public void UpdateHealth ( )
    {
        Debug.Log ( this.name );

        DOTween.To ( ( ) => fakeCurrentHealth , x => fakeCurrentHealth = x , attributes.curHealth , 1 ).OnUpdate ( ( ) =>
        {
            if ( WorldUI )
            {
                fillingHpBar.fillAmount = ( float ) fakeCurrentHealth / attributes.maxHealth;
            }

        } ).OnComplete ( ( ) =>
        {

            if ( WorldUI )
            {
                fillingHpBar.fillAmount = ( float ) fakeCurrentHealth / attributes.maxHealth;
            }
        } );
    }
}