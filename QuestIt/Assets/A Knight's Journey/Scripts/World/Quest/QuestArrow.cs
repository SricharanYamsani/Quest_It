using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using RPG.NPCs;
using RPG.QuestSystem;

public class QuestArrow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] NPC target;
    [SerializeField] float yPos;
    [SerializeField] float distanceFromCamera;
    [SerializeField] TextMeshProUGUI distanceText;

    public List<NPC> targetNPCs = new List<NPC>();

    //---------------------
    private void OnEnable()
    {
        QuestEvents.AddNPCLocation += AddTargetNPC;
        QuestEvents.TaskUpdated += UpdateTargetNPCList;
        distanceText.gameObject.SetActive(true);
    }

    //-----------
    void Update()
    {
        Vector3 resultingPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        resultingPosition.y = yPos;
        transform.position = resultingPosition;

        transform.LookAt(target.transform);

        Vector3 distanceTextPos = Camera.main.WorldToScreenPoint(this.transform.position);
        distanceText.transform.position = distanceTextPos;
        distanceText.text = ((int)Vector3.Distance(playerTransform.position, target.transform.position)).ToString();
    }

    //------------------------------
    void AddTargetNPC(NPC targetNPC)
    {
        NPC targetNPCExists = targetNPCs.Find((NPC npc) => npc == targetNPC);
        if (targetNPCExists == null)
        {
            targetNPCs.Add(targetNPC);
            SetNewTarget();
        }
    }

    //-------------------------
    private void SetNewTarget()
    {
        if (targetNPCs.Count > 0)
        {
            targetNPCs = targetNPCs.OrderBy(x => Vector3.Distance(playerTransform.position, x.transform.position)).ToList();
            target = targetNPCs[0];
        }
    }

    //------------------------
    void UpdateTargetNPCList()
    {
        for (int i = targetNPCs.Count - 1; i >= 0; i--)
        {
            if(targetNPCs[i] == target)
            {
                targetNPCs.RemoveAt(i);
            }
        }

        SetNewTarget();
    }

    //----------------------
    private void OnDisable()
    {
        targetNPCs.Clear();                                                             
        QuestEvents.AddNPCLocation -= AddTargetNPC;
        QuestEvents.TaskUpdated -= UpdateTargetNPCList;
        distanceText.gameObject.SetActive(false);
    }
}
