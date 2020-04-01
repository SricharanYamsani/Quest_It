using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.NPCs;

namespace RPG.Control
{
    public class PlayerWorldController : MonoBehaviour
    {
        //============================Variables=====================//

        WorldMovement worldMovement;
        FollowNPC follow;
        [SerializeField] EventSystem eventSystem;
        public PlayerInfo playerInfo;

        //============================Functions=====================//

        //------------------
        private void Start()
        {
            worldMovement = GetComponent<WorldMovement>();
            follow = GetComponent<FollowNPC>();
            Input.multiTouchEnabled = false;
            playerInfo.IsPlayer = true;
            playerInfo.IsTeamRed = true;
        }

        //-------------------
        private void Update()  
        {
            //Action priority to prioritize NPC interaction over movement
            if(InteractWithNPC()) { return; }
            MoveToCursor();
        }

        //Moves to the point defined by the mouse cursor position in world space
        //-------------------------
        private void MoveToCursor()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit hit;
                    if (Physics.Raycast(GetMouseRay(), out hit))
                    {
                        worldMovement.StartMoveAction(hit.point);
                    }
                }
            }

            else if(Input.touchCount > 0)
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit hit;
                    if (Physics.Raycast(GetMouseRay(), out hit))
                    {
                        worldMovement.StartMoveAction(hit.point);
                    }
                }
            }
        }

        //--------------------
        bool InteractWithNPC()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.GetComponent<NPC>())
                        {
                            worldMovement.StartMoveAction(hits[i].point);
                            follow.SetTargetTransform(hits[i].transform);
                            return true;
                        }
                    }
                }
            }

            else if (Input.touchCount > 0)
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit hit;
                    if (Physics.Raycast(GetMouseRay(), out hit))
                    {
                        worldMovement.StartMoveAction(hit.point);
                    }
                }
            }

            return false;
        }

        //------------------------------
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }    
    }
}

