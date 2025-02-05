using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float gravityMultiplier = 2f; // Multiplier for gravity
    [SerializeField]
    private float groundCheckDistance = 0.2f; // Distance for ground check
    [SerializeField]
    private LayerMask groundLayer; // Layer to define what is ground

    [Header("References")]
    [SerializeField]
    private Rigidbody myRigidbody;

    private bool isGrounded;

    // Start is called before the first frame update
    void Awake()
    {
        if (!myRigidbody)
        {
            myRigidbody = GetComponent<Rigidbody>();
        }

        // Ensure gravity is enabled on the Rigidbody
        myRigidbody.useGravity = true;
    }

    private void Update()
    {
        GroundCheck(); // Continuously check if player is on the ground
    }

    public void Move(Vector3 moveDirection)
    {
        // Preserve the player's vertical velocity to allow gravity to function
        Vector3 currentVelocity = myRigidbody.velocity;

        // Apply movement (x, z)
        Vector3 newVelocity = moveDirection.normalized * moveSpeed;
        newVelocity.y = currentVelocity.y; // Preserve vertical velocity

        // Apply custom gravity force if the player is not grounded
        if (!isGrounded)
        {
            newVelocity.y += Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Apply the new velocity to the Rigidbody
        myRigidbody.velocity = newVelocity;
    }

    // Ground Check method using Raycast
    private void GroundCheck()
    {
        // Raycast downward from the player's position to check if they're near the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
