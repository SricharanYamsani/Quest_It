using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    /* ---------------------------------------------
    Quest Path represents the paths connecting nodes
    in the quest graph datatstructure
    ------------------------------------------------ */
    public class QuestPath
    {
        //===================Variables==============//
        public QuestEvent startEvent; //Previous task
        public QuestEvent endEvent; //Next task

        //===================Functions==============//
        //Constructor
        //----------------------------------------------
        public QuestPath(QuestEvent from, QuestEvent to)
        {
            startEvent = from;
            endEvent = to;
        }

        //---------------------------------
        public void PrintConnectingEvents()
        {
            Debug.Log(startEvent.name + " " + endEvent.name);
        }
    }
}