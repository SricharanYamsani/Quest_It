using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.NPCs
{
    public class NPC : MonoBehaviour
    {  
        //=========================Variables=====================//

        public enum NPCType { Vendor, Duel, Quest};
        public NPCType npcType;
        public NPCWorldController worldController;
        public Transform playerTransform;

        //=========================Functions=====================//
        //------------------
        private void Start()
        {
            worldController = GetComponent<NPCWorldController>();
            playerTransform = FindObjectOfType<PlayerWorldController>().transform;
        }

        public virtual void InteractWithPlayer() { }
    }
}
