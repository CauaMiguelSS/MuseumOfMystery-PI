using UnityEngine;

public class NotebookInteract : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject painelNotebook;
    public GameObject interactionText;
    public FirstPersonController playerController;

    [Header("Settings")]
    public float distanciaInteracao = 3f;

    public bool IsNotebookOpen => aberto;

    private bool aberto;

    private void Start()
    {
        painelNotebook.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (aberto)
            return;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            distanciaInteracao))
        {
            if (hit.transform == transform &&
                Input.GetKeyDown(KeyCode.E))
            {
                AbrirNotebook();
            }
        }
    }

    public void AbrirNotebook()
    {
        aberto = true;
        painelNotebook.SetActive(true);

        interactionText?.SetActive(false);

        playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FecharNotebook()
    {
        aberto = false;
        painelNotebook.SetActive(false);

        playerController.enabled = true;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}