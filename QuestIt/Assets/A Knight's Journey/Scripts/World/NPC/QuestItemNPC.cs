using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;

namespace RPG.NPCs
{
    public class QuestItemNPC : NPC
    {
        [SerializeField] QuestEnums.ItemType itemType;
        
        //--------------------------
        public override void Start()
        {
            base.Start();
            npcType = NPCType.QuestItem;
            questLog = FindObjectOfType<QuestLog>();
            QuestEvents.TrackTask += TrackTask;
        }

        //-------------------
        private void Update()
        {
            //Player in interaction range
            if (Vector3.Distance(player.transform.position, transform.position) <= interactionRange)
            {
                interactionIcon.SetActive(true);
            }
            else
            {
                interactionIcon.SetActive(false);
            }
        }

        //----------------------------------------
        public void TrackTask(QuestTask questTask)
        {
            if (questTask.taskType.type == TaskType.Types.GATHER &&
                   questTask.taskType.gatherTargets.itemType == itemType)
            {
                QuestEvents.AddNPCLocation(this);
            }
        }

        //----------------------------------------
        public override void OnPlayerInteraction()
        {
            List<Quest> playerQuests = GameManager.Instance.playerQuests;
            for (int i = 0; i < playerQuests.Count; i++)
            {
                Quest quest = playerQuests[i];
                //Get current task requirement for every quest
                TaskType currentQuestTaskType = quest.questTasks[quest.currentQuestTaskIndex].taskType;

                //If item type matches quest task requirement update task
                /* TODO : only a single quest is updated */
                if (currentQuestTaskType.type == TaskType.Types.GATHER)
                {
                    if (itemType == currentQuestTaskType.gatherTargets.itemType)
                    {
                        quest.UpdateQuest();
                        break;
                    }
                }
            }
        }
    }
}
