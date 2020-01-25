using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FreeLookCustomInput : MonoBehaviour
{
    CinemachineFreeLook freelook;
    [SerializeField] float ySpeed;
    [SerializeField] float xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        freelook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            freelook.m_XAxis.Value += Input.GetAxis("Mouse X") * xSpeed;
            freelook.m_YAxis.Value -= Input.GetAxis("Mouse Y") * ySpeed; 
        }
    }
}
