using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.Core
{
    public class FreeLookCustomInput : MonoBehaviour
    {
        //====================Variables==================//

        CinemachineFreeLook freelook;
        [SerializeField] float ySpeed;
        [SerializeField] float xSpeed;

        //====================Functions=================//

        // Start is called before the first frame update
        //----------
        void Start()
        {
            freelook = GetComponent<CinemachineFreeLook>();
        }

        // Update is called once per frame
        //-----------
        void Update()
        {
            //Right-Click
            if (Input.GetMouseButton(1))
            {
                freelook.m_XAxis.Value += Input.GetAxis("Mouse X") * xSpeed;
                freelook.m_YAxis.Value -= Input.GetAxis("Mouse Y") * ySpeed;
            }
        }
    }
}
