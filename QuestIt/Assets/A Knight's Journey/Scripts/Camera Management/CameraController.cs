using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineBrain primaryCamera;
        public CinemachineVirtualCamera mainVirtualCamera;
        private CinemachineComposer mainCameraComposer;
        private VirtualCameraComponent defaultVirtualCamera;

        Coroutine cameraControlCoroutine, cameraControlSubCoroutine;

        private void Awake()
        {
            mainCameraComposer = mainVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
            if(mainCameraComposer != null)
            {
                defaultVirtualCamera = new VirtualCameraComponent(mainVirtualCamera.transform, mainVirtualCamera.m_Lens.FieldOfView, mainCameraComposer.m_ScreenX, mainCameraComposer.m_ScreenY);
            }
            else
            {
                defaultVirtualCamera = new VirtualCameraComponent(mainVirtualCamera.transform, mainVirtualCamera.m_Lens.FieldOfView);
            }
        }

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

        void SetMainVirtualCameraProperties(VirtualCameraComponent virtualCamera)
        {
            mainVirtualCamera.transform.position = virtualCamera.position;
            mainVirtualCamera.transform.rotation = virtualCamera.rotation;
            mainVirtualCamera.m_Lens.FieldOfView = virtualCamera.FOV;
            mainCameraComposer.m_ScreenX = virtualCamera.screenX;
            mainCameraComposer.m_ScreenY = virtualCamera.screenY;
        }

        IEnumerator SetMainVirtualCameraProperties(VirtualCameraComponent startVirtualCamera, VirtualCameraComponent endVirtualCamera, float time, bool useTimeScale)
        {
            float currentTime = 0f;
            while (currentTime <= time)
            {
                float lerpValue = currentTime / time;

                mainVirtualCamera.transform.position = Vector3.Slerp(startVirtualCamera.position, endVirtualCamera.position, lerpValue);
                mainVirtualCamera.transform.rotation = Quaternion.Slerp(startVirtualCamera.rotation, endVirtualCamera.rotation, lerpValue);
                mainVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startVirtualCamera.FOV, endVirtualCamera.FOV, lerpValue);
                mainCameraComposer.m_ScreenX = Mathf.Lerp(startVirtualCamera.screenX, endVirtualCamera.screenX, lerpValue);
                mainCameraComposer.m_ScreenY = Mathf.Lerp(startVirtualCamera.screenY, endVirtualCamera.screenY, lerpValue);
                currentTime += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime);
                yield return null;
            }
            SetMainVirtualCameraProperties(endVirtualCamera);
        }
    }

    public enum CameraControlType
    {
        NONE,
        BLEND,
        CUT
    }
}
