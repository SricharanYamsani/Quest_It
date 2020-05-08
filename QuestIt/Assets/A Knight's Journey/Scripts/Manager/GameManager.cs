using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static List<Consumables> allConsumables;
    PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = new PlayerInventory();
    }

    public PlayerInventory GetPlayerInventory() 
    {
        return playerInventory;
    }
}
