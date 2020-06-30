using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.QuestSystem;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI proceedButtonText;

    public List<string> questDialog;
    public bool questInitiationDialog;
    public int currentDialogIndex;

    public GameObject backButton;
    public int dialogInitiatingNPCID;

    //------------------
    private void Start()
    {
        QuestEvents.StartDialogue += StartDialogue;
    }

    //----------------------------------------------------------------------------------------
    public void StartDialogue(List<string> questDialog, bool questInitiationDialog, int npcID)
    {
        this.questDialog = questDialog;
        this.questInitiationDialog = questInitiationDialog;
        dialogInitiatingNPCID = npcID;

        dialoguePanel.SetActive(true);
        dialogueText.text = questDialog[0];
        if (currentDialogIndex == questDialog.Count - 1)
        {
            if (questInitiationDialog)
            {
                proceedButtonText.text = "Accept";
            }
            else
            {
                proceedButtonText.text = "Close";
            }
        }
    }

    //----------------------------
    public void ShowNextDialogue()
    {
        currentDialogIndex++;
        
        if(currentDialogIndex > 0)
        {
            backButton.SetActive(true);
        }

        if (currentDialogIndex == questDialog.Count - 1)
        {
            if (questInitiationDialog)
            {
                proceedButtonText.text = "Accept";
            }
            else
            {
                proceedButtonText.text = "Close";
            }
        }
       
        if (currentDialogIndex >= questDialog.Count)
        {
            ExitDialogue();            
        }
        else
        {
            dialogueText.text = questDialog[currentDialogIndex];
        }
    }

    //--------------------------------
    public void ShowPreviousDialogue()
    {
        currentDialogIndex--;
        currentDialogIndex = Mathf.Clamp(currentDialogIndex, 0, questDialog.Count - 1);

        if (currentDialogIndex == 0)
        {
            backButton.SetActive(false);
        }

        if (currentDialogIndex < questDialog.Count - 1)
        {
            proceedButtonText.text = "Next";
        }
        if (currentDialogIndex >= 0)
        {
            dialogueText.text = questDialog[currentDialogIndex];
        }
    }

    //------------------------
    public void ExitDialogue()
    {
        currentDialogIndex = 0;
        dialoguePanel.SetActive(false);
        backButton.SetActive(false);
        proceedButtonText.text = "Next";

        QuestEvents.InteractionFinished();

        //If dialog not part of quest initiation i.e. part of a quest task
        if(!questInitiationDialog)
        {
            List<Quest> playerQuests = GameManager.Instance.GetPlayerQuests();
            for (int i = 0; i < playerQuests.Count; i++)
            {
                Quest quest = playerQuests[i];
                //Get current task requirement for every quest
                TaskType currentQuestTaskType = quest.questTasks[quest.currentQuestTaskIndex].taskType;

                //If item type matches quest task requirement update task
                /* TODO : only a single quest is updated */
                if (currentQuestTaskType.type == TaskType.Types.TALK_TO_NPC)
                {
                    quest.UpdateQuest();
                    break;                    
                }
            }
        }
        else
        {
            QuestEvents.QuestAccepted(dialogInitiatingNPCID);
        }
    }

    //----------------------
    private void OnDisable()
    {
        QuestEvents.StartDialogue -= StartDialogue;
    }
}
