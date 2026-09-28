using UnityEngine;
using TMPro;

public class InteractableNote : MonoBehaviour
{
    [Header("References")]
    public GameObject notePanel;
    public TextMeshProUGUI noteTextUI;
    public FirstPersonController playerController;

    [Header("Nota")]
    [TextArea]
    public string noteText;

    public static bool IsAnyNoteOpen { get; private set; }

    public void Interact()
    {
        notePanel.SetActive(true);
        noteTextUI.text = noteText;

        IsAnyNoteOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.cameraCanMove = false;
        playerController.playerCanMove = false;

        Time.timeScale = 0f;
    }

    public void FecharNota()
    {
        notePanel.SetActive(false);

        IsAnyNoteOpen = false;

        playerController.cameraCanMove = true;
        playerController.playerCanMove = true;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}