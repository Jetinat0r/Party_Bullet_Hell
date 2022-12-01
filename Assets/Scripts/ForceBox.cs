using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBox : MonoBehaviour
{
    //TODO: Rework to be less hacky. This will work for the demo though

    //Only has to be local because remote players can handle their own stuff
    private LocalPlayerMovement localPlayerMovement = null;

    [SerializeField]
    private float forceMultiplier = 1f;

    private void FixedUpdate()
    {
        if(localPlayerMovement != null)
        {
            localPlayerMovement.AddForce(forceMultiplier * transform.right);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (localPlayerMovement == null)
        {
            localPlayerMovement = collision.gameObject.GetComponent<LocalPlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<LocalPlayerMovement>() != null)
        {
            localPlayerMovement = null;
        }
    }
}
