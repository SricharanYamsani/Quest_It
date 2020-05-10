using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.NPCs;

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
        public int id;
        public QuestType questType;
        public string questDesc;
        public List<QuestTask> questTasks = new List<QuestTask>();
        public int currentQuestTaskIndex;
        
        [HideInInspector] public bool completedQuest;
        [HideInInspector] public bool questCompletionDisplayed;

        public List<string> questDialog;
        public int currentDialogueIndex;

        //============================Functions=====================//
                
        //-----------------------------------------------------------
        public void TrackNextTask(TextMeshProUGUI sceneQuestInfoText)
        {
            questTasks[currentQuestTaskIndex].DisplayTaskDescription(sceneQuestInfoText);
            QuestEvents.TrackTask(questTasks[currentQuestTaskIndex]);
        }

        //-----------------------
        public void UpdateQuest()
        {
            questTasks[currentQuestTaskIndex].UpdateTask();
            if (questTasks[currentQuestTaskIndex].completedTask == true)
            {
                currentQuestTaskIndex++;
                if (currentQuestTaskIndex >= questTasks.Count)
                {
                    completedQuest = true;
                    QuestEvents.QuestCompleted(this);
                    return;
                }
            }
        }
    }

    /* --------------------------------------------------------------
    Quest Events represent the nodes in the quest graph datastructure
    ----------------------------------------------------------------- */
    [System.Serializable]
    public class QuestTask
    {
        //==========================Variables===================//
        public Quest parentQuest;
        [HideInInspector] public bool completedTask;
        public string description;  //Description of the task
        public TaskType taskType;

        public List<string> taskDialog;

        //=========================Functions====================//

        //----------------------
        public void UpdateTask()
        {
            taskType.current++;
            if (taskType.current >= taskType.numberOfTimes)
            {
                completedTask = true;                
            }            
        }

        //--------------------------------------------------------------------
        public void DisplayTaskDescription(TextMeshProUGUI sceneQuestInfoText)
        {
            if (taskType.numberOfTimes > 0)
            {
                sceneQuestInfoText.text = description + " " + taskType.current + "/" + taskType.numberOfTimes;
            }
        }
    }

    [System.Serializable]
    public class TaskType
    {
        public enum Types { KILL, DEFEND, GATHER, RETURN_TO_QUESTGIVER }
        public Types type;

        [HideInInspector] public int current = 0;
        public int numberOfTimes;

        public KillTargets killTargets = new KillTargets();
        public GatherTargets gatherTargets = new GatherTargets();
    }

    [System.Serializable]
    public class KillTargets
    {
        public BattleCharacters npcType;
    }
       
    [System.Serializable]
    public class GatherTargets
    {
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
        public int questID = -1;
    }
}