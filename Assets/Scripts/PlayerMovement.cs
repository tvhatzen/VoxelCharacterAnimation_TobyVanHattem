using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    [SerializeField]
    private AnimatorController myAnim;

    private Vector3 tempMovement;

    private Vector3 lastValidMovement; // Cache for the last valid movement direction

    private void Update()
    {
        // Get movement input relative to camera's orientation
        tempMovement = Input.GetAxis("Horizontal") * Camera.main.transform.right + Input.GetAxis("Vertical") * Camera.main.transform.forward;
        tempMovement.y = 0f; // Ensure no vertical movement

        // Normalize the movement to ensure consistent speed
        tempMovement = tempMovement.normalized;
    }

    private void FixedUpdate()
    {
        // Call PlayerMove and ChangeAnimation methods every FixedUpdate
        PlayerMove();
        ChangeAnimation();
    }

    // Method to handle player movement
    void PlayerMove()
    {
        // Set the movement direction using the Move method from the BaseMovement class
        Move(tempMovement);
    }

    void ChangeAnimation()
    {
        // Check if the AnimationController reference is set
        if (myAnim)
        {
            // Set a small threshold to prevent spinning when movement input is very small
            float movementThreshold = 0.1f;

            // Check if the player is moving significantly
            if (tempMovement.magnitude > movementThreshold)
            {
                // Set the "Running" boolean parameter to true in the animator
                myAnim.ChangeAnimBoolValue("Running", true);

                // Update the last valid movement direction
                lastValidMovement = tempMovement;

                // Calculate rotation angle based on movement direction for player orientation
                float rot = Mathf.Atan2(-lastValidMovement.z, lastValidMovement.x) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.Euler(0f, rot, 0f);
            }
            else
            {
                // If not moving, set the "Running" boolean parameter to false
                myAnim.ChangeAnimBoolValue("Running", false);

                // Ensure the player doesn't spin when idle by stopping further rotation updates
                if (lastValidMovement.magnitude > movementThreshold)
                {
                    // Maintain the last rotation, no need to update when the player is idle
                    float rot = Mathf.Atan2(-lastValidMovement.z, lastValidMovement.x) * Mathf.Rad2Deg + 90f;
                    transform.rotation = Quaternion.Euler(0f, rot, 0f);
                }
            }
        }
    }
}
