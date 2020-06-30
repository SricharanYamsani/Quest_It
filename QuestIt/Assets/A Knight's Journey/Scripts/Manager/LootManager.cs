using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] LootPanel lootPanelPrefab;
    public void GenerateLoot(int difficulty,int experienceGained ,int alliesLeft, int totalHealthLeft,BattleOutcome outcome,UnityEngine.Events.UnityAction action, bool instantiatePanel = true)
    {
        if (lootPanelPrefab == null)
        {
            lootPanelPrefab = Resources.Load<LootPanel>("Prefabs/UI/LootPanel");
        }
        int coins = 50 * difficulty + 20 * alliesLeft + totalHealthLeft;
        //Debug.LogError(coins);
        PlayerInventory playerInventory = GameManager.Instance.GetPlayerInventory();

        playerInventory.AddCoins(coins);

        LootPanel lootPanel = null;
        if (instantiatePanel)
        {
            BattleUIManager.Instance.battleCanvas.SetActive(false);
            lootPanel = Instantiate(lootPanelPrefab, BattleUIManager.Instance.canvasTransform);
            lootPanel.Init(outcome, action);
            lootPanel.AddCoins(coins);
        }
        if (instantiatePanel && GameManager.allConsumables != null && GameManager.allConsumables.Count > 0)
        {
            for (int i = alliesLeft; i > 0; i--)
            {
                Consumables randomConsumable = GameManager.allConsumables[Random.Range(0, GameManager.allConsumables.Count)];

                int quantity = Random.Range(1, difficulty * alliesLeft);

                playerInventory.AddConsumable(randomConsumable, quantity);

                lootPanel.AddLoot(randomConsumable, quantity);
            }
        }
    }

    
}
