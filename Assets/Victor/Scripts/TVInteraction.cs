using System.Collections;
using UnityEngine;

public class TVInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private Rigidbody playerRb;

    [Header("UI")]
    [SerializeField] private GameObject interactionText;

    [Header("Player")]
    [SerializeField] private GameObject handItem;
    [SerializeField] private GameObject flashlight;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 2f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private bool watchingTV;
    private bool transitioning;
    private bool tvAvailable;
    private bool flashlightWasOn;

    public bool IsTVOpen => watchingTV;
    public bool EscUsedToExitTV { get; private set; }

    private void Update()
    {
        EscUsedToExitTV = false;

        if (!watchingTV) return;

        interactionText?.SetActive(false);

        if (!transitioning && Input.GetKeyDown(KeyCode.Escape))
        {
            EscUsedToExitTV = true;
            ExitTV();
        }
    }

    public void EnterTV()
    {
        if (!tvAvailable || watchingTV || transitioning) return;

        watchingTV = transitioning = true;

        if (playerCamera != null)
        {
            originalPos = playerCamera.position;
            originalRot = playerCamera.rotation;
        }

        interactionText?.SetActive(false);

        if (playerController != null)
        {
            playerController.playerCanMove = false;
            playerController.cameraCanMove = false;
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        handItem?.SetActive(false);

        if (flashlight != null)
        {
            flashlightWasOn = flashlight.activeSelf;
            flashlight.SetActive(false);
        }

        Outline outline = GetComponentInChildren<Outline>(true);
        if (outline != null)
            outline.enabled = false;

        StartCoroutine(MoveCamera(cameraPoint.position, cameraPoint.rotation, true));
    }

    public void ExitTV()
    {
        if (!watchingTV || transitioning) return;

        transitioning = true;
        interactionText?.SetActive(false);

        StartCoroutine(MoveCamera(originalPos, originalRot, false));
    }

    private IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, bool entering)
    {
        if (playerCamera == null || (entering && cameraPoint == null))
        {
            watchingTV = transitioning = false;
            yield break;
        }

        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;

            playerCamera.position = Vector3.Lerp(startPos, targetPos, t);
            playerCamera.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        playerCamera.position = targetPos;
        playerCamera.rotation = targetRot;

        if (!entering)
        {
            playerController.playerCanMove = true;
            playerController.cameraCanMove = true;

            handItem?.SetActive(true);

            if (flashlight != null)
                flashlight.SetActive(flashlightWasOn);

            watchingTV = false;
        }

        transitioning = false;
    }

    public void SetTVAvailable(bool available)
    {
        tvAvailable = available;
        Debug.Log(available ? "TV agora está disponível." : "TV agora está bloqueada.");
    }

    public bool IsTVAvailable() => tvAvailable;
}