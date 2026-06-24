using UnityEngine;
using TMPro;

public class InteractableNote : MonoBehaviour
{
    public GameObject notePanel;
    public TextMeshProUGUI noteTextUI;

    [TextArea]
    public string noteText;

    public FirstPersonController playerController;

    public void Interact()
    {
        notePanel.SetActive(true);
        noteTextUI.text = noteText;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.cameraCanMove = false;
        playerController.playerCanMove = false;

        Time.timeScale = 0f;
    }

    void Update()
    {
        if (notePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            notePanel.SetActive(false);

            playerController.cameraCanMove = true;
            playerController.playerCanMove = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
        }
    }
}