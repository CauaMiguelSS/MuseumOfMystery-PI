
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
    [Tooltip("Objeto da lanterna. Pode ser a própria luz ou o GameObject inteiro.")]
    [SerializeField] private GameObject flashlight;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 2f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private bool watchingTV;
    private bool transitioning;

    // Guarda se a lanterna estava ligada antes de entrar na TV
    private bool flashlightWasOn;

    private void Update()
    {
        // Enquanto estiver assistindo à TV, ESC sai da tela
        if (watchingTV && !transitioning && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTV();
        }
    }

    public void EnterTV()
    {
        if (watchingTV || transitioning)
            return;

        watchingTV = true;
        transitioning = true;

        // Salva a posição e rotação original da câmera
        originalPos = playerCamera.position;
        originalRot = playerCamera.rotation;

        // Desativa movimento e câmera do player
        playerController.playerCanMove = false;
        playerController.cameraCanMove = false;

        // Zera a física do player
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // Mantém o cursor travado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Esconde o Interaction Text
        if (interactionText != null)
        {
            interactionText.SetActive(false);
        }

        // Esconde o item que está na mão
        if (handItem != null)
        {
            handItem.SetActive(false);
        }

        // Salva o estado da lanterna e desliga
        if (flashlight != null)
        {
            flashlightWasOn = flashlight.activeSelf;
            flashlight.SetActive(false);
        }

        // Desliga o outline da TV
        Outline outline = GetComponentInChildren<Outline>();

        if (outline != null)
        {
            outline.enabled = false;
        }

        StartCoroutine(MoveCameraToTV());
    }

    public void ExitTV()
    {
        if (!watchingTV || transitioning)
            return;

        transitioning = true;

        StartCoroutine(ReturnToPlayer());
    }

    private IEnumerator MoveCameraToTV()
    {
        yield return MoveCamera(cameraPoint.position, cameraPoint.rotation);

        transitioning = false;
    }

    private IEnumerator ReturnToPlayer()
    {
        // Volta a câmera para a posição original
        yield return MoveCamera(originalPos, originalRot);

        // Devolve o controle ao player
        playerController.playerCanMove = true;
        playerController.cameraCanMove = true;

        // Mantém o mouse travado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Mostra novamente o item da mão
        if (handItem != null)
        {
            handItem.SetActive(true);
        }

        // Restaura a lanterna ao estado anterior
        if (flashlight != null)
        {
            flashlight.SetActive(flashlightWasOn);
        }

        // Agora realmente saiu da TV
        watchingTV = false;
        transitioning = false;
    }

    private IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
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

        // Garante que termine exatamente no ponto
        playerCamera.position = targetPos;
        playerCamera.rotation = targetRot;
    }
}