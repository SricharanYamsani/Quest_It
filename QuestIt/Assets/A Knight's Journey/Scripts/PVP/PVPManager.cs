using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PVPManager : MonoBehaviourPunCallbacks
{
    public static byte MinPlayers { get; protected set; }
    public static byte MaxPlayers { get; protected set; }

    public static string RoomName { get; protected set; }

    public static float MatchMakingTime { get; protected set; }

    public static List<QuestItPlayer> opponents;
    public static QuestItPlayer player;
    private void Initialize()
    {
        MinPlayers = 2;
        MaxPlayers = 2;
        RoomName = "Test";
        MatchMakingTime = 30f;
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError("Connected to master. \n Trying to join random room...");

        player.name = "PLAYER";
        player.photonId = PhotonNetwork.LocalPlayer.UserId;

        PhotonNetwork.JoinRandomRoom(null,MaxPlayers,MatchmakingMode.RandomMatching,null,"");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("Couldn't join random room. \n Creating a new room");
        opponents = new List<QuestItPlayer>();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = MaxPlayers;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom(RoomName, roomOptions, null, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("Joined Room. Is Master = " + PhotonNetwork.IsMasterClient);
        opponents = new List<QuestItPlayer>();

        photonView.RPC("ShareInfo", RpcTarget.OthersBuffered, player.name,player.photonId);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError("Player Entered : " + newPlayer.NickName);

        photonView.RPC("ShareInfo", newPlayer, player.name, player.photonId);

        if(PhotonNetwork.IsMasterClient)
        {
            if(opponents.Count == MaxPlayers || (MatchMakingTime < 0 &&  opponents.Count >= MinPlayers))
            {
                //Start game

            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room, returnCode : " + returnCode + "\nMessage : " + message);
    }

    [PunRPC]
    void ShareInfo(string name,string photonId)
    {
        QuestItPlayer player = new QuestItPlayer();
        player.name = name;
        player.photonId = photonId;
        opponents.Add(player);
    }
}

public class QuestItPlayer
{
    public string name;
    public string photonId;
}
