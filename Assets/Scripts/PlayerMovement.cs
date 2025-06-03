using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    private InputAction moveAction;
    private InputAction jumpAction;

    private CharacterController controller;
    private Transform cam;

    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public Vector3 groundOffset = new Vector3(0f, 1.2f, 0f);
    public LayerMask groundMask;

    private Vector3 velocity;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    private bool isMoving;
    private bool isGrounded;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        moveAction = input.actions["Movement"];
        jumpAction = input.actions["Jump"];
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        isGrounded = Physics.CheckSphere(transform.position - groundOffset, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        controller.Move(move * speed * Time.deltaTime);

        if(jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // falling
        velocity.y += gravity * Time.deltaTime;

        // executing jump if was pressed
        controller.Move(velocity * Time.deltaTime);
    }
}
