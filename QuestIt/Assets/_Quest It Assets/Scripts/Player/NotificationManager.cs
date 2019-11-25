using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private NotificationManager instance;

    public NotificationManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<NotificationManager> ( );

                if(instance == null)
                {
                    GameObject nObject = new GameObject ( );

                    nObject.name = "NotificationManager";

                    instance = nObject.AddComponent<NotificationManager> ( );
                }
            }

            return instance;
        }
    }
    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad ( this.gameObject );
        }
        else if(instance!=this)
        {
            Destroy ( this.gameObject );
        }
    }

    public void SetupaNotification()
    {

    }
}
