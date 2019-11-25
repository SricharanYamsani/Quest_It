using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleManager instance;

    public BattleManager Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<BattleManager> ( );

                if ( instance == null )
                {
                    GameObject nObject = new GameObject ( );

                    nObject.name = "BattleManager";

                    instance = nObject.AddComponent<BattleManager> ( );
                }
            }
            return instance;
        }
    }

    // VARIABLE SECTION

    [Header ( "-BATTLE STATE-" )]
    public BattleStates battleState = BattleStates.NONE;

    [Header ( "CONSTANTS" )]
    private const int MTime = 15;

    [Header ( "INTEGERS" )]
    public int mTimer = 0;

    private void Awake ( )
    {

    }
    public void SwitchPlayState (BattleStates mState)
    {
        battleState = mState;

        PlayState ( );
    }

    public void PlayState ( )
    {
        switch ( battleState )
        {
            case BattleStates.NONE: break;

            case BattleStates.CHOICE:

            StartCoroutine ( StartProcess ( ) );

            break;

            case BattleStates.BATTLE:

            // Check Who Attacks First
            // Play
            //Check If Dead or Not
            //If not, play
            //else
            //end
            break;

            case BattleStates.OUTCOME:

            // Show the Outcome
            // End the Lobby
            break;
        }
    }

    private IEnumerator StartProcess()
    {
        mTimer = MTime;

        while(mTimer>0)
        {
            yield return new WaitForSeconds ( 1f );
            mTimer--;
        }
        yield return null;

        SwitchPlayState ( BattleStates.BATTLE );
    }
}

public enum BattleStates
{
    NONE,
    CHOICE,
    BATTLE,
    OUTCOME
}
