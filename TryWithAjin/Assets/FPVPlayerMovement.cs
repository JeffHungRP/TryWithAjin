using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVPlayerMovement : MonoBehaviour
{
    [Header("FPV Character Setup")]
    public CharacterController characterController;
    public Transform firstPersonCamera;
    public Transform groundDetector;
    
    public LayerMask groundMask;

    [Header("Movement Parameters")]
    public float speed = 10f;
    public float gravity = -30f;
    public float jumpHeight = 15f;
    public float groundDistance = 0.2f;
    public float lookSensitivity = 500f;

    [Header("Misc.")]
    public bool enableMovement = true;
    public bool enableLook = true;

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

// Try out 123
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (enableMovement) CharacterMove();
        if (enableLook) MouseLook();
    }

    void CharacterMove()
    {
        // Checking if the FPV Player in on ground
        isGrounded = Physics.CheckSphere(groundDetector.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Detecting WASD Keyboard inputs
        float x = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // Applying the movements
        Vector3 move = transform.right * x + transform.forward * v;
        velocity.x = move.x * speed;
        velocity.z = move.z *speed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpHeight;
        }
        velocity.y += gravity * Time.deltaTime;

        // Move the Player
        characterController.Move(velocity * Time.deltaTime);
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        firstPersonCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
