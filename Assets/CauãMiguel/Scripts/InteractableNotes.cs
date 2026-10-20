
using UnityEngine;
using TMPro;

public class InteractableNote : MonoBehaviour
{
    [Header("References")]
    public GameObject notePanel;
    public TextMeshProUGUI noteTextUI;
    public FirstPersonController playerController;
    public DialogueManager dialogueManager;

    [Header("Nota")]
    [TextArea]
    public string noteText;

    [Header("Dialogo apos fechar")]
    [TextArea(2, 4)]
    public string[] dialogueAfterClose;

    public static bool IsAnyNoteOpen { get; private set; }
    public static InteractableNote NotaAtual { get; private set; }

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

        NotaAtual = this;
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

        if (dialogueManager != null && dialogueAfterClose != null)
        {
            string[] falas = (string[])dialogueAfterClose.Clone();
            dialogueManager.IniciarDialogo(falas);
        }
        else
        {
            Debug.LogWarning(
                "DialogueManager ou falas nao configurados em: "
                + gameObject.name
            );
        }

        NotaAtual = null;
    }
}
