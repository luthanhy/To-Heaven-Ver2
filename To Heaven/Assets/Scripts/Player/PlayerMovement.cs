using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    public float speed = 6f;         // Walking speed
    public float runSpeed = 12f;     // Running speed
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Reference to the camera
    public Transform cameraTransform;

    // Smoothing variable for rotation
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Update Animator parameter for grounded state
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep the player on the ground without a hard impact
        }

        // Get player input for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction based on camera orientation
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        // Only proceed if there's movement input
        if (direction.magnitude >= 0.1f)
        {
            // Calculate the target rotation based on camera and input
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Rotate the player to face the direction of movement
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Determine movement direction and speed
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : speed;

            // Determine if moving forward or backward
            if (z < 0) // Moving backward
            {
                currentSpeed = speed; // Set to walking speed for backward movement
                animator.SetBool("isWalkingBackward", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
            else // Moving forward
            {
                animator.SetBool("isWalkingBackward", false);
                animator.SetBool("isWalking", !isRunning);
                animator.SetBool("isRunning", isRunning);
            }

            // Move the player
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            // Debugging log
            Debug.Log("Walking: " + animator.GetBool("isWalking") + ", Running: " + animator.GetBool("isRunning") + ", Walking Backward: " + animator.GetBool("isWalkingBackward"));
        }
        else
        {
            // No input: transition to idle
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkingBackward", false);
        }

        // Jump handling
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move player vertically (gravity effect)
        controller.Move(velocity * Time.deltaTime);
    }
}
