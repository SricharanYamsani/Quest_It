using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

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
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        //--------------------------------------
        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;
        }

        //------------------
        public void CancelAction()
        {
            agent.isStopped = true;
        }

        //---------------------------
        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
            float speed = localVelocity.z;
            animator.SetFloat("ForwardSpeed", speed);
        }
    }
}
