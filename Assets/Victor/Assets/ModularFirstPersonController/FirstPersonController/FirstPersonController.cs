using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    #region Camera Variables
    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    private float yaw = 0f;
    private float pitch = 0f;
    #endregion

    #region Movement Variables
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 8f;

    private float currentSpeed;

    private bool isWalking;
    private bool isSprinting;
    private bool isCrouched;
    #endregion

    #region Jump
    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    private bool isGrounded;
    #endregion

    #region Crouch
    public bool enableCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = 0.75f;
    public float speedReduction = 0.5f;

    private Vector3 originalScale;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            enabled = false;
            return;
        }

        originalScale = transform.localScale;
        currentSpeed = walkSpeed;

        if (playerCamera != null)
            playerCamera.fieldOfView = fov;

        yaw = transform.eulerAngles.y;

        LockCursor();
    }

    private void Update()
    {
        if (cameraCanMove)
        {
            LockCursor();

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            yaw += mouseX;

            if (invertCamera)
                pitch += mouseY;
            else
                pitch -= mouseY;

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.eulerAngles = new Vector3(0f, yaw, 0f);

            if (playerCamera != null)
                playerCamera.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        if (playerCanMove)
        {
            if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
            {
                Jump();
            }

            if (enableCrouch && Input.GetKeyDown(crouchKey))
            {
                Crouch();
            }
        }

        if (playerCanMove)
        {
            if (Input.GetKey(sprintKey))
            {
                isSprinting = true;
                currentSpeed = sprintSpeed;
            }
            else
            {
                isSprinting = false;
                currentSpeed = walkSpeed;
            }
        }
        CheckGround();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (!playerCanMove)
        {
            rb.linearVelocity = Vector3.zero;
            isWalking = false;
            isSprinting = false;
            return;
        }

        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        isWalking = (input.x != 0 || input.z != 0) && isGrounded;

        Vector3 targetVelocity = transform.TransformDirection(input) * currentSpeed;

        Vector3 velocity = rb.linearVelocity;

        Vector3 change = targetVelocity - velocity;

        change.x = Mathf.Clamp(
            change.x,
            -maxVelocityChange,
            maxVelocityChange
        );

        change.z = Mathf.Clamp(
            change.z,
            -maxVelocityChange,
            maxVelocityChange
        );

        change.y = 0;

        rb.AddForce(change, ForceMode.VelocityChange);

        // Mantém o jogador mais "grudado" ao chão
        if (isGrounded)
        {
            rb.AddForce(Vector3.down * 5f, ForceMode.Force);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;
    }

    private void Crouch()
    {
        if (isCrouched)
        {
            transform.localScale = originalScale;
            currentSpeed /= speedReduction;
            walkSpeed /= speedReduction;
            isCrouched = false;
        }
        else
        {
            transform.localScale = new Vector3(
                originalScale.x,
                crouchHeight,
                originalScale.z
            );

            currentSpeed *= speedReduction;
            walkSpeed *= speedReduction;
            isCrouched = true;
        }
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.down * 0.5f;

        isGrounded = Physics.Raycast(origin, Vector3.down, 0.75f);
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}