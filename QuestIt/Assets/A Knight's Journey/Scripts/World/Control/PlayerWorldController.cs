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
            if (InteractWithNPC()) { return; }
            if (GoToNPC()) { return; }
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
                    if (Physics.Raycast(GetRayFromInput(Input.mousePosition), out hit))
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
                    if (Physics.Raycast(GetRayFromInput(Input.GetTouch(0).position), out hit))
                    {
                        worldMovement.StartMoveAction(hit.point);
                    }
                }
            }
        }

        //------------
        bool GoToNPC()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit[] hits = Physics.RaycastAll(GetRayFromInput(Input.mousePosition));

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
                    if (Physics.Raycast(GetRayFromInput(Input.GetTouch(0).position), out hit))
                    {
                        worldMovement.StartMoveAction(hit.point);
                    }
                }
            }

            return false;
        }

        //--------------------
        bool InteractWithNPC()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit[] hits = Physics.RaycastAll(GetRayFromInput(Input.mousePosition));

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.GetComponent<NPCInteractionIcon>())
                        {
                            hits[i].transform.GetComponent<NPCInteractionIcon>().Interact();
                            return true;
                        }
                    }
                }
            }
            else if(Input.touchCount > 0)
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    RaycastHit[] hits = Physics.RaycastAll(GetRayFromInput(Input.GetTouch(0).position));

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.GetComponent<NPCInteractionIcon>())
                        {
                            hits[i].transform.GetComponent<NPCInteractionIcon>().Interact();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //--------------------------------------------------
        private static Ray GetRayFromInput(Vector3 position)
        {
            return Camera.main.ScreenPointToRay(position);
        }    
    }
}

