using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : Singleton<ChoiceManager>
{
    RaycastHit getHit;

    public LayerMask playerMask;

    public bool isSelectingTarget;

    public int targetIndex;

    private int currentTargets;

    private void Update ( )
    {
        if ( isSelectingTarget )
        {
            if ( currentTargets < targetIndex )
            {
                LookForInput ( );
            }
        }
    }
    private void OnDrawGizmos ( )
    {

    }

    private void LookForInput ( )
    {
        if ( Input.GetMouseButtonDown ( 0 ) )
        {
            if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ) , out getHit , playerMask ) )
            {
                if ( getHit.collider.gameObject.CompareTag ( "Player" ) )
                {
                    // Selections
                    currentTargets++;

                    if ( currentTargets == targetIndex )
                    {
                        isSelectingTarget = false;
                    }
                }
                else
                {
                    Debug.Log ( getHit.collider.name );
                }
            }
        }
    }
    public void StartSelecting (AttackRange range)
    {
        switch ( range )
        {
            case AttackRange.ONEENEMY:
            case AttackRange.ONETEAM:
            isSelectingTarget = true;
            break;
            case AttackRange.ALLENEMY:
            //SHOW ALL TARGETED ENEMIES
            break;
            case AttackRange.ALLTEAM:
            //SHOW ALL TARGETED TEAMS
            break;
            case AttackRange.EVERYONE:
            //SHOW ALL
            break;
        }
    }
}
