using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using TMPro;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();
    public static Dictionary<ushort, Color> PlayerColorMap = new Dictionary<ushort, Color>();

    [SerializeField]
    private GameObject playerPivot; //TODO: Come up w/ a better name

    [SerializeField]
    private TMP_Text nameplate;
    [SerializeField]
    private SpriteRenderer baseGraphics;


    public ushort PlayerId { get; protected set; }
    public string PlayerUserName { get; protected set; }

    private void Awake()
    {
        if (PlayerColorMap.Count == 0)
        {
            PlayerColorMap.Add(1, Color.red);
            PlayerColorMap.Add(2, Color.green);
            PlayerColorMap.Add(3, Color.magenta);
            PlayerColorMap.Add(4, Color.blue);
        }
    }

    private void FixedUpdate()
    {
        //SendPlayerPosRot();
    }

    public void SetSpawnInfo(ushort id, string username)
    {
        //TODO: Set color in here
        PlayerId = id;
        PlayerUserName = username != "" ? username : $"Guest ({PlayerId})"; //TODO: Move to when the name is received
        //Above line should actually be handled by server
        baseGraphics.color = PlayerColorMap[id];

        nameplate.text = PlayerUserName;
    }

    public void SetPosRot(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        playerPivot.transform.rotation = rot;
    }

    #region Messages
    protected void SendPlayerPosRot()
    {
        //TODO: Make it so only the local player sends this
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.playerPosRot);
        message.AddVector3(transform.position);
        message.AddQuaternion(playerPivot.transform.rotation);

        NetworkManager.instance.Client.Send(message);
    }
    #endregion
}
