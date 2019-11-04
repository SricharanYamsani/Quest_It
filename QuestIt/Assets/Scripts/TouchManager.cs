using UnityEngine;

public class TouchManager : MonoBehaviour
{
    bool multiTouch = false;

    #region TOUCH DATA

    public Vector2 StartPosition { get; protected set; }
    public Vector2 EndPosition { get; protected set; }
    public Vector2 DeltaPosition { get; protected set; }
    #endregion

    private void Update()
    {
        ProcessTouches();
    }

    void ProcessTouches()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        if(multiTouch)
        {
            //TODO : MULTITOUCH FUNCTIONS TO BE ADDED, CAN IGNORE FOR NOW
        }
        else
        {
            Touch touch = Input.touches[0];

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchPhaseStart(touch);
                    break;
                case TouchPhase.Moved:
                    OnTouchPhaseMoved(touch);
                    break;
                case TouchPhase.Ended:
                    OnTouchPhaseEnd(touch);
                    break;
                case TouchPhase.Stationary:
                    OnTouchPhaseStationary(touch);
                    break;
            }
        }
    }

    void OnTouchPhaseStart(Touch touch)
    {
        StartPosition = touch.position;
    }
    void OnTouchPhaseEnd(Touch touch)
    {
        EndPosition = touch.position;
    }
    void OnTouchPhaseMoved(Touch touch)
    {
        DeltaPosition = touch.deltaPosition;
    }
    void OnTouchPhaseStationary(Touch touch)
    {
    }
}
