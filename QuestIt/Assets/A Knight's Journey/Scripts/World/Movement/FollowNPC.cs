using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Core;
using RPG.Control;

namespace RPG.Movement
{
    public class FollowNPC : MonoBehaviour, IAction
    {
        //===========================Variables=================================//

        [SerializeField] float stopRange; //Defines the range at which player character should stop following a NPC
        Transform targetNPCTransform;
        WorldMovement worldMovement;
        ActionScheduler actionScheduler;
        [SerializeField] EventSystem eventSystem;

        //==========================Functions==================================//

        //------------------  
        private void Start()
        {
            worldMovement = GetComponent<WorldMovement>();
            actionScheduler = worldMovement.actionScheduler;
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
                worldMovement.MoveTo(targetNPCTransform.position);
            }
            else
            {
                worldMovement.CancelAction();
            }
        }

        //----------------------------------------------------------
        public void SetTargetTransform(Transform targetNPCTransform)
        {
            actionScheduler.StartAction(this);
            this.targetNPCTransform = targetNPCTransform;
        }

        //-------------------
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