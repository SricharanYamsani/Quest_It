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

        [SerializeField] GameObject questIcon;
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
            CheckQuestAvailable();

            QuestEvents.QuestAccepted += QuestAccepted;
            QuestEvents.QuestCompleted += CheckQuestAvailable;
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

        //-------------------------------
        public void CheckQuestAvailable()
        {
            //Get quests that are available according to the player level
            List<Quest> unlockedQuests = quests.FindAll((Quest quest) =>
                                quest.questType.unlockRequirements.level == 0);

            for (int i = 0; i < unlockedQuests.Count; i++)
            {
                //Find a quest that is not yet given to the player
                Quest playerHasQuest = questLog.quests.Find((Quest quest) => quest.id == unlockedQuests[i].id);
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
        public override void InteractWithPlayer()
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
            questLog.AddQuest(availableQuest);
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