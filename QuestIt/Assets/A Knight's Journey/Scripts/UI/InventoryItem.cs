using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] Image itemImage;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI quantityText;

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
