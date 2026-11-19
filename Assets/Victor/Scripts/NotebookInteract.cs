
using UnityEngine;

public class NotebookInteract : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject painelNotebook;
    public GameObject interactionText;
    public FirstPersonController playerController;
    public DialogueManager dialogueManager;

    [Header("Dialogo ao fechar")]
    [TextArea(2, 4)]
    public string[] dialogueOnClose;

    [Header("Settings")]
    public float distanciaInteracao = 3f;

    public bool IsNotebookOpen => aberto;

    private bool aberto;

    private void Start()
    {
        if (painelNotebook != null)
            painelNotebook.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (aberto)
            return;

        if (playerCamera == null)
            return;

        if (DialogueManager.IsPlaying)
        {
            interactionText?.SetActive(false);
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
                interactionText?.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    AbrirNotebook();
                }

                return;
            }
        }

        interactionText?.SetActive(false);
    }

    public void AbrirNotebook()
    {
        if (aberto)
            return;

        aberto = true;

        if (painelNotebook != null)
            painelNotebook.SetActive(true);

        interactionText?.SetActive(false);

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

        if (painelNotebook != null)
            painelNotebook.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (dialogueManager != null)
        {
            dialogueManager.IniciarDialogo(dialogueOnClose);
        }
        else
        {
            Debug.LogWarning(
                "DialogueManager nao configurado no notebook: "
                + gameObject.name
            );
        }
    }
}
