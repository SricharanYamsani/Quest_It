using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController player;

    Camera mainCamera;

    private void Awake ( )
    {
        player = FindObjectOfType<PlayerController> ( );

        mainCamera = FindObjectOfType<Camera> ( );
    }

    private void FixedUpdate ( )
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray r = mainCamera.ScreenPointToRay ( Input.mousePosition );

            RaycastHit hit;

            if(Physics.Raycast(r,out hit))
            {
                player.endPos = hit.point;

                player.pState = PlayerState.MOVING;
            }
            Debug.Log ( player.endPos );
        }
    }
}
