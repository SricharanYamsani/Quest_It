using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.NPCs
{
    public class NPCInteractionIcon : MonoBehaviour
    {
        [SerializeField] NPC parentNPC;

        //--------------------
        public void Interact()
        {
            parentNPC.OnPlayerInteraction();
        }

        //-----------------------
        public NPC GetParentNPC()
        {
            return parentNPC;
        }
    }
}