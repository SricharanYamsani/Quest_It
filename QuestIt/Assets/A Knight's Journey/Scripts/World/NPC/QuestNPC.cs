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
        public Quest availableQuest;

        List<string> questDialog;
        bool questInitiationDialog;

        //============================Functions=====================//

        // Start is called before the first frame update
        //--------------------------
        public override void Awake()
        {
            base.Awake();
            npcType = NPCType.QuestGiver;
            questLog = FindObjectOfType<QuestLog>();

            QuestEvents.TrackTask += TrackTask;
            QuestEvents.QuestAccepted += QuestAccepted;
            QuestEvents.QuestCompleted += RemoveQuests;

            RemoveQuests();
        }

        //Removes acquired and completed quests from quest list
        //-------------------------------------------
        private void RemoveQuests(Quest quest = null)
        {
            //Remove quests from list that have already been acquired by the player
            List<Quest> playerQuests = GameManager.Instance.GetPlayerQuests();
            for (int i = 0; i < playerQuests.Count; i++)
            {
                if (playerQuests[i].questGiverID == npcID)
                {
                    int index = quests.FindIndex((Quest item) => item.id == playerQuests[i].id);
                    if (index != -1)
                    {
                        quests.RemoveAt(index);
                    }
                }
            }
            //Remove quests that have already been completed
            List<Quest> completedQuests = GameManager.Instance.GetCompletedQuests();
            if (completedQuests.Count > 0)
            {
                for (int i = 0; i < completedQuests.Count; i++)
                {
                    if (completedQuests[i].questGiverID == npcID)
                    {
                        int index = quests.FindIndex((Quest item) => item.id == completedQuests[i].id);
                        if (index != -1)
                        {
                            quests.RemoveAt(index);
                        }
                    }
                    CheckQuestAvailable(completedQuests[i]);
                }
            }
            else
            {
                CheckQuestAvailable();
            }              
        }

        //-------------------
        private void Update()
        {
            if (questDialog != null)
            {
                //Player in interaction range and quest available
                if (Vector3.Distance(player.transform.position, transform.position) <= interactionRange)
                {
                    questIcon.SetActive(false);
                    interactionIcon.SetActive(true);
                }
                //Player out of interaction range and quest available
                else if (Vector3.Distance(player.transform.position, transform.position) > interactionRange)
                {
                    questIcon.SetActive(true);
                    interactionIcon.SetActive(false);
                }
            }
        }

        //----------------------------------------
        public void TrackTask(QuestTask questTask)
        {
            if (questTask.taskType.type == TaskType.Types.TALK_TO_NPC &&
                questTask.taskType.talkToNPC.npcID == npcID)
            {
                QuestEvents.AddNPCLocation(this);
                questDialog = questTask.taskDialog;
                questInitiationDialog = false;                
            }
        }

        //-------------------------------------------------
        public void CheckQuestAvailable(Quest quest = null)
        {
            questDialog = null;

            //Get quests that are available according to the player level
            List<Quest> unlockedQuests = new List<Quest>();
            for (int i = 0; i < quests.Count; i++)
            {
                //TODO : Replace 0 with actual player level
                if(quests[i].questType.unlockRequirements.level == 0)
                {
                    if(quests[i].questType.unlockRequirements.questID != -1)
                    {
                        if (quest != null && quests[i].questType.unlockRequirements.questID == quest.id)
                        {
                            unlockedQuests.Add(quests[i]);
                        }                        
                    }
                    else
                    {
                        unlockedQuests.Add(quests[i]);
                    }
                }
            }

            for (int i = 0; i < unlockedQuests.Count; i++)
            {
                //Find a quest that is not yet given to the player
                Quest playerHasQuest = GameManager.Instance.GetPlayerQuests().Find((Quest item) => item.id == unlockedQuests[i].id);
                if (playerHasQuest == null)
                {
                    availableQuest = unlockedQuests[i];
                    questDialog = availableQuest.questDialog;
                    questInitiationDialog = true;
                    questIcon.SetActive(true);
                    return;
                }
            }

            //No quests available
            questIcon.SetActive(false);
            interactionIcon.SetActive(false);
            availableQuest = null;
        }

        //----------------------------------------
        public override void OnPlayerInteraction()
        {
            //Disable Player movement while dialog is active
            QuestEvents.InteractionStarted();

            //Start Dialogue
            QuestEvents.StartDialogue(questDialog, questInitiationDialog, npcID);
            interactionIcon.SetActive(false);            
        }

        //-----------------------------------
        private void QuestAccepted(int npcID)
        {
            if (this.npcID == npcID)
            {
                //Enable player movement
                QuestEvents.InteractionFinished();

                questLog.DisplayQuest(availableQuest.questDesc);
                questLog.SetupQuestInQuestLog(availableQuest);
                GameManager.Instance.AddAvailableQuestToPlayerQuests(availableQuest);
                questLog.ToggleQuestActive(availableQuest.id);

                //Remove quest given to player from quest list
                for (int j = 0; j < quests.Count; j++)
                {
                    if (availableQuest.id == quests[j].id)
                    {
                        quests.RemoveAt(j);
                    }
                }

                questDialog = null;
                questIcon.SetActive(false);
                interactionIcon.SetActive(false);
                //CheckQuestAvailable();
            }
        }

        //----------------------
        private void OnDisable()
        {
            QuestEvents.QuestAccepted -= QuestAccepted;
            QuestEvents.QuestCompleted -= RemoveQuests;
            QuestEvents.TrackTask -= TrackTask;
        }
    }
}