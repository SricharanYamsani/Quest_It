using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    Image itemImage;
    
    TextMeshProUGUI nameText;
    TextMeshProUGUI quantityText;

    public void Init (string name,int quantity,Sprite sprite = null)
    {
        nameText.text = name;
        quantityText.text = quantity.ToString();

        if(sprite != null)
        {
            itemImage.sprite = sprite;
        }
    }

}
