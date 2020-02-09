using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    /* ------------------------------------------------------------------------
    Quest system is implemented using a Graph datastructure
    Quest Events represent graph nodes
    Quest Paths represent paths connecting nodes
    
    A graph datastructure is implemented to allow flexibility in quest creation
    including multiple paths towards quest completion
    --------------------------------------------------------------------------- */
    public class Quest
    {
        //============================Variables=====================//

        List<QuestEvent> questEvents = new List<QuestEvent>();
        List<QuestEvent> pathList = new List<QuestEvent>();

        //============================Functions=====================//

        //Constructor
        //------------
        public Quest() { }

        //--------------------------------------------------
        public QuestEvent AddQuestEvent(string n, string d)
        {
            QuestEvent questEvent = new QuestEvent(n, d);
            questEvents.Add(questEvent);
            return questEvent;
        }

        //Creates a path connecting two quest events
        //and adds path to the first quest event
        //-------------------------------------------------------------
        public void AddPath(byte[] fromQuestEvent, byte[] toQuestEvent)
        {            
            QuestEvent from = FindQuestEvent(fromQuestEvent);
            QuestEvent to = FindQuestEvent(toQuestEvent);

            if (from != null && to != null)
            {
                QuestPath p = new QuestPath(from, to);
                from.pathList.Add(p);
            }
        }

        //Finds quest event from given quest id
        //-------------------------------------
        QuestEvent FindQuestEvent(byte[] id)
        {
            for (int i = 0; i < questEvents.Count; i++)
            {
                if (questEvents[i].GetID() == id)
                {
                    return questEvents[i];
                }
            }
            return null;
        }

        //Breadth First Search to order tasks in a quest
        //--------------------------------------------------------------
        public void BFS(QuestEvent startQuestEvent, int orderNumber = 1)
        {
            QuestEvent thisEvent = startQuestEvent;
            thisEvent.orderNumber = orderNumber;
            for (int i = 0; i < thisEvent.pathList.Count; i++)
            {
                if(thisEvent.pathList[i].endEvent.orderNumber == -1)
                {
                    BFS(thisEvent.pathList[i].endEvent, orderNumber + 1);
                }
            }
        }

        //----------------------------
        public void PrintQuestSystem()
        {
            for (int i = 0; i < questEvents.Count; i++)
            {
                Debug.Log(questEvents[i].name + " " + questEvents[i].orderNumber);
                questEvents[i].PrintPath();
            }
        }
    }
}