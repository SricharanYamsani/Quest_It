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
        public GameObject questArrow;

        //Quest information that is shown when the quest log is opened
        public GameObject questLogQuestInfo;

        public GameObject questInfo;
        public TextMeshProUGUI questInfoText;
        public GameObject questCompletedInfo;
        public TextMeshProUGUI questCompletedInfoText;

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
            if (questCompletedInfoText.canvasRenderer.GetAlpha() == 0 && questCompletedInfo.activeSelf)
            {
                questCompletedInfo.SetActive(false);
            }
        }

        //---------------------------
        private void QuestCompleted()
        {
            QuestEvents.QuestCompleted();
            RemoveQuest();
            //Track a new quest if available
            if (quests.Count > 0)
            {
                activeQuest = quests.Find((Quest quest) => quest.questType.type == QuestType.Type.MAIN);
                if (activeQuest == null)
                {
                    activeQuest = quests[UnityEngine.Random.Range(0, quests.Count)];
                }

                if (activeQuest != null)
                {
                    ToggleQuestActive(activeQuest.id);
                    return;
                }
            }            
        }

        //------------------------
        private void RemoveQuest()
        {
            questArrow.SetActive(false);
            questInfoText.text = "";
            taskDescText.text = "";
            DisplayQuestCompleted("Quest Completed");
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
                    GetComponent<QuestTrackButton>().questID == activeQuest.id)
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
            activeQuest.TrackNextTask(taskDescText);
        }

        //-----------------------------------
        public void DisplayQuest(string info)
        {
            questInfo.SetActive(true);
            questInfoText.text = info;            
        }

        //--------------------------------------------
        public void DisplayQuestCompleted(string info)
        {
            questCompletedInfo.SetActive(true);
            questCompletedInfoText.canvasRenderer.SetAlpha(1.0f);
            questCompletedInfoText.CrossFadeAlpha(0, 10f, false);
            questCompletedInfoText.text = info;
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
            questInfoPanelInstance.GetComponentInChildren<Button>().onClick.AddListener( () => ToggleQuestActive(quest.id));
            questInfoPanelInstance.GetComponentInChildren<Button>().GetComponent<QuestTrackButton>().questID = quest.id;
            questInfoPanelInstance.GetComponentInChildren<TextMeshProUGUI>().text = quest.questDesc;
            questInfoPanelInstance.SetActive(true);
            questInfoPanelInstances.Add(questInfoPanelInstance);

            activeQuest = quest;
            ToggleQuestActive(activeQuest.id);
        }

        //--------------------------
        public void ToggleQuestLog()
        {
            toggle = !toggle;
            questPanel.SetActive(toggle);
        }

        //------------------------------------------
        private void UpdateQuestPanel(Button button)
        {
            for (int i = 0; i < questInfoPanelInstances.Count; i++)
            {
                if (questInfoPanelInstances[i].GetComponentInChildren<Button>() != button)
                {
                    questInfoPanelInstances[i].GetComponentInChildren<Button>().
                        GetComponentInChildren<TextMeshProUGUI>().text = "Track";
                    questInfoPanelInstances[i].GetComponentInChildren<Button>().
                        GetComponent<QuestTrackButton>().track = false;
                }                
            }            
        }

        //--------------------------------------
        public void ToggleQuestActive(byte[] id)
        {
            questArrow.SetActive(false);
            for (int i = 0; i < questInfoPanelInstances.Count; i++)
            {
                //Get button in the list that matches 'this' button
                if (questInfoPanelInstances[i].GetComponentInChildren<Button>().
                    GetComponent<QuestTrackButton>().questID == id)
                {
                    Button button = questInfoPanelInstances[i].GetComponentInChildren<Button>();
                    button.GetComponent<QuestTrackButton>().track = !button.GetComponent<QuestTrackButton>().track;

                    //toggle
                    UpdateQuestPanel(button);
                    if (button.GetComponent<QuestTrackButton>().track)
                    {
                        button.GetComponentInChildren<TextMeshProUGUI>().text = "Untrack";
                        activeQuest = quests.Find((Quest quest) => quest.id == id);
                        questArrow.SetActive(true);
                        DisplayQuest(activeQuest.questDesc);
                        TrackNextTask();
                        break;
                    }
                    else
                    {
                        button.GetComponentInChildren<TextMeshProUGUI>().text = "Track";
                        activeQuest = null;
                        questInfoText.text = "";
                        taskDescText.text = "";                        
                        QuestEvents.UntrackTask();
                        break;
                    }
                }
            }            
        }

        //---------------------
        public void OnDisable()
        {
            QuestEvents.TaskUpdated -= TrackNextTask;
        }
    }
}