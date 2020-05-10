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
        public PlayerInfo npcInfo;
        
        public enum NPCType { Vendor, Duel, QuestGiver, QuestItem };
        public NPCType npcType;
        
        public PlayerWorldController player;
        public QuestLog questLog;
        public float interactionRange;
        public GameObject interactionIcon;
                                                   
        //=========================Functions=====================//
        //-------------------------
        public virtual void Start()
        {
            player = FindObjectOfType<PlayerWorldController>();      
        }
                
        public virtual void CreateQuest() { }
        public virtual void OnPlayerInteraction() { } 
    }
}
