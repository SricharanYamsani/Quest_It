using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;
using UnityEngine.SceneManagement;

namespace RPG.NPCs
{
    public class DuelNPC : NPC
    {
        public QuestEnums.NPCKillType npcKillType;
        QuestTask questTask;

        //--------------------------
        public override void Start()
        {
            base.Start();
            npcType = NPCType.Duel;
            QuestEvents.TrackTask += TrackTask;
            QuestEvents.TaskCompleted += TaskCompleted;
            QuestEvents.UntrackTask += UntrackTask;
        }

        //-------------------
        private void Update()
        {
            //Player in interaction range and quest available
            if (Vector3.Distance(player.transform.position, transform.position) <= interactionRange &&
                questTask != null)
            {
                if (questTask.taskType.type == TaskType.Types.KILL &&
                    questTask.taskType.killTargets.npcType == npcKillType)
                {
                    interactionIcon.SetActive(true);
                }
            } 
            else
            {
                interactionIcon.SetActive(false);
            }
        }

        //----------------------------------------
        public void TrackTask(QuestTask questTask)
        {
            this.questTask = questTask;
            if (questTask.taskType.type == TaskType.Types.KILL &&
                   questTask.taskType.killTargets.npcType == npcKillType)
            {
                QuestEvents.AddNPCLocation(this);
            }
        }

        //-------------------------
        public void TaskCompleted()
        {
            questTask = null;            
        }

        //-----------------------
        public void UntrackTask()
        {
            questTask = null;
        }

        //---------------------------------------
        public override void InteractWithPlayer()
        {
            //playerInfo.IsTeamRed = false;
            //player.playerInfo.IsTeamRed = true;
            //BattleInitializer.Instance.AddBattlePlayer(playerInfo); //NPC
            //BattleInitializer.Instance.AddBattlePlayer(player.playerInfo); //Player
            //SceneManager.LoadScene("Lobby");
            questTask.UpdateTask();
            gameObject.SetActive(false);
        }

        //----------------------
        public void OnDisable()
        {
            QuestEvents.TrackTask -= TrackTask;
            QuestEvents.TaskCompleted -= TaskCompleted;
            QuestEvents.UntrackTask -= UntrackTask;
        }
    }
}