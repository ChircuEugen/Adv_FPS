using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseMovement : MonoBehaviour
{
    private PlayerInput input;
    private InputAction lookAction;
    private Vector2 mouseInput;

    public float mouseSensitivity = 5f;

    private float xRotation = 0;
    private float yRotation = 0;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        input = GetComponent<PlayerInput>();
        lookAction = input.actions["Look"];
    }

    private void Update()
    {
        mouseInput = lookAction.ReadValue<Vector2>();

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
