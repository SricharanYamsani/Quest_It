using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        //=====================Variables===================//

        IAction currentAction;

        //=====================Functions===================//

        //Cancels previous action and stores reference to current action
        //-------------------------------------
        public void StartAction(IAction action)
        {
            if (action == currentAction) return;
            if(currentAction != null)
            {
                currentAction.CancelAction();
            }
            currentAction = action;
        }
    }
}