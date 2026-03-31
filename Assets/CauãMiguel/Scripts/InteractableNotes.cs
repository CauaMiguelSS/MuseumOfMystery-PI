using UnityEngine;
using TMPro;

public class InteractableNote : MonoBehaviour
{
    public GameObject notePanel;
    public TextMeshProUGUI noteTextUI;

    [TextArea]
    public string noteText;

    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            notePanel.SetActive(true);
            noteTextUI.text = noteText;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f; // pausa o jogo 😈
        }

        if (notePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            notePanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f; // volta ao normal
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
