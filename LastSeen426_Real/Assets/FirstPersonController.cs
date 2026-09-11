using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("MOVIMENTO")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("VISUALE")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 85f;

    private CharacterController controller;
    private Rigidbody rb;

    private float verticalVelocity;
    private float cameraPitch;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError(
                "FirstPersonController: manca il Character Controller sul Player!"
            );
            enabled = false;
            return;
        }

        // Recupera o aggiunge il Rigidbody per permettere le collisioni con i Trigger
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true; // Impedisce alla fisica di interferire con il CharacterController

        if (cameraTransform == null)
        {
            Debug.LogError(
                "FirstPersonController: assegna la Main Camera al campo Camera Transform!"
            );
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Destra / sinistra:
        // ruota tutto il Player.
        transform.Rotate(Vector3.up * mouseX);

        // Su / giù:
        // ruota solamente la Camera.
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(
            cameraPitch,
            -maxLookAngle,
            maxLookAngle
        );

        cameraTransform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Movimento relativo alla direzione in cui guarda il Player.
        Vector3 direction =
            transform.right * horizontal +
            transform.forward * vertical;

        // Evita che diagonale + diagonale sia più veloce.
        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        // SHIFT = corsa
        float currentSpeed = Input.GetKey(KeyCode.LeftShift)
            ? runSpeed
            : walkSpeed;

        Vector3 movement = direction * currentSpeed;

        // Gravità
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            // SPAZIO = salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;

        movement.y = verticalVelocity;

        // Movimento gestito dal Character Controller,
        // quindi il Player non attraversa i collider.
        controller.Move(movement * Time.deltaTime);
    }
}