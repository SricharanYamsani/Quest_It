using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;
using UnityEngine.SceneManagement;

namespace RPG.NPCs
{
    public class DuelNPC : NPC
    {
        public BattleCharacters npcKillType;
        
        //--------------------------
        public override void Start()
        {
            base.Start();
            npcType = NPCType.Duel;
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
            if (questTask.taskType.type == TaskType.Types.KILL &&
                   questTask.taskType.killTargets.npcType == npcKillType)
            {
                QuestEvents.AddNPCLocation(this);
            }
        }

        //---------------------------------------
        public override void OnPlayerInteraction()
        {
            npcInfo.IsTeamRed = false;
            player.playerInfo.IsTeamRed = true;

            GameManager.Instance.PreBattleSetup(npcInfo, player.playerInfo);            
        }

        //----------------------
        public void OnDisable()
        {
            QuestEvents.TrackTask -= TrackTask;            
        }
    }
}