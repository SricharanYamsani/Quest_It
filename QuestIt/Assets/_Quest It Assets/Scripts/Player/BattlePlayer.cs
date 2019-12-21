using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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

    [Header ( "UI OBJECTS" )]
    public Image fillingHpBar;

    public bool isPlayer = false;


    public Animator mPlayerController;

    private int fakeCurrentHealth;

    private Coroutine mCoroutine;

    private delegate void onComplete ( );

    onComplete coroutineComplete = null;

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
    private void Start ( )
    {
        
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

    private void LateUpdate ( )
    {
        if ( WorldUI )
        {
            fillingHpBar.fillAmount = ( float ) attributes.curHealth / attributes.maxHealth;
        }
    }

    public void Health (int mHealth , ChoiceStyle attackType)
    {
        GraphicHealthRun ( mHealth , attackType );
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
        if ( validChoices.Count > 0 )
        {
            currentChoice = validChoices [ 0 ];
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
        }
        else if ( currentChoice.AttackStyle == ChoiceStyle.ATTACK )
        {
            GraphicHealthRun ( BattleManager.Instance.currentPlayer.currentChoice.healthChange , BattleManager.Instance.currentPlayer.currentChoice.AttackStyle );

            mPlayerController.SetTrigger ( AnimationType.HIT.ToString ( ) );
        }
        else
        {
            // Heal animation
        }
    }

    private IEnumerator GetBackToIdle (float waitTime)
    {
        yield return new WaitForSeconds ( waitTime );
    }

    public void GraphicHealthRun (int mx , ChoiceStyle attackType)
    {
        if ( mCoroutine != null )
        {
            coroutineComplete?.Invoke ( );

            coroutineComplete = null;
        }

        mCoroutine = StartCoroutine ( UpdateHealth ( mx , attackType, ()=> {

            if(attackType == ChoiceStyle.ATTACK)
            {
                attributes.curHealth -= mx;
            }
            else if(attackType == ChoiceStyle.DEFEND)
            {

            }


            if(attributes.curHealth<= 0)
            {
                mPlayerController.SetTrigger ( AnimationType.DEAD.ToString ( ) );
            }

        }

        ) );
    }

    IEnumerator UpdateHealth (int changeInHealth , ChoiceStyle attackType , onComplete complete = null)
    {
        int xFactor = 1;

        int oldValue = attributes.curHealth;

        coroutineComplete = complete;

        if ( attackType == ChoiceStyle.DEFEND || attackType == ChoiceStyle.HEAL )
        {
            while ( attributes.curHealth > oldValue + changeInHealth )
            {
                yield return null;

                attributes.curHealth -= xFactor;
            }

            attributes.curHealth = Mathf.Clamp ( oldValue + changeInHealth , 0 , attributes.maxHealth );
        }
        else
        {
            while ( attributes.curHealth > oldValue - changeInHealth )
            {
                yield return null;

                attributes.curHealth -= xFactor;
            }
        }

        coroutineComplete?.Invoke ( );
    }
}