using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player playerPrefab;

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
    }

    //Will NOT be moved to the ClientConnected event, as I may not always want to send this kind of data
    [MessageHandler((ushort)ServerToClientId.playerSpawnInfo)]
    private static void SpawnPlayer(Message message)
    {
        ushort fromClientId = message.GetUShort();
        string playerUsername = message.GetString();

        Player player = Instantiate(GameManager.instance.playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Player.PlayerList.Add(fromClientId, player);
        player.GetComponent<SpriteRenderer>().color = Player.PlayerColorMap[fromClientId];

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
}
