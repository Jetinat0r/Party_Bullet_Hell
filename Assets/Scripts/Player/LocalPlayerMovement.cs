using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerMovement : MonoBehaviour
{
    [SerializeField]
    private LocalPlayer player;

    public float movementSpeed = 5f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject playerPivot;

    [SerializeField]
    private float frictionCoefficient = 1f;

    private Vector2 movementVector = Vector2.zero;
    private Vector2 forceVector = Vector2.zero;


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
        //TODO: Check if this.magnitude is > 1, and if so: normalize and multipy again?
        Vector2 appliedForceVector = Time.fixedDeltaTime * forceVector;
        rb.MovePosition(rb.position +
            (movementSpeed * Time.fixedDeltaTime * movementVector.normalized) +
            (Time.fixedDeltaTime * forceVector));

        if(forceVector != Vector2.zero)
        {
            forceVector -= frictionCoefficient * forceVector;
        }
    }

    public void AddForce(Vector2 newForce)
    {
        forceVector += newForce;
    }
}
