using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class LocalPlayerAttack : MonoBehaviour
{
    [SerializeField]
    private LocalPlayer player;

    [SerializeField]
    private float pushCooldown = 1f;

    private float pushTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        pushTimer -= Time.deltaTime;

        if(pushTimer <= 0f && Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryShove();
        }
    }

    private void TryShove()
    {
        pushTimer = pushCooldown;
        SendShove();
        player.PlayShoveAnim();
    }

    private void SendShove()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.shove);

        Transform pivotTransform = player.playerPivot.transform;
        message.AddVector3(pivotTransform.position);
        message.AddQuaternion(pivotTransform.rotation);

        NetworkManager.instance.Client.Send(message);
    }
}
