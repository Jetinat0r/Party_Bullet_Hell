using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Riptide;

public class JoinServerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject joinServerPanel;

    [SerializeField]
    private TMP_InputField userNameField;
    [SerializeField]
    private TMP_InputField serverIpField;
    [SerializeField]
    private TMP_InputField serverPortField;

    private void Start()
    {
        NetworkManager.instance.Client.Connected += DidConnect;
        NetworkManager.instance.Client.ConnectionFailed += FailedToConnect;
        NetworkManager.instance.Client.Disconnected += DidDisconnect;
    }

    public void JoinServer()
    {
        /*
        if (userNameField.text == "")
        {
            //TODO: Tell player that they need a username, or send a default username
            return;
        }
        */

        try
        {
            IPAddress[] addresses = Dns.GetHostAddresses(serverIpField.text);

            if (addresses.Length == 0)
            {
                //TODO: Alert to invalid IP, should never run
                return;
            }

            CloseJoinMenu();
            NetworkManager.instance.JoinServer(userNameField.text, addresses[0].ToString(), serverPortField.text);
        }
        catch(Exception e)
        {
            //TODO: Alert client to invalid IP
            Debug.LogError($"Invalid Host Name! Error:\n{e}");
        }
    }

    private void CloseJoinMenu()
    {
        joinServerPanel.SetActive(false);
    }
    private void ReturnToMainMenu()
    {
        joinServerPanel.SetActive(true);
    }

    #region Server Events
    private void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.joinInfo);
        message.AddString(userNameField.text);

        NetworkManager.instance.Client.Send(message);
    }

    private void DidConnect(object sender, EventArgs e)
    {
        SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        ReturnToMainMenu();
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        ReturnToMainMenu();
    }
    #endregion
}
