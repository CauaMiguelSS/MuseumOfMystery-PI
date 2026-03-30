using UnityEngine;
using UnityEngine.UI;
using TMPro; // remove if not using TextMeshPro

public class NoteUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject notePanel;        // panel to show/hide
    public GameObject promptObject;     // small "Press E" UI element
    public TMP_Text titleText;          // TextMeshPro title text
    public TMP_Text bodyText;           // TextMeshPro body text
    public Image noteImageUI;           // optional image (can be null)

    [Header("Settings")]
    public bool lockCursorWhenOpen = true;

    public bool IsOpen { get; private set; } = false;

    void Start()
    {
        if (notePanel) notePanel.SetActive(false);
        if (promptObject) promptObject.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        if (promptObject) promptObject.SetActive(show);
    }

    public void OpenNote(string title, string body, Sprite image = null)
    {
        if (notePanel == null) return;
        titleText.text = title;
        bodyText.text = body;
        if (noteImageUI != null)
        {
            if (image != null)
            {
                noteImageUI.sprite = image;
                noteImageUI.gameObject.SetActive(true);
            }
            else noteImageUI.gameObject.SetActive(false);
        }

        notePanel.SetActive(true);
        IsOpen = true;

        if (lockCursorWhenOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Optionally disable player movement script here
    }

    public void CloseNote()
    {
        if (notePanel == null) return;
        notePanel.SetActive(false);
        IsOpen = false;

        if (lockCursorWhenOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Re-enable player movement if you disabled it
    }
}


