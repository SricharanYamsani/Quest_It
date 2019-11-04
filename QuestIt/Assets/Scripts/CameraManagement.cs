using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    [Range ( 0 , 5 )]
    public float distance = 2;

    [Range ( 0 , 500 )]
    public float height = 10;

    public GameObject lookat;

    private bool moveCamera = false;

    private void Awake ( )
    {
        // We will find the object later. todo
        if ( lookat )
        {
            moveCamera = true;
        }
    }
    private void LateUpdate ( )
    {
        if ( moveCamera )
        {
            transform.localPosition = new Vector3 ( lookat.transform.position.x , lookat.transform.position.y + height , lookat.transform.position.z - distance );
        }
    }

    void PanCamera()
    {

    }
}
