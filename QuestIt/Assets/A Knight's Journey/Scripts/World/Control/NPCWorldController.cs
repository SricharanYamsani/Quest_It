using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    public class NPCWorldController : MonoBehaviour, IAction
    {
        //===========================Variables====================//

        //Look At
        [SerializeField] float lookAtDuration; //Defines how fast a npc should turn to face the player
        [SerializeField] float lookAtRange;    //Defines the range at which npc stops to face the player
        Coroutine lookAtCoroutine;

        //Movement
        WorldMovement worldMovement;
        [SerializeField] List<Transform> wayPoints;
        Transform nextWayPoint;
        int wayPointIndex;
        Transform playerTransform;

        ActionScheduler actionScheduler;

        //==========================Functions====================//

        // Start is called before the first frame update
        //----------
        void Start()
        {
            worldMovement = GetComponent<WorldMovement>();
            playerTransform = FindObjectOfType<PlayerWorldController>().transform;
            actionScheduler = GetComponent<ActionScheduler>();            
        }

        //-----------
        void Update()
        {
            //Player within look at range
            if (Vector3.Distance(playerTransform.position, transform.position) < lookAtRange)
            {
                LookAtPlayer();
            }
            else
            {
                MoveToNextWaypoint();
            }
        }

        //------------------------------
        public void MoveToNextWaypoint()
        {
            //Set Initial way point and start moving
            if(nextWayPoint == null)
            {
                nextWayPoint = wayPoints[0];
            }

            //Reached current waypoint
            if (Vector3.Distance(transform.position, nextWayPoint.position) < 0.1f)
            {
                wayPointIndex++;
                //Reset to initial waypoint
                if (wayPointIndex == wayPoints.Count)
                {
                    wayPointIndex = 0;
                }
                nextWayPoint = wayPoints[wayPointIndex];
            }
            worldMovement.StartMoveAction(nextWayPoint.position);
        }

        //-------------------------
        private void LookAtPlayer()
        {
            if (lookAtCoroutine == null)
            {
                StartLookAtAction();
            }
            else
            {
                CancelAction();
                StartLookAtAction();
            }
        }

        //------------------------------
        private void StartLookAtAction()
        {
            actionScheduler.StartAction(this);
            lookAtCoroutine = StartCoroutine(LookAtSmooth(playerTransform));
        }

        //Smoothly rotate towards player
        //--------------------------------------------------------
        public IEnumerator LookAtSmooth(Transform targetTransform)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion newRot = Quaternion.LookRotation(targetTransform.position - transform.position,
                                                        transform.TransformDirection(Vector3.up));
            float counter = 0;
            while (counter < lookAtDuration)
            {
                counter += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / lookAtDuration);
                yield return null;
            }
        }

        //------------------------
        public void CancelAction()
        {
            if (lookAtCoroutine != null)
            {
                StopCoroutine(lookAtCoroutine);
            }
        }
    }
}