using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : IMenuButtons
{
    public override void OnButtonPress()
    {
        MainMenuManager.Instance.faderCanvas.SetFader(1, 0.75f, () =>
        {
            SceneManager.LoadScene(GameManager.Instance.worldScene);
        });
    }
}
