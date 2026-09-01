using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("Pause")]
    public GameObject pausePanel;
    public MonoBehaviour playerCamera;

    [Header("Painel do Quadro")]
    public PaintingInteraction paintingInteraction;

    [Header("Projetor / TV")]
    public TVInteraction tvInteraction;

    [Header("Notebook")]
    public NotebookInteract notebookInteract;

    private bool paused;

    private void Start()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void Update()
    {
        if (paintingInteraction != null &&
            paintingInteraction.EscUsedToClosePanel)
            return;

        if (tvInteraction != null &&
            tvInteraction.EscUsedToExitTV)
            return;

        if (notebookInteract != null &&
            notebookInteract.IsNotebookOpen)
            return;

        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        paused = !paused;

        pausePanel.SetActive(paused);

        Time.timeScale = paused ? 0f : 1f;
        AudioListener.pause = paused;

        Cursor.lockState = paused
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = paused;

        if (playerCamera != null)
            playerCamera.enabled = !paused;
    }
}