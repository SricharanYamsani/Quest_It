using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public IMenuButtons playButton;
    public IMenuButtons loadButton;
    public IMenuButtons optionsButton;
    public IMenuButtons quitButton;

    public ViewFader faderCanvas;

    private void Start()
    {
        faderCanvas.SetFader(0, 1, () =>
        {
            SoundManager.Instance.PlayBGMusic("Menu");
        });
    }
}
