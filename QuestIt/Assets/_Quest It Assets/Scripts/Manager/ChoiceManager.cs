using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : Singleton<ChoiceManager>
{
    RaycastHit getHit;

    public LayerMask playerMask;

    private void Update ( )
    {
        if ( Input.GetMouseButtonDown ( 0 ) )
        {
            if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ) , out getHit , playerMask ) )
            {
                if ( getHit.collider.gameObject.CompareTag ( "Player" ) )
                {
                    // Selections
                }
                else
                {
                    Debug.Log ( getHit.collider.name );
                }
            }
        }
    }
    private void OnDrawGizmos ( )
    {
        
    }
}
