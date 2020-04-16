using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadManager : Singleton<LoadManager>
{
    public LoadManagerUI loadUI;

    public LoadManagerUI reference;

    protected override void Awake()
    { 
        base.Awake();
    }
    public void LoadBattleScene(List<PlayerInfo> listOfPlayers, string battleGround)
    {
        if (loadUI == null)
        {
            if (reference == null)
            {
                reference = Resources.Load<LoadManagerUI>("Prefabs/UI/LoaderManagerUI");
            }
            loadUI = Instantiate<LoadManagerUI>(reference);
        }
        DontDestroyOnLoad(loadUI);

        loadUI.gameObject.SetActive(true);

        loadUI.loadingImage.fillAmount = 0;

        loadUI.backgroundCanvas.DOFade(1, 0.2f).OnComplete(() =>
        {
            loadUI.loadingImage.DOFillAmount(0.2f, 0.4f).OnComplete(() =>
            {
                StartCoroutine(LoadSceneAsync(listOfPlayers, battleGround));
            });
        });
    }

    IEnumerator LoadSceneAsync(List<PlayerInfo> listOfPlayers, string battleGround)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(battleGround);

        loadOperation.allowSceneActivation = false;

        while (!loadOperation.allowSceneActivation)
        {
            loadUI.loadingImage.fillAmount = loadOperation.progress;

            if (loadOperation.progress >= 0.9f)
            {
                InformationHandler.Instance.SetLobby(listOfPlayers);

                loadOperation.allowSceneActivation = true;

                while (!InformationHandler.Instance.isLoaded)
                {
                    yield return null;
                }

                loadUI.backgroundCanvas.DOFade(0, 0.2f).OnComplete(() =>
                {
                    loadUI.gameObject.SetActive(false);
                });
            }
        }
    }
}