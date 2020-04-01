using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;
using TMPro;

namespace RPG.NPCs
{
    public class QuestNPC : NPC
    {
        //============================Variables=====================//

        public List<Quest> quests = new List<Quest>();
        Quest availableQuest;
               
        //============================Functions=====================//

        // Start is called before the first frame update
        //--------------------------
        public override void Start()
        {
            base.Start();
            npcType = NPCType.Quest;
            questLog = FindObjectOfType<QuestLog>();

            QuestEvents.QuestAccepted += QuestAccepted;
        }

        //---------------------------------------
        public override void InteractWithPlayer()
        {
            //TODO implement proper player interaction with dialogue bubble etc.

            //TODO use actual player level
            //Get quests that are available according to the player level
            List<Quest> unlockedQuests = quests.FindAll((Quest quest) => 
                                quest.questType.unlockRequirements.level == 0);

            for (int i = 0; i < unlockedQuests.Count; i++)
            {
                //Find a quest that is not yet given to the player
                Quest playerHasQuest = questLog.quests.Find((Quest quest) => quest.id == unlockedQuests[i].id);
                if(playerHasQuest == null)
                {
                    availableQuest = unlockedQuests[i];

                    //Start Dialogue
                    QuestEvents.StartDialogue(availableQuest);
                    break;
                }
            }

            completedInteraction = true;
        }

        //--------------------------
        private void QuestAccepted()
        {
            questLog.DisplayQuestUpdateInfo("New Quest : " + availableQuest.questDesc);
            questLog.AddQuest(availableQuest);
            //Remove quest given to player from quest list
            for (int j = 0; j < quests.Count; j++)
            {
                if (availableQuest.id == quests[j].id)
                {
                    quests.RemoveAt(j);
                }
            }           
        }

        //----------------------
        private void OnDisable()
        {
            QuestEvents.QuestAccepted -= QuestAccepted;
        }
    }
}