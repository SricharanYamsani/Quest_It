using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraControl
{
    public class VirtualCameraComponent
    {
        public Vector3 position;
        public Quaternion rotation;
        public float FOV;
        public float screenX, screenY;

        public VirtualCameraComponent(Transform virtualCameraTransform,float FOV,float screenX = 0.5f,float screenY = 0.5f)
        {
            position = virtualCameraTransform.position;
            rotation = virtualCameraTransform.rotation;
            this.FOV = FOV;
            this.screenX = screenX;
            this.screenY = screenY;
        }

        public VirtualCameraComponent(VirtualCameraComponent virtualCameraTransform)
        {
            position = virtualCameraTransform.position;
            rotation = virtualCameraTransform.rotation;
            this.FOV = virtualCameraTransform.FOV;
            this.screenX = virtualCameraTransform.screenX;
            this.screenY = virtualCameraTransform.screenY;
        }

        public static VirtualCameraComponent GetVirtualCameraComponent(Transform virtualCameraTransform, float FOV, float screenX = 0.5f, float screenY = 0.5f)
        {
            VirtualCameraComponent component = new VirtualCameraComponent(virtualCameraTransform, FOV, screenX, screenY);
            return component;
        }
    }
}

