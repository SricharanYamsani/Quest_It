using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : IMenuButtons
{
    public override void OnButtonPress()
    {
        base.Awake();

        MainMenuManager.Instance.faderCanvas.SetFader(1, 1, () =>
        {
            Application.Quit();
        });
    }
}
