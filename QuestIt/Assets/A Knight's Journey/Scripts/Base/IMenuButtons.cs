using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IMenuButtons : MonoBehaviour
{
    public Button myButton;

    protected virtual void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(() => { OnButtonPress(); });
    }
    public abstract void OnButtonPress();
}
