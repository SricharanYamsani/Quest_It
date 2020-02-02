using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Movement
{
    public class FollowNPC : MonoBehaviour, IAction
    {
        //===========================Variables=================================//

        [SerializeField] float stopRange; //Defines the range at which player character should stop following a NPC
        Transform targetNPCTransform;
        WorldMovement worldMovement;
        ActionScheduler actionScheduler;

        //==========================Functions==================================//

        //------------------  
        private void Start()
        {
            worldMovement = GetComponent<WorldMovement>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        //-------------------
        private void Update()
        {
            FollowTargetNPC();
        }

        //Follows patrolling / moving NPCs for interaction purposes
        //--------------------
        void FollowTargetNPC()
        {
            if (targetNPCTransform == null) { return; }

            //Follow if player has selected a NPC upto a given range
            if (!InRange())
            {
                Debug.Log("Not In Range");
                worldMovement.MoveTo(targetNPCTransform.position);
            }
            else
            {
                Debug.Log("in Range");
                worldMovement.CancelAction();
            }
        }

        //----------------------------------------------------------
        public void SetTargetTransform(Transform targetNPCTransform)
        {
            actionScheduler.StartAction(this);
            this.targetNPCTransform = targetNPCTransform;
        }

        //--------------------
        private bool InRange()
        {
            return (Vector3.Distance(transform.position, targetNPCTransform.position) < stopRange);
        }

        //------------------
        public void CancelAction()
        {
            targetNPCTransform = null;
        }
    }
}