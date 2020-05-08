using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = this.GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(() => { BattleInitializer.Instance.LoadWorldScene(GameManager.Instance.worldScene); });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
