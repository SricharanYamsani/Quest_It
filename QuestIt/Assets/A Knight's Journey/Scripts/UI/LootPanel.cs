using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LootPanel : MonoBehaviour
{ 
    [SerializeField] InventoryItem inventoryItemPrefab;

    [SerializeField] Transform contentTransform;

    [SerializeField] Button okButton;

    [SerializeField] TextMeshProUGUI headerText;
    public void Init(BattleOutcome outcome, UnityAction okButtonFn)
    {
        headerText.text = outcome.ToString();
        okButton.onClick.RemoveAllListeners();
        if(okButtonFn != null)
        {
            okButton.onClick.AddListener(okButtonFn);
        }
    }

    public void AddLoot(Consumables consumables,int quantity )
    {
        InventoryItem item = Instantiate(inventoryItemPrefab, contentTransform);

        item.Init(consumables.ToString(), quantity);
    }

    public void AddCoins(int amount)
    {
        InventoryItem item = Instantiate(inventoryItemPrefab, contentTransform);

        item.Init("OSHAIN", amount,ResourceManager.Instance.currencySpritesRef[Currency.OSHAIN.ToString()]);
    }
}
