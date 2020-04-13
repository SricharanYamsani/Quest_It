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

    Quest availableQuest;

    //------------------
    private void Start()
    {
        QuestEvents.StartDialogue += StartDialogue;
    }

    //------------------------------------
    public void StartDialogue(Quest quest)
    {
        availableQuest = quest;
        dialoguePanel.SetActive(true);
        dialogueText.text = availableQuest.questDialogue[0];
    }

    //----------------------------
    public void ShowNextDialogue()
    {
        availableQuest.currentDialogueIndex++;
        if (availableQuest.currentDialogueIndex == availableQuest.questDialogue.Count - 1)
        {
            proceedButtonText.text = "Accept";
        }
        if (availableQuest.currentDialogueIndex >= availableQuest.questDialogue.Count)
        {
            QuestEvents.QuestAccepted();
            ExitDialogue();
        }
        else
        {
            dialogueText.text = availableQuest.questDialogue[availableQuest.currentDialogueIndex];
        }
    }

    //--------------------------------
    public void ShowPreviousDialogue()
    {
        availableQuest.currentDialogueIndex--;
        availableQuest.currentDialogueIndex = Mathf.Clamp(availableQuest.currentDialogueIndex, 
                                0, availableQuest.questDialogue.Count - 1);
        
        if(availableQuest.currentDialogueIndex < availableQuest.questDialogue.Count - 1)
        {
            proceedButtonText.text = "Next";
        }
        
        if (availableQuest.currentDialogueIndex >= 0)
        {
            dialogueText.text = availableQuest.questDialogue[availableQuest.currentDialogueIndex];
        }
    }

    //------------------------
    public void ExitDialogue()
    {
        availableQuest.currentDialogueIndex = 0;
        dialoguePanel.SetActive(false);
        proceedButtonText.text = "Next";

        QuestEvents.InteractionFinished();
    }

    //----------------------
    private void OnDisable()
    {
        QuestEvents.StartDialogue -= StartDialogue;
    }
}
