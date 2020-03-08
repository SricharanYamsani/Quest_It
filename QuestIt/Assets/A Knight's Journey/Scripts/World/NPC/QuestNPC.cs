using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;

namespace RPG.NPCs
{
    public class QuestNPC : NPC
    {
        //============================Variables=====================//

        public List<Quest> quests = new List<Quest>();

        //============================Functions=====================//

        // Start is called before the first frame update
        //--------------------------
        public override void Start()
        {
            base.Start();
            npcType = NPCType.Quest;
            questLog = FindObjectOfType<QuestLog>();
        }

        //---------------------------------------
        public override void InteractWithPlayer()
        {
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
                    //TODO Dialogue
                    StartCoroutine(Dialogue());

                    questLog.DisplayQuestUpdateInfo("New Quest : " + unlockedQuests[i].questDesc);
                    questLog.AddQuest(unlockedQuests[i]);
                    //Remove quest given to player from quest list
                    for (int j = 0; j < quests.Count; j++)
                    {
                        if (unlockedQuests[i].id == quests[j].id)
                        {
                            quests.RemoveAt(j);
                        }
                    }
                }
            }
        }

        public IEnumerator Dialogue()
        {
            yield return null;
        }
    }
}