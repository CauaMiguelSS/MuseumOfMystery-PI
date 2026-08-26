using System.Collections;
using UnityEngine;
using TMPro;

public class PaintingInteraction : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Player")]
    [SerializeField] private FirstPersonController playerController;

    [Header("Painel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject interactionText;

    [Header("Vidro")]
    [SerializeField] private Transform glass;
    [SerializeField] private float openHeight = 1f;
    [SerializeField] private float openSpeed = 2f;

    private Vector3 glassStartPosition;
    private bool panelOpen;
    private bool answered;

    // Informa se o ESC foi usado para fechar o painel neste frame
    public bool EscUsedToClosePanel { get; private set; }

    public bool IsPanelOpen => panelOpen;

    private void Start()
    {
        panel.SetActive(false);

        if (interactionText != null)
            interactionText.SetActive(false);

        if (glass != null)
            glassStartPosition = glass.position;
    }

    private void Update()
    {
        // Reseta a informação a cada frame
        EscUsedToClosePanel = false;

        // ========================================
        // PAINEL ABERTO
        // ========================================

        if (panelOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EscUsedToClosePanel = true;
                ClosePanel();
            }

            return;
        }

        if (answered)
            return;

        // ========================================
        // RAYCAST
        // ========================================

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.transform == transform)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenPanel();
                }
            }
        }
    }

    private void OpenPanel()
    {
        panel.SetActive(true);
        panelOpen = true;

        if (interactionText != null)
            interactionText.SetActive(false);

        playerController.playerCanMove = false;
        playerController.cameraCanMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void CheckAnswer()
    {
        string answer = inputField.text.Trim();

        if (answer.ToLower() == "virgem do fuso")
        {
            answered = true;

            ClosePanel();

            StartCoroutine(OpenGlass());
        }
        else
        {
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
        panelOpen = false;

        playerController.playerCanMove = true;
        playerController.cameraCanMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (interactionText != null)
            interactionText.SetActive(false);
    }

    private IEnumerator OpenGlass()
    {
        Vector3 targetPosition =
            glassStartPosition + Vector3.up * openHeight;

        while (Vector3.Distance(glass.position, targetPosition) > 0.01f)
        {
            glass.position = Vector3.MoveTowards(
                glass.position,
                targetPosition,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        glass.position = targetPosition;
    }
}