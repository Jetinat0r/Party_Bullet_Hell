using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player localPlayerPrefab;
    public Player remotePlayerPrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NetworkManager.instance.Client.ClientDisconnected += RemovePlayer;
        NetworkManager.instance.Client.Disconnected += DisconnectFromServer;
    }

    #region Messages
    #region Joining & Disconnecting
    //Will NOT be moved to the ClientConnected event, as I may not always want to send this kind of data
    [MessageHandler((ushort)ServerToClientId.playerSpawnInfo)]
    private static void SpawnPlayer(Message message)
    {
        ushort fromClientId = message.GetUShort();
        string playerUsername = message.GetString();
        if(playerUsername == "")
        {
            playerUsername = $"Guest ({fromClientId})";
        }

        Player player;
        if(fromClientId == NetworkManager.instance.Client.Id)
        {
            //Spawn local Player prefab
            player = Instantiate(GameManager.instance.localPlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

        }
        else
        {
            //Spawn remote Player prefab
            player = Instantiate(GameManager.instance.remotePlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
        Player.PlayerList.Add(fromClientId, player);
        player.SetSpawnInfo(fromClientId, playerUsername);
        //player.GetComponent<SpriteRenderer>().color = Player.PlayerColorMap[fromClientId];

        Debug.Log($"Player \"{playerUsername}\" joined!");
    }

    private static void RemovePlayer(object sender, ClientDisconnectedEventArgs e)
    {
        //ushort fromClientId = message.GetUShort();

        Player player = Player.PlayerList[e.Id];
        Player.PlayerList.Remove(e.Id);
        Destroy(player.gameObject);

        Debug.Log($"Removed client id {{{e.Id}}}");
    }

    //Clears all gameplay elements from the screen
    private static void DisconnectFromServer(object sender, EventArgs e)
    {
        //Destroy all players
        foreach(ushort clientId in Player.PlayerList.Keys)
        {
            Destroy(Player.PlayerList[clientId].gameObject);
        }
        Player.PlayerList.Clear();

    }
    #endregion

    #region Movement
    [MessageHandler((ushort)ServerToClientId.playerPosRot)]
    private static void SetPlayerPosRot(Message message)
    {
        ushort clientId = message.GetUShort();
        Vector3 pos = message.GetVector3();
        Quaternion rot = message.GetQuaternion();

        if (Player.PlayerList.TryGetValue(clientId, out Player player))
        {
            player.SetPosRot(pos, rot);
        }
    }
    #endregion
    [MessageHandler((ushort)ServerToClientId.playerShove)]
    private static void MakePlayerShove(Message message)
    {
        ushort clientId = message.GetUShort();

        Player.PlayerList[clientId].RemoteShove(message.GetVector3(), message.GetQuaternion());
    }
    #region Player Attacks

    #endregion
    #endregion
}
