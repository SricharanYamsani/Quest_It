using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{
    public Transform contentTransform;
    GridLayout gridLayoutComponent;

    public InventoryItem inventoryItemPrefab;

    public void Init(PlayerInventory playerInventory)
    {
        Dictionary<Consumables, int> playerConsumables = playerInventory.GetPlayerConsumables();

        foreach (Consumables consumable in playerConsumables.Keys)
        {
            InventoryItem item = Instantiate(inventoryItemPrefab, contentTransform);
            item.Init(consumable.ToString(), playerConsumables[consumable]);
        }
    }
}
