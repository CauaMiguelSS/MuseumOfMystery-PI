using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    private Vector3 originalScale;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;

        playerCamera.fieldOfView = fov;
    }

    private void Update()
    {
        #region Camera Look
        if (cameraCanMove)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.eulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }
        #endregion

        #region Jump
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
        #endregion

        #region Crouch
        if (enableCrouch && Input.GetKeyDown(crouchKey))
        {
            Crouch();
        }
        #endregion

        CheckGround();
    }

    private void FixedUpdate()
    {
        // 🔥 TRAVA TOTAL (TV MODE FIX)
        if (!playerCanMove)
        {
            rb.linearVelocity = Vector3.zero;
            isWalking = false;
            isSprinting = false;
            return;
        }

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 🔥 FIX BUG WALK (parênteses corretos)
        if ((input.x != 0 || input.z != 0) && isGrounded)
            isWalking = true;
        else
            isWalking = false;

        Vector3 targetVelocity = transform.TransformDirection(input) * walkSpeed;

        Vector3 velocity = rb.linearVelocity;
        Vector3 change = targetVelocity - velocity;

        change.x = Mathf.Clamp(change.x, -maxVelocityChange, maxVelocityChange);
        change.z = Mathf.Clamp(change.z, -maxVelocityChange, maxVelocityChange);
        change.y = 0;

        rb.AddForce(change, ForceMode.VelocityChange);
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
            walkSpeed /= speedReduction;
            isCrouched = false;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;
            isCrouched = true;
        }
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.down * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, 0.75f))
            isGrounded = true;
        else
            isGrounded = false;
    }
}