using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;

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
        }

        //----------------------------------------
        public void TrackTask(QuestTask questTask)
        {
            this.questTask = questTask;
        }   

        //-------------------------
        public void TaskCompleted()
        {
            questTask = null;
        }

        //---------------------------------------
        public override void InteractWithPlayer()
        {
            BattleInitializer.Instance.AddaBattlePlayer(playerInfo); //NPC
            BattleInitializer.Instance.AddaBattlePlayer(player.playerInfo); //Player
            BattleInitializer.Instance.InitializeLobby();
            
            //if (questTask != null)
            //{
            //    if (questTask.taskType.type == TaskType.Types.KILL &&
            //       questTask.taskType.killTargets.npcType == npcKillType)
            //    {
            //        questTask.UpdateTask();
            //    }
            //}
        }

        //----------------------
        public void OnDisable()
        {
            QuestEvents.TrackTask -= TrackTask;
            QuestEvents.TaskCompleted -= TaskCompleted;
        }
    }
}