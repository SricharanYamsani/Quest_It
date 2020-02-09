using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RPG.Quest
{
    /* --------------------------------------------------------------
    Quest Events represent the nodes in the quest graph datastructure
    ----------------------------------------------------------------- */
    public class QuestEvent
    {
        //==========================Variables===================//
        public enum EventStatus { WAITING, CURRENT, DONE }; //Keep track of quest task status
        public EventStatus status;

        //Task Information
        public byte[] id; //Unique ID assigned to a quest task on creation
        public string description;  //Description of the task
        public string name;
        public int orderNumber = -1;
        public List<QuestPath> pathList = new List<QuestPath>(); //Path to next task...Can be multiple

        //=========================Functions====================//

        //Constructor
        //-----------------------------------
        public QuestEvent(string n, string d)
        {
            id = Guid.NewGuid().ToByteArray();
            name = n;
            description = d;
            status = EventStatus.WAITING;
        }

        //------------------------------------------
        public void UpdateEventState(EventStatus es)
        {
            status = es;
        }

        //-------------------
        public byte[] GetID()
        {
            return id;
        }

        //---------------------
        public void PrintPath()
        {
            for (int i = 0; i < pathList.Count; i++)
            {
                pathList[i].PrintConnectingEvents();
            }
        }
    }
}