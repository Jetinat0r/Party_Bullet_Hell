using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using TMPro;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();
    public static Dictionary<ushort, Color> PlayerColorMap = new Dictionary<ushort, Color>();

    [SerializeField]
    public GameObject playerPivot; //TODO: Come up w/ a better name

    [SerializeField]
    public GameObject handHolder;
    [SerializeField]
    private Animation shoveAnimation;
    [SerializeField]
    private GameObject shoveForceBox;


    [SerializeField]
    private TMP_Text nameplate;
    [SerializeField]
    private SpriteRenderer baseGraphics;
    [SerializeField]
    private SpriteRenderer[] handGraphics = new SpriteRenderer[2];


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

    public void SetSpawnInfo(ushort id, string username)
    {
        PlayerId = id;
        PlayerUserName = username;
        //Above line should actually be handled by server
        baseGraphics.color = PlayerColorMap[id];

        foreach(SpriteRenderer sr in handGraphics)
        {
            sr.color = PlayerColorMap[id];
        }

        nameplate.text = PlayerUserName;
    }

    public void SetPosRot(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        playerPivot.transform.rotation = rot;
    }

    //Plays the shove animation and creates a hostile shove force box
    public void RemoteShove(Vector3 pos, Quaternion rot)
    {
        Destroy(Instantiate(shoveForceBox, pos, rot), 0.3f);

        PlayShoveAnim();
    }

    public void PlayShoveAnim()
    {
        //shoveAnimation.Rewind();
        shoveAnimation.Play();
    }

    #region Messages
    protected void SendPlayerPosRot()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.playerPosRot);
        message.AddVector3(transform.position);
        message.AddQuaternion(playerPivot.transform.rotation);

        NetworkManager.instance.Client.Send(message);
    }
    #endregion
}
