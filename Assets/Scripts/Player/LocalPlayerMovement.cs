using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject playerPivot;

    private Vector2 movementVector;


    void Update()
    {
        //TODO: Update to new input system
        //Input
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 _directionToMouse = new Vector2(_mousePos.x - transform.position.x, _mousePos.y - transform.position.y);

        playerPivot.transform.right = _directionToMouse;
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (movementVector.normalized * movementSpeed * Time.fixedDeltaTime));
    }
}
