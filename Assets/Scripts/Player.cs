using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;             // reference to the camera (typically attached to "Head")
    public Transform headTransform;         // reference to the "Head" GameObject
    public bool isTeleporting = false;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float jumpPower = 7f;
    public float gravity = 0f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 1f;

    private float defWalkSpeed, defRunSpeed;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;  // For vertical (pitch) rotation.
    private float rotationY = 0f;  // For horizontal (yaw) rotation.

    private CharacterController characterController;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defWalkSpeed = walkSpeed;
        defRunSpeed = runSpeed;

        // Inicijalizacija rotacija prema trenutnom stanju kamere
        rotationY = headTransform.localEulerAngles.y;
        rotationX = headTransform.localEulerAngles.x;
    }

    void Update()
    {
        if (!GlobalState.canMove) {
            return;
        }
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (isTeleporting) return;

        // Umesto da se koristi transform igrača, koristimo orijentaciju kamere za kretanje.
        // Dobijamo horizontalnu komponentu kamere za napred/nazad.
        Vector3 camForward = playerCamera.transform.forward;
        camForward.y = 0; // ignorisemo vertikalnu komponentu
        camForward.Normalize();

        // Dobijamo horizontalnu komponentu kamere za levo/desno.
        Vector3 camRight = playerCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        bool isRunning = keyboard.leftShiftKey.isPressed;
        bool isJumping = keyboard.spaceKey.isPressed;
        bool isCrouching = keyboard.leftCtrlKey.isPressed;

        float moveForward = 0f;
        float moveRight = 0f;

        // Mapa tastera: W - napred, S - nazad, D - desno, A - levo
        if (keyboard.wKey.isPressed) moveForward += 1f;
        if (keyboard.sKey.isPressed) moveForward -= 1f;
        if (keyboard.dKey.isPressed) moveRight += 1f;
        if (keyboard.aKey.isPressed) moveRight -= 1f;

        // Odredi brzinu na osnovu stanja (trčanje ili hodanje)
        float speed = canMove ? (isRunning ? runSpeed : walkSpeed) : 0;

        // Izračunaj horizontalni pokret u odnosu na kameru
        Vector3 horizontalMove = (camForward * moveForward + camRight * moveRight) * speed;

        // Zadrži trenutno vertikalno kretanje (za jump i gravitaciju)
        horizontalMove.y = moveDirection.y;
        moveDirection = horizontalMove;

        if (isJumping && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        /*if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }*/

        if (characterController.isGrounded)
        {
            moveDirection.y = 0;
        }
        else
        {
            // Ako ipak želiš određenu silu kad igrač nije na zemlji, ovde možeš prilagoditi logiku ili je potpuno ukloniti
            // moveDirection.y -= gravity * Time.deltaTime;
        }


        if (isCrouching && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = defWalkSpeed;
            runSpeed = defRunSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            // Preuzimanje kretanja mišem.
            float mouseY = mouse.delta.y.ReadValue() * lookSpeed * 0.1f;
            float mouseX = mouse.delta.x.ReadValue() * lookSpeed * 0.1f;

            // Ažuriramo vertikalnu rotaciju (pitch) i ograničavamo ga.
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Ažuriramo horizontalnu rotaciju (yaw)
            rotationY += mouseX;

            // Postavljamo rotaciju na glavu (kamere) tako da se kamera okrene nezavisno.
            headTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}