using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    public MonoBehaviour playerCamera;

    [Header("Painel do Quadro")]
    public PaintingInteraction paintingInteraction;

    private bool paused;

    void Start()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Update()
    {
        // ========================================
        // ESC USADO PARA FECHAR O PAINEL
        // ========================================

        if (paintingInteraction != null &&
            paintingInteraction.EscUsedToClosePanel)
        {
            return;
        }

        // ========================================
        // PAUSE NORMAL
        // ========================================

        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
}