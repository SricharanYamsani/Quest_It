using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLobbyButton : MonoBehaviour
{
    private Button myButton;

    private void Awake()
    {
        myButton = this.GetComponent<Button>();

        myButton.onClick.AddListener(() =>
        {
            BattleInitializer.Instance.StartLobby();
        });
    }
}
