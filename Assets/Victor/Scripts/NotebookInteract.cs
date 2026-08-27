using UnityEngine;

public class NotebookInteract : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject painelNotebook;
    public GameObject interactionText;

    public FirstPersonController playerController;

    public float distanciaInteracao = 3f;

    private bool aberto = false;

    void Start()
    {
        painelNotebook.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (aberto) return;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            distanciaInteracao))
        {
            if (hit.transform == transform && Input.GetKeyDown(KeyCode.E))
            {
                AbrirNotebook();
            }
        }
    }

    public void AbrirNotebook()
    {
        aberto = true;

        painelNotebook.SetActive(true);

        if (interactionText)
            interactionText.SetActive(false);

        // Desativa o controle do jogador
        playerController.enabled = false;

        // Libera o mouse para clicar no notebook
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FecharNotebook()
    {
        aberto = false;

        painelNotebook.SetActive(false);

        // Reativa o controle do jogador
        playerController.enabled = true;

        // Garante que o jogo não fique pausado
        Time.timeScale = 1f;

        // Volta a prender o mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}