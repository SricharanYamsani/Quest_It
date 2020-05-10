using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] LootPanel lootPanelPrefab;
    public void GenerateLoot(int difficulty, int alliesLeft, int totalHealthLeft, bool instantiatePanel = true)
    {
        int coins = 50 * difficulty + 20 * alliesLeft + totalHealthLeft;

        PlayerInventory playerInventory = GameManager.Instance.GetPlayerInventory();

        playerInventory.AddCoins(coins);

        LootPanel lootPanel = null;
        if (instantiatePanel)
        {
            lootPanel = Instantiate(lootPanelPrefab, UIManager.Instance.overlayPanel);
        }
        if (instantiatePanel)
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
