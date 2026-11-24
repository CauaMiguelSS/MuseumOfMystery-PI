
using UnityEngine;

public class NotebookInteract : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject painelNotebook;
    public GameObject interactionText;
    public FirstPersonController playerController;
    public DialogueManager dialogueManager;

    [Header("Dialogue")]
    [TextArea(2, 4)]
    public string[] dialogueOnClose;

    [Header("Settings")]
    public float distanciaInteracao = 3f;

    public bool IsNotebookOpen => aberto;

    private bool aberto;

    private static GameObject currentInteractionText;
    private static Object currentTarget;

    private void Start()
    {
        painelNotebook?.SetActive(false);
        interactionText?.SetActive(false);
    }

    private void Update()
    {
        if (aberto || playerCamera == null || DialogueManager.IsPlaying)
        {
            HideInteraction();
            return;
        }

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            distanciaInteracao))
        {
            NotebookInteract notebook =
                hit.collider.GetComponentInParent<NotebookInteract>();

            if (notebook == this)
            {
                ShowInteraction();

                if (Input.GetKeyDown(KeyCode.E))
                    AbrirNotebook();

                return;
            }
        }

        HideInteraction();
    }

    private void ShowInteraction()
    {
        currentInteractionText = interactionText;
        currentTarget = this;

        interactionText?.SetActive(true);
    }

    private void HideInteraction()
    {
        if (currentTarget == this)
        {
            interactionText?.SetActive(false);
            currentTarget = null;
            currentInteractionText = null;
        }
    }

    public void AbrirNotebook()
    {
        if (aberto)
            return;

        aberto = true;

        painelNotebook?.SetActive(true);
        HideInteraction();

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FecharNotebook()
    {
        if (!aberto)
            return;

        aberto = false;

        painelNotebook?.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (dialogueManager != null)
            dialogueManager.IniciarDialogo(dialogueOnClose);
    }

    private void OnDisable()
    {
        HideInteraction();
    }
}
