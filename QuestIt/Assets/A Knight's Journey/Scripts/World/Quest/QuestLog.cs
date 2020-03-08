using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace RPG.QuestSystem
{
    /* -------------------------------------------------
    Quest Manager is responsible for creating new quests
    and keeping track of all active quests 
    ---------------------------------------------------- */

    public class QuestLog : MonoBehaviour
    {
        //====================Variables==============//
        Quest activeQuest;
        [Header("Quests")]
        public List<Quest> quests = new List<Quest>();
        public GameObject questPanel;

        //Quest information that is shown when the quest log is opened
        public GameObject questLogQuestInfo;

        public GameObject questUpdatedInfo;
        public TextMeshProUGUI questUpdatedInfoText;

        //Quest information that is shown to the player in the scene
        //when the quest gets updated i.e. next task description, quest completed etc.
        public TextMeshProUGUI taskDescText;

        public List<GameObject> questInfoPanelInstances;
        public bool toggle;

        //====================Functions==============//

        //-----------------
        public void Start()
        {
            QuestEvents.TaskUpdated += TrackNextTask;
        }

        //------------------
        public void Update()
        {
            if (activeQuest != null && activeQuest.completedQuest == true)
            {
                QuestCompleted();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleQuestLog();
            }

            //UI related
            if (questUpdatedInfoText.canvasRenderer.GetAlpha() == 0 && questUpdatedInfo.activeSelf)
            {
                questUpdatedInfo.SetActive(false);
            }
        }

        //---------------------------
        private void QuestCompleted()
        {
            RemoveQuest();
            //Track a new quest if available
            if (quests.Count > 0)
            {
                activeQuest = quests.Find((Quest quest) => quest.questType.type == QuestType.Type.MAIN);
                if (activeQuest == null)
                {
                    activeQuest = quests[UnityEngine.Random.Range(0, quests.Count)];
                }
                TrackNextTask();
            }
        }

        //------------------------
        private void RemoveQuest()
        {
            taskDescText.text = "";
            DisplayQuestUpdateInfo("Quest Completed : " + activeQuest.questDesc);
            for (int i = 0; i < quests.Count; i++)
            {
                if (activeQuest.id == quests[i].id)
                {
                    quests.RemoveAt(i);
                }
            }

            for (int i = 0; i < questInfoPanelInstances.Count; i++)
            {
                GameObject questInfoPanelInstance = questInfoPanelInstances[i];
                if (questInfoPanelInstance.GetComponentInChildren<Button>().
                    GetComponent<QuestButtonID>().questID == activeQuest.id)
                {
                    Destroy(questInfoPanelInstance);
                    questInfoPanelInstances.RemoveAt(i);
                }
            }

            activeQuest = null;
        }

        //-------------------------
        public void TrackNextTask()
        {
            if (activeQuest != null)
            {
                activeQuest.TrackNextTask(taskDescText);
            }
        } 

        //---------------------------------------------
        public void DisplayQuestUpdateInfo(string info)
        {
            questUpdatedInfo.SetActive(true);
            questUpdatedInfoText.canvasRenderer.SetAlpha(1.0f);
            questUpdatedInfoText.text = info;
            questUpdatedInfoText.CrossFadeAlpha(0, 10f, false);
        }

        //-------------------------------
        public void AddQuest(Quest quest)
        {
            if(activeQuest == null)
            {
                activeQuest = quest;
            }
            quest.id = Guid.NewGuid().ToByteArray();
            quests.Add(quest);
            GameObject questInfoPanelInstance = Instantiate(questLogQuestInfo, questPanel.transform);
            questInfoPanelInstance.GetComponentInChildren<Button>().onClick.AddListener( () => SetActiveQuest(quest.id));
            questInfoPanelInstance.GetComponentInChildren<Button>().GetComponent<QuestButtonID>().questID = quest.id;
            questInfoPanelInstance.GetComponentInChildren<TextMeshProUGUI>().text = "Quest : " + quest.questDesc;
            questInfoPanelInstance.SetActive(true);
            questInfoPanelInstances.Add(questInfoPanelInstance);

            activeQuest = quest;
            TrackNextTask();

            //TODO add functionality to support quest updation for quests that are not being tracked but are in the log
        }

        //--------------------------
        public void ToggleQuestLog()
        {
            toggle = !toggle;
            questPanel.SetActive(toggle);
        }

        //-----------------------------------
        public void SetActiveQuest(byte[] id)
        {
            activeQuest = quests.Find((Quest quest) => quest.id == id);
            DisplayQuestUpdateInfo("Quest : " + activeQuest.questDesc);
            TrackNextTask();   
        }

        //---------------------
        public void OnDisable()
        {
            QuestEvents.TaskUpdated -= TrackNextTask;
        }
    }
}