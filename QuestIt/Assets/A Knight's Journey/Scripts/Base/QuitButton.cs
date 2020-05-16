using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : IMenuButtons
{
    public override void OnButtonPress()
    {
        MainMenuManager.Instance.faderCanvas.SetFader(1, 1, () =>
        {
            Application.Quit();
        });
    }
}
