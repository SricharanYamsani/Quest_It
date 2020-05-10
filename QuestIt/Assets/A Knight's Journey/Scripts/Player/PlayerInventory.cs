using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    [SerializeField]Dictionary<Consumables, int> playerConsumables;
    [SerializeField]int coins;

    public PlayerInventory()
    {
        playerConsumables = new Dictionary<Consumables, int>();
        coins = 0;

        FillRandomConsumables();
    }
    public int Coins
    {
        get;
    }

    public void AddCoins(int value)
    {
        coins += value;
    }

    public Dictionary<Consumables,int> GetPlayerConsumables()
    {
        return playerConsumables;
    }

    public int GetConsumableQuantity(Consumables consumables)
    {
        if(playerConsumables.ContainsKey(consumables))
        {
            return playerConsumables[consumables];
        }
        return 0;
    }

    public void AddConsumable(Consumables consumables, int value)
    {
        if(playerConsumables.ContainsKey(consumables))
        {
            playerConsumables[consumables] += value;
        }
        else
        {
            playerConsumables.Add(consumables, value);
        }
    }

    void FillRandomConsumables()
    {
        List<Consumables> availableConsumableList = GameManager.allConsumables;

        if (availableConsumableList != null)
        {
            for (int i = 0; i < availableConsumableList.Count; i++)
            {
                if (Random.value < 0.6) continue;

                AddConsumable(availableConsumableList[i], Random.Range(0, 3));
            }
        }
    }
}
