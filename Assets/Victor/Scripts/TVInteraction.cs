using System.Collections;
using UnityEngine;

public class TVInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private Rigidbody playerRb;

    [Header("TV UI")]
    [Tooltip("Texto que aparece quando o player pode interagir com a TV.")]
    [SerializeField] private GameObject interactionText;

    [Header("Player Hand")]
    [Tooltip("Objeto do item que fica na mão do player.")]
    [SerializeField] private GameObject handItem;

    [Header("Flashlight")]
    [Tooltip("Objeto da lanterna. Pode ser a luz ou o GameObject inteiro.")]
    [SerializeField] private GameObject flashlight;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 2f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private bool watchingTV;
    private bool transitioning;

    private bool tvAvailable = false;

    private bool flashlightWasOn;

    private void Update()
    {
        if (watchingTV &&
            !transitioning &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTV();
        }
    }

    public void EnterTV()
    {
        if (!tvAvailable)
        {
            Debug.Log(
                "A TV ainda não está disponível. Insira uma fita VHS."
            );

            return;
        }

        if (watchingTV || transitioning)
        {
            return;
        }

        watchingTV = true;
        transitioning = true;

        if (playerCamera != null)
        {
            originalPos = playerCamera.position;
            originalRot = playerCamera.rotation;
        }

        if (playerController != null)
        {
            playerController.playerCanMove = false;
            playerController.cameraCanMove = false;
        }
        else
        {
            Debug.LogWarning(
                "TVInteraction: PlayerController não foi configurado."
            );
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (interactionText != null)
        {
            interactionText.SetActive(false);
        }

        if (handItem != null)
        {
            handItem.SetActive(false);
        }

        if (flashlight != null)
        {
            flashlightWasOn = flashlight.activeSelf;
            flashlight.SetActive(false);
        }

        Outline outline = GetComponentInChildren<Outline>(true);

        if (outline != null)
        {
            outline.enabled = false;
        }

        StartCoroutine(MoveCameraToTV());
    }

    public void ExitTV()
    {
        if (!watchingTV || transitioning)
        {
            return;
        }

        transitioning = true;

        StartCoroutine(ReturnToPlayer());
    }

    private IEnumerator MoveCameraToTV()
    {
        if (playerCamera == null)
        {
            Debug.LogError(
                "TVInteraction: Player Camera não foi configurada!"
            );

            transitioning = false;
            watchingTV = false;

            yield break;
        }

        if (cameraPoint == null)
        {
            Debug.LogError(
                "TVInteraction: Camera Point não foi configurado!"
            );

            transitioning = false;
            watchingTV = false;

            yield break;
        }

        yield return MoveCamera(
            cameraPoint.position,
            cameraPoint.rotation
        );

        transitioning = false;
    }

    private IEnumerator ReturnToPlayer()
    {
        if (playerCamera != null)
        {
            yield return MoveCamera(
                originalPos,
                originalRot
            );
        }

        if (playerController != null)
        {
            playerController.playerCanMove = true;
            playerController.cameraCanMove = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (handItem != null)
        {
            handItem.SetActive(true);
        }

        if (flashlight != null)
        {
            flashlight.SetActive(flashlightWasOn);
        }

        watchingTV = false;
        transitioning = false;
    }

    private IEnumerator MoveCamera(
        Vector3 targetPos,
        Quaternion targetRot
    )
    {
        if (playerCamera == null)
        {
            yield break;
        }

        float t = 0f;

        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;

            playerCamera.position = Vector3.Lerp(
                startPos,
                targetPos,
                t
            );

            playerCamera.rotation = Quaternion.Slerp(
                startRot,
                targetRot,
                t
            );

            yield return null;
        }

        playerCamera.position = targetPos;
        playerCamera.rotation = targetRot;
    }

    public void SetTVAvailable(bool available)
    {
        tvAvailable = available;

        if (available)
        {
            Debug.Log("TV agora está disponível.");
        }
        else
        {
            Debug.Log("TV agora está bloqueada.");
        }
    }

    public bool IsTVAvailable()
    {
        return tvAvailable;
    }
}