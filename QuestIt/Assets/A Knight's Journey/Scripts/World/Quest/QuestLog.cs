using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

namespace RPG.QuestSystem
{
    /* -------------------------------------------------
    Quest Manager is responsible for creating new quests
    and keeping track of all active quests 
    ---------------------------------------------------- */

    public class QuestLog : MonoBehaviour
    {
        //====================Variables==============//
        [Header("Quests")]
        [SerializeField] Quest currentTrackedQuest;
        public GameObject questPanel;
        public GameObject questArrow;

        //Quest information that is shown when the quest log is opened
        public GameObject questLogQuestInfo;

        public GameObject questInfo;
        public TextMeshProUGUI questInfoText;
        public GameObject questCompletedInfo;
        public TextMeshProUGUI questCompletedInfoText;
        public TextMeshProUGUI questNameText;

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
            QuestEvents.QuestCompleted += DisplayQuestCompleted;

            ReBuildQuestLog();

            //Keep tracking the quest that was tracked by the player
            currentTrackedQuest = GameManager.Instance.GetCurrentTrackedQuest();
            if(currentTrackedQuest != null)
            {
                if (!currentTrackedQuest.completedQuest)
                {
                    ToggleQuestActive(currentTrackedQuest.id);
                }
                else
                {
                    currentTrackedQuest = null;
                }
            }            
        }

        //----------------------------
        private void ReBuildQuestLog()
        {
            //If returning from a battle
            //Re-Build quest log with active quests
            List<Quest> playerQuests = GameManager.Instance.GetPlayerQuests();
            for (int i = 0; i < playerQuests.Count; i++)
            {
                SetupQuestInQuestLog(playerQuests[i]);
            }

            StartCoroutine(DisplayCompletedQuests());
        }

        //----------------------------------
        IEnumerator DisplayCompletedQuests()
        {
            List<Quest> completedQuests = GameManager.Instance.GetCompletedQuests();
            //Display completed quests
            for (int i = 0; i < completedQuests.Count; i++)
            {
                if (!completedQuests[i].questCompletionDisplayed)
                {
                    DisplayQuestCompleted(completedQuests[i]);
                    yield return new WaitForSeconds(5f);
                }
            }
        }

        //------------------
        public void Update()
        {
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

        //--------------------------------------------
        public void DisplayQuestCompleted(Quest quest)
        {
            quest.questCompletionDisplayed = true;

            questArrow.SetActive(false);
            questInfoText.text = "";
            taskDescText.text = "";

            questCompletedInfo.SetActive(true);
            questCompletedInfoText.canvasRenderer.SetAlpha(1.0f);
            questCompletedInfoText.CrossFadeAlpha(0, 5f, false);
            questCompletedInfoText.text = "Quest Completed";

            questNameText.text = quest.questDesc;
            questNameText.canvasRenderer.SetAlpha(1.0f);
            questNameText.CrossFadeAlpha(0, 5f, false);

            for (int i = 0; i < questInfoPanelInstances.Count; i++)
            {
                GameObject questInfoPanelInstance = questInfoPanelInstances[i];
                if (questInfoPanelInstance.GetComponentInChildren<Button>().
                    GetComponent<QuestTrackButton>().questID == currentTrackedQuest.id)
                {
                    Destroy(questInfoPanelInstance);
                    questInfoPanelInstances.RemoveAt(i);
                }
            }

            currentTrackedQuest = null;
        }

        //-------------------------
        public void TrackNextTask()
        {
            currentTrackedQuest.TrackNextTask(taskDescText);
        }

        //-----------------------------------
        public void DisplayQuest(string info)
        {
            questInfo.SetActive(true);
            questInfoText.text = info;
        }          

        //-------------------------------------
        public void SetupQuestInQuestLog(Quest quest)
        {
            GameObject questInfoPanelInstance = Instantiate(questLogQuestInfo, questPanel.transform);
            questInfoPanelInstance.GetComponentInChildren<Button>().onClick.AddListener(() => ToggleQuestActive(quest.id));
            questInfoPanelInstance.GetComponentInChildren<Button>().GetComponent<QuestTrackButton>().questID = quest.id;
            questInfoPanelInstance.GetComponentInChildren<TextMeshProUGUI>().text = quest.questDesc;
            questInfoPanelInstance.SetActive(true);
            questInfoPanelInstances.Add(questInfoPanelInstance);                   
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

        //-----------------------------------
        public void ToggleQuestActive(int id)
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
                        currentTrackedQuest = GameManager.Instance.GetPlayerQuests().Find((Quest quest) => quest.id == id);
                        questArrow.SetActive(true);
                        DisplayQuest(currentTrackedQuest.questDesc);
                        TrackNextTask();
                        break;
                    }
                    else
                    {
                        button.GetComponentInChildren<TextMeshProUGUI>().text = "Track";
                        currentTrackedQuest = null;
                        questInfoText.text = "";
                        taskDescText.text = "";
                        break;
                    }
                }
            }
        }

        //-----------------------------------
        public Quest GetCurrentTrackedQuest()
        {
            return currentTrackedQuest;
        }

        //---------------------
        public void OnDisable()
        {
            QuestEvents.TaskUpdated -= TrackNextTask;
            QuestEvents.QuestCompleted -= DisplayQuestCompleted;
        }
    }
}