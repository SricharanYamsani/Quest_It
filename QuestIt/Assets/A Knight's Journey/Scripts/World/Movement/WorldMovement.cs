using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Control;
using RPG.QuestSystem;

namespace RPG.Movement
{
    public class WorldMovement : MonoBehaviour, IAction
    {
        //============================Variables=======================//

        NavMeshAgent agent;
        Animator animator;
        public ActionScheduler actionScheduler;

        //============================Functions======================//

        // Start is called before the first frame update
        //----------
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            QuestEvents.InteractionStarted += DisableMovement;
            QuestEvents.InteractionFinished += EnableMovement;            
        }

        // Update is called once per frame
        //-----------
        void Update()
        {
            UpdateAnimator();
        }

        //----------------------------------------------
        public void StartMoveAction(Vector3 destination)
        {
            if (agent.enabled)
            {
                actionScheduler.StartAction(this);
                MoveTo(destination);
            }
        }

        //-------------------------------------
        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;
        }

        //------------------------
        public void CancelAction()
        {
            if (agent.enabled)
            {
                agent.isStopped = true;
            }
        }

        //--------------------------
        public void EnableMovement()
        {
            if (GetComponent<PlayerWorldController>())
            { agent.enabled = true; }
        }

        //---------------------------
        public void DisableMovement()
        {
            if (GetComponent<PlayerWorldController>())
            { agent.enabled = false; }
        }

        //---------------------------
        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
            float speed = localVelocity.z;
            animator.SetFloat("ForwardSpeed", speed);
        }

        //--------------
        void OnDisable()
        {
            QuestEvents.InteractionStarted -= DisableMovement;
            QuestEvents.InteractionFinished -= EnableMovement;
        }
    }
}
