using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    [SerializeField]
    Vector2 positionOffset = new Vector2(5, 10);
    
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
            transform.localPosition = new Vector3 ( lookat.transform.position.x , lookat.transform.position.y + positionOffset.y , lookat.transform.position.z - positionOffset.x );
        }
    }

    void PanCamera(float deltaAngle)
    {
    }
}
