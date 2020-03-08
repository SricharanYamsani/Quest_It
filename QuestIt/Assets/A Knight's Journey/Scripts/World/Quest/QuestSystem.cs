using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RPG.QuestSystem
{
    /* ------------------------------------------------------------------------
    Quest system is implemented using a Graph datastructure
    Quest Events represent graph nodes
    Quest Paths represent paths connecting nodes
    
    A graph datastructure is implemented to allow flexibility in quest creation
    including multiple paths towards quest completion
    --------------------------------------------------------------------------- */
    [System.Serializable]
    public class Quest 
    {
        //============================Variables=====================//
        [HideInInspector] public byte[] id;
        public QuestType questType;
        public string questDesc;
        public List<QuestTask> questTasks = new List<QuestTask>();
        int currentQuestTaskIndex;
        [HideInInspector] public bool completedQuest;

        //============================Functions=====================//

        //-----------------------------------------------------------
        public void TrackNextTask(TextMeshProUGUI sceneQuestInfoText)
        {
            if (questTasks[currentQuestTaskIndex].completedTask == true)
            {
                QuestEvents.TaskCompleted();
                currentQuestTaskIndex++;
                if (currentQuestTaskIndex >= questTasks.Count)
                {
                    completedQuest = true;
                    return;
                }
            }
            questTasks[currentQuestTaskIndex].DisplayTaskDescription(sceneQuestInfoText);
            QuestEvents.TrackTask(questTasks[currentQuestTaskIndex]);
        }
    }

    /* --------------------------------------------------------------
    Quest Events represent the nodes in the quest graph datastructure
    ----------------------------------------------------------------- */
    [System.Serializable]
    public class QuestTask
    {
        //==========================Variables===================//
        [HideInInspector] public bool completedTask;
        public string description;  //Description of the task
        public TaskType taskType;

        //=========================Functions====================//
       
        //----------------------
        public void UpdateTask()
        {
            taskType.killTargets.numberOfTimes--;
            if(taskType.killTargets.numberOfTimes <= 0)
            {
                completedTask = true;
                QuestEvents.TaskUpdated();
            }       
        }

        //--------------------------------------------------------------------
        public void DisplayTaskDescription(TextMeshProUGUI sceneQuestInfoText)
        {
            sceneQuestInfoText.text = description;   
        }
    }

    [System.Serializable]
    public class TaskType
    {
        public enum Types { KILL, DEFEND, GATHER }
        public Types type;

        public KillTargets killTargets = new KillTargets();
        public DefendTargets defendTargets = new DefendTargets();
        public GatherTargets gatherTargets = new GatherTargets();
    }

    [System.Serializable]
    public class KillTargets
    {
        public int numberOfTimes;
        public QuestEnums.NPCKillType npcType;
    }

    [System.Serializable]
    public class DefendTargets
    {
        public int numberOfTimes;
        public QuestEnums.DefendType npcType;
    }

    [System.Serializable]
    public class GatherTargets
    {
        public int numberOfTimes;
        public QuestEnums.ItemType itemType;
    }
        
    [System.Serializable]
    public class QuestType
    {
        public enum Type { MAIN, SIDE }
        public Type type;

        public UnlockRequirements unlockRequirements = new UnlockRequirements();
    }

    [System.Serializable]
    public class UnlockRequirements
    {
        //Player level
        public int level;
    }
}