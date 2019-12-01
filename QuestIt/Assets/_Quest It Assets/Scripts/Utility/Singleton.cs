using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<T> ( );

                if ( instance == null )
                {
                    GameObject temp = new GameObject ( );

                    temp.name = typeof ( T ).Name;

                    instance = temp.AddComponent<T> ( );
                }
            }

            return instance;
        }
    }

    public bool isDontDestroyOnLoad = false;

    private void Awake ( )
    {
        if ( instance == null )
        {
            instance = GetComponent<T> ( );

            if ( isDontDestroyOnLoad )
            {
                DontDestroyOnLoad ( this.gameObject );
            }
        }
        else if ( instance != this )
        {
            Destroy ( this.gameObject );
        }
    }
}
