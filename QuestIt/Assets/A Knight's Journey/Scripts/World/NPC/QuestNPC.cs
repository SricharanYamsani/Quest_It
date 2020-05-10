using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;
using System;
using TMPro;

namespace RPG.NPCs
{
    public class QuestNPC : NPC
    {
        //============================Variables=====================//

        [SerializeField] GameObject questIcon;
        public List<Quest> quests = new List<Quest>();
        Quest availableQuest;
                               
        //============================Functions=====================//

        // Start is called before the first frame update
        //--------------------------
        public override void Start()
        {
            base.Start();
            npcType = NPCType.QuestGiver;
            questLog = FindObjectOfType<QuestLog>();

            QuestEvents.QuestAccepted += QuestAccepted;
            QuestEvents.QuestCompleted += CheckQuestAvailable;

            RemoveQuests();
        }

        //Removes acquired and completed quests from quest list
        //-------------------------
        private void RemoveQuests()
        {
            //Remove quests from list that have already been acquired by the player
            List<Quest> playerQuests = GameManager.Instance.GetPlayerQuests();
            for (int i = 0; i < playerQuests.Count; i++)
            {
                int index = quests.FindIndex((Quest quest) => quest.id == playerQuests[i].id);
                quests.RemoveAt(index);
            }
            //Remove quests that have already been completed
            List<Quest> completedQuests = GameManager.Instance.GetCompletedQuests();
            for (int i = 0; i < completedQuests.Count; i++)
            {
                int index = quests.FindIndex((Quest quest) => quest.id == completedQuests[i].id);
                quests.RemoveAt(index);
            }
            CheckQuestAvailable();
        }

        //-------------------
        private void Update()
        {
            //Player in interaction range and quest available
            if(Vector3.Distance(player.transform.position, transform.position) <= interactionRange && 
                availableQuest != null)
            {
                questIcon.SetActive(false);
                interactionIcon.SetActive(true);
            }
            //Player out of interaction range and quest available
            else if(Vector3.Distance(player.transform.position, transform.position) > interactionRange &&
                availableQuest != null)
            {
                questIcon.SetActive(true);
                interactionIcon.SetActive(false);
            }
        }

        //-------------------------------------------------
        public void CheckQuestAvailable(Quest quest = null)
        {
            //Get quests that are available according to the player level
            List<Quest> unlockedQuests = quests.FindAll((Quest item) =>
                                item.questType.unlockRequirements.level == 0);

            for (int i = 0; i < unlockedQuests.Count; i++)
            {
                //Find a quest that is not yet given to the player
                Quest playerHasQuest = GameManager.Instance.playerQuests.Find((Quest item) => item.id == unlockedQuests[i].id);
                if (playerHasQuest == null)
                {
                    availableQuest = unlockedQuests[i];
                    questIcon.SetActive(true);
                    return;
                }
            }

            //No quests available
            questIcon.SetActive(false);
            interactionIcon.SetActive(false);
            availableQuest = null;
        }

        //---------------------------------------
        public override void OnPlayerInteraction()
        {
            //Disable Player movement while dialog is active
            QuestEvents.InteractionStarted();

            //Start Dialogue
            QuestEvents.StartDialogue(availableQuest);
            interactionIcon.SetActive(false);
        }

        //--------------------------
        private void QuestAccepted()
        {
            //Enable player movement
            QuestEvents.InteractionFinished();

            questLog.DisplayQuest(availableQuest.questDesc);
            questLog.SetupQuestInQuestLog(availableQuest);
            GameManager.Instance.playerQuests.Add(availableQuest);
            questLog.ToggleQuestActive(availableQuest.id);

            //Remove quest given to player from quest list
            for (int j = 0; j < quests.Count; j++)
            {
                if (availableQuest.id == quests[j].id)
                {
                    quests.RemoveAt(j);
                }
            }

            CheckQuestAvailable();
        }

        //----------------------
        private void OnDisable()
        {
            QuestEvents.QuestAccepted -= QuestAccepted;
            QuestEvents.QuestCompleted -= CheckQuestAvailable;
        }
    }
}