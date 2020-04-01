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
        public PlayerInfo playerInfo;


        public enum NPCType { Vendor, Duel, Quest };
        public NPCType npcType;
        public NPCWorldController worldController;
        public PlayerWorldController player;
        public QuestLog questLog;
        public float interactionRange;

        public bool completedInteraction;
                            
        //=========================Functions=====================//
        //-------------------------
        public virtual void Start()
        {
            worldController = GetComponent<NPCWorldController>();
            player = FindObjectOfType<PlayerWorldController>();      
        }

        //------------------
        public void Update()
        {
            if(Vector3.Distance(transform.position, player.transform.position) < interactionRange && !completedInteraction)
            {
                InteractWithPlayer();
            }
        }

        public virtual void CreateQuest() { }
        public virtual void InteractWithPlayer() { } 
    }
}
