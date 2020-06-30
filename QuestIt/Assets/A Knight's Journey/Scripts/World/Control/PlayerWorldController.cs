using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.NPCs;
using System;

namespace RPG.Control
{
    public class PlayerWorldController : MonoBehaviour
    {
        //============================Variables=====================//

        WorldMovement worldMovement;
        FollowNPC follow;
        [SerializeField] EventSystem eventSystem;
        public PlayerInfo playerInfo;

        public enum CursorType { None, Duel, Dialogue }
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] List<CursorMapping> cursorMappings;

        //============================Functions=====================//

        //------------------
        private void Start()
        {
            worldMovement = GetComponent<WorldMovement>();
            follow = GetComponent<FollowNPC>();                         
            Input.multiTouchEnabled = false;
            playerInfo.IsPlayer = true;
            playerInfo.IsTeamRed = true;

            transform.position = GameManager.Instance.GetPlayerSpawnPosition();
            //Debug.LogError(transform.position);
        }

        //-------------------
        private void Update()
        {
            if(InteractWithUI()) { return; }

            //Action priority to prioritize NPC interaction over movement
            if (InteractWithNPC()) { return; }
            if (GoToNPC()) { return; }
            MoveToCursor();
            SetCursor();
        }        

        //--------------------------
        public bool InteractWithUI()
        {
            if(eventSystem.IsPointerOverGameObject())
            {
                return true;
            }
            return false;
        }

        //Moves to the point defined by the mouse cursor position in world space
        //-------------------------
        private void MoveToCursor()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(GetRayFromInput(Input.mousePosition), out hit))
                {
                    worldMovement.StartMoveAction(hit.point);
                }
            }

            else if (Input.touchCount > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(GetRayFromInput(Input.GetTouch(0).position), out hit))
                {
                    worldMovement.StartMoveAction(hit.point);
                }
            }           
        }        

        //------------
        bool GoToNPC()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRayFromInput(Input.mousePosition));

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.GetComponent<NPC>())
                {
                    if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                    {
                        worldMovement.StartMoveAction(hits[i].point);
                        follow.SetTargetTransform(hits[i].transform);
                    }
                    SetCursor(hits[i].transform.GetComponent<NPC>());
                    return true;
                }
            }         
            return false;
        }

        //--------------------
        bool InteractWithNPC()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRayFromInput(Input.mousePosition));

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.GetComponent<NPCInteractionIcon>())
                {
                    if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                    {
                        hits[i].transform.GetComponent<NPCInteractionIcon>().Interact();
                    }
                    SetCursor(hits[i].transform.GetComponent<NPCInteractionIcon>().GetParentNPC());
                    return true;
                }
            }
            return false;
        }

        //------------------------------------
        private void SetCursor(NPC npc = null)
        {
            CursorMapping cursorMapping = GetCursorMapping(CursorType.None);
            if (npc != null)
            {
                if (npc.npcType == NPC.NPCType.Duel)
                {
                    cursorMapping = GetCursorMapping(CursorType.Duel);
                }
                else if (npc.npcType == NPC.NPCType.QuestGiver)
                {
                    cursorMapping = GetCursorMapping(CursorType.Dialogue);
                }
            }
            
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        //-----------------------------------------------------
        private CursorMapping GetCursorMapping(CursorType type)
        {
            for (int i = 0; i < cursorMappings.Count; i++)
            {
                if(cursorMappings[i].cursorType == type)
                {
                    return cursorMappings[i];
                }
            }
            return cursorMappings[0];
        }

        //--------------------------------------------------
        private static Ray GetRayFromInput(Vector3 position)
        {
            return Camera.main.ScreenPointToRay(position);
        }    
    }
}

