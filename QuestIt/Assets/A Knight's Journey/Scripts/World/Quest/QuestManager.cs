using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    /* -------------------------------------------------
    Quest Manager is responsible for creating new quests
    and keeping track of all active quests
    ---------------------------------------------------- */

    public class QuestManager : MonoBehaviour
    {
        //====================Variables==============//
        public Quest quest = new Quest();

        //====================Functions==============//
        //------------------
        private void Start()
        {
            //Create a sample quest

            QuestEvent a = quest.AddQuestEvent("task1", "desc1");
            QuestEvent b = quest.AddQuestEvent("task2", "desc2");
            QuestEvent c = quest.AddQuestEvent("task3", "desc3");
            QuestEvent d = quest.AddQuestEvent("task4", "desc4");
            QuestEvent e = quest.AddQuestEvent("task5", "desc5");

            quest.AddPath(a.GetID(), b.GetID()); //a to b
            quest.AddPath(b.GetID(), c.GetID()); //b to c multiple path
            quest.AddPath(b.GetID(), d.GetID()); //b to d multiple path
            quest.AddPath(c.GetID(), e.GetID()); //c to e
            quest.AddPath(d.GetID(), e.GetID()); //d to e

            quest.BFS(a);
            quest.PrintQuestSystem();
        }
    }
}