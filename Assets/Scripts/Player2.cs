using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementArrowKeys : MonoBehaviour {
    public Camera playerCamera;
    public bool isTeleporting = false;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    private float def_walk_speed, def_run_speed;

    public float jumpPower = 7f;
    public float gravity = 0f;
    public float lookSpeed = 100f; // ubrzano jer numpad nije analogni
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;  // za pitch (vertikalnu rotaciju)
    private float rotationY = 0f;  // za yaw (horizontalnu rotaciju) – koristićemo samo za kameru
    private CharacterController characterController;

    private bool canMove = true;

    void Start() {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        def_walk_speed = walkSpeed;
        def_run_speed = runSpeed;
    }

    void Update() {
        if (!GlobalState.canMove)
        {
            return;
        }
        var keyboard = Keyboard.current;

        if (isTeleporting) return;

        // Koristimo orijentaciju kamere za kretanje.
        // Uzimamo horizontalnu komponentu forward i right vektora kamere (bez uticaja y komponente)
        Vector3 camForward = playerCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = playerCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        bool isRunning = keyboard.rightShiftKey.isPressed;
        bool isJumping = keyboard.numpad0Key.isPressed;
        bool isCrouching = keyboard.rightCtrlKey.isPressed;

        float moveForward = 0f;
        float moveRight = 0f;

        if (keyboard.upArrowKey.isPressed) moveForward += 1f;
        if (keyboard.downArrowKey.isPressed) moveForward -= 1f;
        if (keyboard.rightArrowKey.isPressed) moveRight += 1f;
        if (keyboard.leftArrowKey.isPressed) moveRight -= 1f;

        float speed = canMove ? (isRunning ? runSpeed : walkSpeed) : 0f;
        float movementDirectionY = moveDirection.y; // zadržava vertikalnu komponentu zbog gravitacije i skoka

        // Kretanje se sada računa u odnosu na pravac kamere
        Vector3 move = (camForward * moveForward + camRight * moveRight) * speed;
        move.y = movementDirectionY;
        moveDirection = move;

        if (isJumping && canMove && characterController.isGrounded) {
            moveDirection.y = jumpPower;
        }
        else {
            moveDirection.y = movementDirectionY;
        }

        /*if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }*/

        if (characterController.isGrounded) {
            moveDirection.y = 0f;
        }



        if (isCrouching && canMove) {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else {
            characterController.height = defaultHeight;
            walkSpeed = def_walk_speed;
            runSpeed = def_run_speed;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove) {
            float lookHorizontal = 0f;
            float lookVertical = 0f;

            // Numpad 4 i 6 kontrolišu horizontalnu rotaciju (yaw)
            if (keyboard.numpad4Key.isPressed) lookHorizontal = -1f;
            if (keyboard.numpad6Key.isPressed) lookHorizontal = 1f;

            // Numpad 8 i 2 kontrolišu vertikalnu rotaciju (pitch)
            if (keyboard.numpad8Key.isPressed) lookVertical = 1f;
            if (keyboard.numpad2Key.isPressed) lookVertical = -1f;

            // Ažuriramo pitch (vertikalnu rotaciju) i ograničavamo je
            rotationX += lookVertical * lookSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Akumuliramo horizontalnu (yaw) rotaciju
            rotationY += lookHorizontal * lookSpeed * Time.deltaTime;

            // Primeni celu rotaciju samo na kameru ("glavu")
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}