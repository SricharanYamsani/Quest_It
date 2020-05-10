using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.QuestSystem;
using System;

public class GameManager : Singleton<GameManager>
{
    public static List<Consumables> allConsumables;

    private PlayerInventory playerInventory;

    private List<Quest> currentQuests = new List<Quest>();

    private static int playerLevel;

    public static int PlayerLevel
    {
        get
        {
            return playerLevel;
        }
        private set
        {
            playerLevel = value;
        }
    }

    public Vector3 playerWorldPos;

    public string worldScene = "World";

    private void Start()
    {
        playerInventory = new PlayerInventory();
    }

    public PlayerInventory GetPlayerInventory()
    {
        if(playerInventory == null)
        {
            Debug.LogError("Player inventory was null");
            playerInventory = new PlayerInventory();
        }

        return playerInventory;
    }

    public void CheckForQuestCompletion(Action<Quest, bool> callback, Consumables consumable) // If Addition in Inventory
    {

    }

    public void CheckForQuestCompletion(Action<bool> callback, BattleData data) // If Completed a Battle
    {
        Quest holder;

        bool questChanged = false;

        for (int i = 0; i < currentQuests.Count; i++)
        {
            holder = currentQuests[i];

            for (int j = 0; j < holder.questTasks.Count; j++)
            {
                TaskType type = holder.questTasks[j].taskType;

                if (type.type == TaskType.Types.KILL)
                {

                    foreach (BattleCharacters character in data.BattleLosers)
                    {
                        if (character.Equals(type.killTargets.npcType))
                        {
                            type.current++;

                            if (type.current >= type.numberOfTimes)
                            {
                                holder.questTasks[j].completedTask = true;
                            }

                            questChanged = true;
                        }
                    }
                }
            }

            bool questCompleted = true;

            foreach (QuestTask qt in holder.questTasks)
            {
                if (!qt.completedTask)
                {
                    questCompleted = false;
                }
            }

            holder.completedQuest = questCompleted;
        }

        callback?.Invoke(questChanged);
    }

    public void GetCurrentQuests(Action<List<Quest>> callback)
    {
        List<Quest> temp = new List<Quest>();

        for (int i = 0; i < currentQuests.Count; i++)
        {
            temp.Add(currentQuests[i]);
        }

        callback?.Invoke(temp);
    }

    public void AddQuests(Quest quest)
    {
        currentQuests.Add(quest);
    }
}

