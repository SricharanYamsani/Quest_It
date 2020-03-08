using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using DG.Tweening;

namespace RPG.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineBrain primaryCamera; //Main CinemachineBrain
        public CinemachineVirtualCamera primaryComposerVirtualCamera; //Primary Virtual Camera of composer type
        public CinemachineVirtualCamera primaryGroupComposerVirtualCamera; //Primary Virtual Camera of group composer type
        private CinemachineComposer primaryCameraComposer; // Composer of primary virtual camera
        private CinemachineGroupComposer primaryGroupComposer; //Group composer of primary virtual camera
        private VirtualCameraComponent defaultVirtualCamera; // Default Virtual Camera comopnent
        [SerializeField]private CinemachineTargetGroup targetGroup;
        //[SerializeField] private Transform lookAt;

        Coroutine cameraControlCoroutine, cameraControlSubCoroutine; // Coroutines to handle camera movement

        private VirtualCameraType cameraType; // Virtual camera type

        #region V2
        public CinemachineVirtualCamera V1Cam;
        public CinemachineVirtualCamera V2Cam;
        public Transform redLookAt;
        public Transform blueLookAt;
        public Transform camParent;

        public Dictionary<int, Transform> targetDict;
        public Dictionary<int, float> yAngles;
        List<int> ids;
        public List<int> redTeam;
        public List<int> blueTeam;

        public PlayableDirector director;

        #endregion


        private void Awake()
        {
            if(primaryComposerVirtualCamera != null)
            {
                primaryCameraComposer = primaryComposerVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
            
                if(primaryCameraComposer != null)
                {
                    defaultVirtualCamera = new VirtualCameraComponent(primaryComposerVirtualCamera.transform, primaryComposerVirtualCamera.m_Lens.FieldOfView, primaryCameraComposer.m_ScreenX, primaryCameraComposer.m_ScreenY);
                }
                else
                {
                    defaultVirtualCamera = new VirtualCameraComponent(primaryComposerVirtualCamera.transform, primaryComposerVirtualCamera.m_Lens.FieldOfView);
                }
            }

            if(primaryGroupComposerVirtualCamera != null)
            {
                primaryGroupComposer = primaryGroupComposerVirtualCamera.GetCinemachineComponent<CinemachineGroupComposer>();
            }


            targetDict = new Dictionary<int, Transform>();
            yAngles = new Dictionary<int, float>();
            redTeam = new List<int>();
            blueTeam = new List<int>();
            ids = new List<int>();
        }

        /// <summary>
        /// Switch virtual camera type
        /// </summary>
        /// <param name="type">The type of camera to switch</param>
        public void SwitchCameraType(VirtualCameraType type)
        {
            if (cameraType == type)
            {
                return;
            }

            cameraType = type;
            if (primaryComposerVirtualCamera != null && primaryGroupComposerVirtualCamera != null)
            {
                if (type == VirtualCameraType.COMPOSER)
                {
                    primaryComposerVirtualCamera.gameObject.SetActive(true);
                    primaryGroupComposerVirtualCamera.gameObject.SetActive(false);
                }
                else if (type == VirtualCameraType.GROUPCOMPOSER)
                {
                    Debug.Log("Here");
                    primaryComposerVirtualCamera.gameObject.SetActive(false);
                    primaryGroupComposerVirtualCamera.gameObject.SetActive(true);
                    //lookAt.position = targetGroup.transform.position;
                    //lookAt.rotation = targetGroup.transform.rotation;
                }
            }
        }

        /// <summary>
        /// Reset camera to default camera position
        /// </summary>
        public void ResetCameraToDefault()
        {
            if (cameraControlCoroutine != null)
            {
                StopCoroutine(cameraControlCoroutine);
            }
            if (cameraControlSubCoroutine != null)
            {
                StopCoroutine(cameraControlSubCoroutine);
            }

            SetMainVirtualCameraProperties(defaultVirtualCamera);
        }

        /// <summary>
        /// Starts a camera motion
        /// </summary>
        public void StartCameraMotion(List<KeyValuePair<VirtualCameraComponent, float>> virtualCameraClips, CameraControlType cameraControlType, bool useTimeScale = false)
        {
            if (virtualCameraClips == null)
            {
#if DEBUG
                Debug.LogError("List of Virtual Camera Clips is Null");
#endif
                return;
            }
        

            int length = virtualCameraClips.Count;

            if (length == 0)
            {
#if DEBUG
                Debug.LogError("List of Virtual Camera Clips is Empty");
#endif
                return;
            }
        
            if(cameraControlType.Equals(CameraControlType.NONE))
            {
#if DEBUG
                Debug.LogError("Undefined Camera Control Type");
#endif
                return;
            }
            else
            {
                if(cameraControlCoroutine != null)
                {
#if DEBUG
                    Debug.Log("Camera Control Coroutine overriden");
#endif
                    StopCoroutine(cameraControlCoroutine);
                }
                if(cameraControlSubCoroutine != null)
                {
                    StopCoroutine(cameraControlSubCoroutine);
                }
            }

            switch (cameraControlType)
            {
                case CameraControlType.CUT:
                    cameraControlCoroutine = StartCoroutine(StartCutCameraMotion(virtualCameraClips,useTimeScale));
                    break;
                case CameraControlType.BLEND:
                    cameraControlCoroutine = StartCoroutine(StartBlendCameraMotion(virtualCameraClips,useTimeScale));
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Cuts through all the cameras in the list 
        /// </summary>
        /// <param name="virtualCameraClips"></param>
        /// <param name="useTimeScale"></param>
        /// <returns></returns>
        IEnumerator StartCutCameraMotion(List<KeyValuePair<VirtualCameraComponent,float>> virtualCameraClips, bool useTimeScale)
        {
            int length = virtualCameraClips.Count;
            for (int i = 0; i < length; i++)
            {
                SetMainVirtualCameraProperties(virtualCameraClips[i].Key);
                yield return new WaitForSeconds(virtualCameraClips[i].Value / (useTimeScale ? Time.timeScale : 1f));
            }

#if DEBUG
            Debug.Log("(Cut Type) Camera control coroutine completed");
#endif
        }

        /// <summary>
        /// Blends through all the cameras in the list
        /// </summary>
        /// <param name="virtualCameraClips"></param>
        /// <param name="useTimeScale"></param>
        /// <returns></returns>
        IEnumerator StartBlendCameraMotion(List<KeyValuePair<VirtualCameraComponent, float>> virtualCameraClips, bool useTimeScale)
        {
            int length = virtualCameraClips.Count;
            SetMainVirtualCameraProperties(virtualCameraClips[0].Key);
            for (int i = 0; i < length - 1; i++)
            {
                float endTime = virtualCameraClips[i].Value;
                cameraControlSubCoroutine = StartCoroutine(SetMainVirtualCameraProperties(virtualCameraClips[i].Key, virtualCameraClips[i + 1].Key, endTime, useTimeScale));
                yield return cameraControlSubCoroutine;
                cameraControlSubCoroutine = null;
            }

#if DEBUG
            Debug.Log("(Cut Type) Camera control coroutine completed");
#endif
        }

        /// <summary>
        /// Sets the values of the primary virtual camera using a single VirtualCameraComponent
        /// </summary>
        /// <param name="virtualCamera"></param>
        void SetMainVirtualCameraProperties(VirtualCameraComponent virtualCamera)
        {
            primaryComposerVirtualCamera.transform.position = virtualCamera.position;
            primaryComposerVirtualCamera.transform.rotation = virtualCamera.rotation;
            primaryComposerVirtualCamera.m_Lens.FieldOfView = virtualCamera.FOV;
            primaryCameraComposer.m_ScreenX = virtualCamera.screenX;
            primaryCameraComposer.m_ScreenY = virtualCamera.screenY;
        }
        /// <summary>
        /// Sets the values of the primary virtual camera by blending between two VirtualCameraComponents
        /// </summary>
        /// <param name="virtualCamera"></param>
        IEnumerator SetMainVirtualCameraProperties(VirtualCameraComponent startVirtualCamera, VirtualCameraComponent endVirtualCamera, float time, bool useTimeScale)
        {
            float currentTime = 0f;
            while (currentTime <= time)
            {
                float lerpValue = currentTime / time;

                primaryComposerVirtualCamera.transform.position = Vector3.Slerp(startVirtualCamera.position, endVirtualCamera.position, lerpValue);
                primaryComposerVirtualCamera.transform.rotation = Quaternion.Slerp(startVirtualCamera.rotation, endVirtualCamera.rotation, lerpValue);
                primaryComposerVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startVirtualCamera.FOV, endVirtualCamera.FOV, lerpValue);
                primaryCameraComposer.m_ScreenX = Mathf.Lerp(startVirtualCamera.screenX, endVirtualCamera.screenX, lerpValue);
                primaryCameraComposer.m_ScreenY = Mathf.Lerp(startVirtualCamera.screenY, endVirtualCamera.screenY, lerpValue);
                currentTime += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime);
                yield return null;
            }
            SetMainVirtualCameraProperties(endVirtualCamera);
        }

        public void AddTransformToTargetGroup(Transform transform, float weight = 1, float radius = 1)
        {
            int index = targetGroup.FindMember(transform);
            if (index == -1)
            {
                targetGroup.AddMember(transform, weight, radius);
            }
            else
            {
                targetGroup.m_Targets[index].weight = weight;
                targetGroup.m_Targets[index].radius = radius;
            }
        }

        Vector2 sum = Vector2.zero;
        int count = 0;
        public void RegisterTarget(int id, Transform targetTransform, bool isTeamRed)
        {
            if(!targetDict.ContainsKey(id))
            {
                targetDict.Add(id, targetTransform);

                if(isTeamRed)
                {
                    redTeam.Add(id);
                }
                else
                {
                    blueTeam.Add(id);
                }
                ids.Add(id);
                sum += new Vector2(targetTransform.position.x, targetTransform.position.z);
                count++;
            }
        }

        public void OnTargetsRegistered()
        {
            sum /= count;
            camParent.transform.position = new Vector3(sum.x, 0, sum.y);

            for (int i = 0;i<ids.Count; i++)
            {
                float y = 0f;
                int id = ids[i];
                Transform targetTransform = targetDict[i];
                Vector2 diff = new Vector2(camParent.position.x, camParent.position.z) - new Vector2(targetTransform.position.x, targetTransform.position.z);

                Debug.DrawLine(camParent.transform.position, targetTransform.position, Color.red, 10000f);

                y = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg ;

                Debug.Log(y);

                yAngles.Add(id, y);
            }
        }

        public void SwitchToActivePlayer(int playerId)
        {
            StartCoroutine(StartCameraSwitch(playerId));
        }

        public IEnumerator StartCameraSwitch(int toPlayer,float switchTime = 2f)
        {
            float currentY = camParent.transform.localRotation.eulerAngles.y;
            float toY = 0f;

            if (targetDict.ContainsKey(toPlayer))
            {
                toY = yAngles[toPlayer];
                
                float elaspedTime = 0f;
                Vector3 tempEuler;

                camParent.DORotate(new Vector3(0, toY, 0), switchTime);

                //while (elaspedTime <= switchTime)
                //{
                //    tempEuler = camParent.transform.localRotation.eulerAngles;
                //    tempEuler.y = Mathf.Lerp(currentY, toY, elaspedTime / switchTime);
                //    camParent.transform.localRotation = Quaternion.Euler(tempEuler);
                //    elaspedTime += Time.deltaTime;
                //    yield return null;
                //}
                //tempEuler = camParent.transform.localRotation.eulerAngles;
                //camParent.transform.localRotation = Quaternion.Euler(tempEuler);

                tempEuler.y = toY;
            }
            yield return new WaitForSeconds(switchTime);
        }
    }

    public enum CameraControlType
    {
        NONE,
        BLEND,
        CUT
    }

    public enum VirtualCameraType
    {
        NONE,
        COMPOSER,
        GROUPCOMPOSER
    }
}
