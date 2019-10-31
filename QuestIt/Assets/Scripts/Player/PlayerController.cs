using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float p_speed = 0;

    public float p_time = 1;

    public PlayerState pState = PlayerState.NONE;

    public GameObject player;

    public NavMeshAgent agent;

    public Vector3 endPos = new Vector3 ( 0 , 0.15f , 0 );

    public Animator curAnimator = null;

    bool speedCalculated = false;

    private void Awake ( )
    {
        curAnimator = player.GetComponent<Animator> ( );
    }
    // Start is called before the first frame update
    void Start ( )
    {
        transform.position = new Vector3 ( 0 , 0.15f , 0 );
    }

    public void CalculateSpeed ( )
    {
        float distance = Mathf.Abs ( Vector3.Distance ( transform.position , endPos ) - 0.15f );

        p_speed = Mathf.Clamp ( ( distance / p_time ) , 0 , 1 );

        curAnimator.SetFloat ( "Speed" , p_speed );

        Debug.Log ( p_speed );
    }

    public void MoveTowards ( )
    {

    }

    private void Update ( )
    {
        CalculateSpeed ( );

        if ( pState == PlayerState.MOVING )
        {
            agent.SetDestination ( endPos );

            pState = PlayerState.NONE;
        }
        if ( Mathf.Abs ( Vector3.Distance ( transform.position , endPos ) ) <= 0.15f )
        {
            curAnimator.SetFloat ( "Speed" , 0 );
        }
    }
}