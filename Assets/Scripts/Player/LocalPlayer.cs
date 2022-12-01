using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    [SerializeField]
    public LocalPlayerMovement movement;
    [SerializeField]
    public LocalPlayerAttack attack;

    private void FixedUpdate()
    {
        SendPlayerPosRot();
    }
}
