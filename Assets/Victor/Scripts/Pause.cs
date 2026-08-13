using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    public MonoBehaviour playerCamera;

    bool paused;

    void Start()
    {
        pausePanel.SetActive(false);

        // Garante que o áudio comece funcionando
        AudioListener.pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            pausePanel.SetActive(paused);

            // Pausa/despausa o jogo
            Time.timeScale = paused ? 0 : 1;

            // Pausa/despausa TODOS os sons do jogo
            AudioListener.pause = paused;

            Cursor.lockState = paused
                ? CursorLockMode.None
                : CursorLockMode.Locked;

            Cursor.visible = paused;

            playerCamera.enabled = !paused;
        }
    }
}