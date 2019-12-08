using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayer : MonoBehaviour
{
    public PlayerAttributes attributes = new PlayerAttributes ( );

    public List<BattleChoice> validChoices = new List<BattleChoice> ( );

    public BattleChoice currentChoice = null;

    public List<BattlePlayer> targetEnemies = new List<BattlePlayer> ( );

    public List<BattlePlayer> teamPlayers = new List<BattlePlayer> ( );

    public BattlePlayer target;

    public Transform WorldUI = null;

    [Header ( "UI OBJECTS" )]
    public Image fillingHpBar;

    public bool isPlayer = false;


    public Animator mPlayerController;

    private int fakeCurrentHealth;

    private void Awake ( )
    {
        BattleManager.Instance.GameInit += SetArenaTargets;

        fakeCurrentHealth = attributes.curHealth;

        if ( validChoices.Count > 0 )
        {
            currentChoice = validChoices [ 0 ];
        }
    }
    private void SetArenaTargets ( )
    {
        targetEnemies = BattleManager.Instance.currentEnemies;

        teamPlayers = BattleManager.Instance.currentPlayers;
    }

    private void LateUpdate ( )
    {
        if ( WorldUI )
        {
            WorldUI.transform.position = transform.position;

            fillingHpBar.fillAmount = ( float ) attributes.curHealth / attributes.maxHealth;
        }
    }

    public void Health (int mHealth)
    {
        if ( attributes.curHealth + mHealth > 0 )
        {
            attributes.curHealth = attributes.curHealth + mHealth > attributes.maxHealth ? attributes.maxHealth : attributes.curHealth + mHealth;
        }
        else
        {
            attributes.curHealth = 0;

            mPlayerController.SetTrigger ( "Dying" );
        }
    }
    public void CommitChoice ( )
    {
        if ( validChoices.Count > 0 )
        {
            currentChoice = validChoices [ 0 ];
        }

        if ( isPlayer )
        {
            if ( currentChoice.AttackStyle == ChoiceStyle.ATTACK )
            {
                target = targetEnemies [ 0 ];
            }
            else
            {
                target = teamPlayers [ 0 ];
            }
        }
        else
        {
            if ( currentChoice.AttackStyle == ChoiceStyle.ATTACK )
            {
                target = teamPlayers [ 0 ];
            }
            else
            {
                target = targetEnemies [ 0 ];
            }

        }
        currentChoice.mWork ( target );

        StartCoroutine ( GetBackToIdle ( currentChoice.endTime ) );

        mPlayerController.SetTrigger ( "Attack" );
    }

    private IEnumerator GetBackToIdle (float waitTime)
    {
        yield return new WaitForSeconds ( waitTime );
    }
}
