using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.QuestSystem;
using System;
using RPG.Control;
using UnityEngine.PlayerLoop;
using RPG.NPCs;

public class GameManager : Singleton<GameManager>
{
    public static List<Consumables> allConsumables;

    [SerializeField]private PlayerInventory playerInventory;

    [SerializeField] List<Quest> playerQuests = new List<Quest>();
    [SerializeField] List<Quest> completedQuests = new List<Quest>();
    Quest currentTrackedQuest;

    public List<Transform> spawnPositions;
    public DuelNPC []currentDuelNPCs;
    public int currentNPCId;

    public bool hasPlayerLost;

    //-------------------------------
    private QuestLog questLog = null;
    public QuestLog QuestLogInstance
    {
        get
        {
            if (questLog == null)
            {
                questLog = FindObjectOfType<QuestLog>();
            }
            return questLog;
        }        
    }

    //------------------------------------------
    private PlayerWorldController player = null;
    public PlayerWorldController Player
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerWorldController>();
            }
            return player;
        }
    }

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
    public Vector3 hospitalWorldPos;

    public string worldScene = "World";

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        QuestEvents.QuestCompleted += AddCompletedQuestToList;
        base.Awake();

        currentDuelNPCs = FindObjectsOfType<DuelNPC>();
    }

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

    //-------------------------------------------------------------------
    public void PreBattleSetup(PlayerInfo npcInfo, PlayerInfo playerInfo) //Susceptible to change. Depends on number of enemies
    {
        currentTrackedQuest = QuestLogInstance.GetCurrentTrackedQuest();
        playerWorldPos = Player.transform.position;
        BattleInitializer.Instance.AddBattlePlayer(npcInfo); //NPC
        BattleInitializer.Instance.AddBattlePlayer(playerInfo); //Player            

        //Start Battle
        SceneManager.LoadScene("Lobby");
    }

    public void CheckForQuestCompletion(Action<Quest, bool> callback, Consumables consumable) // If Addition in Inventory
    {

    }

    //---------------------------------------
    public void UpdateQuests(BattleData data) // If Completed a Battle
    {
        //Iterate through all player quests
        for (int i = 0; i < playerQuests.Count; i++)
        {
            Quest quest = playerQuests[i];
            //Get current task requirement for every quest
            TaskType currentQuestTaskType = quest.questTasks[quest.currentQuestTaskIndex].taskType;

            //If npc matches quest task requirement update task
            /* TODO : Function currently takes into account 1v1 battles and therefore
                      only a single quest is updated */

            if (currentQuestTaskType.type == TaskType.Types.KILL)
            {
                foreach (BattleCharacters character in data.BattleLosers)
                {
                    if (character.Equals(currentQuestTaskType.killTargets.npcType))
                    {
                        quest.UpdateQuest();
                        break;
                    }
                }
            }
        }
        if (BattleInitializer.Instance.lobbyPlayers != null)
        {
            BattleInitializer.Instance.lobbyPlayers.Clear();
        }
    }

    //----------------------------------------
    public Vector3 GetRandomSpawnPointForNPC()
    {
        return spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)].position;
    }

    //--------------------------------------------
    public Vector3 GetPlayerSpawnPosition()
    {
        //if(hasPlayerLost)
        //{
        //    hasPlayerLost = false;
        //    return hospitalWorldPos;
        //} 

        return playerWorldPos;
    }

    //----------------------------------------------
    public void AddCompletedQuestToList(Quest quest)
    {
        completedQuests.Add(quest);
        for (int i = 0; i < playerQuests.Count; i++)
        {
            if(playerQuests[i].id == quest.id)
            {
                playerQuests.RemoveAt(i);
                break;
            }
        }
    }    

    //------------------------------------------------------
    public void AddAvailableQuestToPlayerQuests(Quest quest)
    {
        playerQuests.Add(quest);        
    }

    //----------------------------------
    public List<Quest> GetPlayerQuests()
    {
        return playerQuests;
    }

    //-------------------------------------
    public List<Quest> GetCompletedQuests()
    {
        return completedQuests;
    }

    //-----------------------------------
    public Quest GetCurrentTrackedQuest()
    {
        return currentTrackedQuest;
    }

    //---------------------
    public void OnDisable()
    {
        QuestEvents.QuestCompleted -= AddCompletedQuestToList;
    }
}

