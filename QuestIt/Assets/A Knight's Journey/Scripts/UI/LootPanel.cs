using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPanel : MonoBehaviour
{ 
    [SerializeField] InventoryItem inventoryItemPrefab;

    [SerializeField] Transform contentTransform;

    public void AddLoot(Consumables consumables,int quantity )
    {
        InventoryItem item = Instantiate(inventoryItemPrefab, contentTransform);

        item.Init(consumables.ToString(), quantity);
    }
}
