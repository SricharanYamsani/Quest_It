using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.QuestSystem;

namespace RPG.NPCs
{
    public class NPC : MonoBehaviour
    {  
        //=========================Variables=====================//

        public enum NPCType { Vendor, Duel, Quest };
        public NPCType npcType;
        public NPCWorldController worldController;
        public Transform playerTransform;
        public QuestLog questLog;
        public float interactionRange;
                            
        //=========================Functions=====================//
        //-------------------------
        public virtual void Start()
        {
            worldController = GetComponent<NPCWorldController>();
            playerTransform = FindObjectOfType<PlayerWorldController>().transform;           
        }

        //------------------
        public void Update()
        {
            if(Vector3.Distance(transform.position, playerTransform.position) < interactionRange)
            {
                InteractWithPlayer();
            }
        }

        public virtual void CreateQuest() { }
        public virtual void InteractWithPlayer() { } 
    }
}
