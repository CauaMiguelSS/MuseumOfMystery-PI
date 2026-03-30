using UnityEngine;

public class InteractableNote : MonoBehaviour
{
    [Tooltip("Assign the NoteUIManager in scene")]
    public NoteUIManager noteUI;

    [Tooltip("Distance check fallback (if not using trigger)")]
    public float interactionDistance = 2f;

    [Header("Note content")]
    public string noteTitle = "Nota";
    [TextArea] public string noteBody = "Aqui vai o texto da nota...";
    public Sprite noteImage; // optional

    bool playerNearby = false;
    Transform player;

    void Start()
    {
        if (noteUI == null) Debug.LogWarning("NoteUIManager not assigned on " + name);
        GameObject p = GameObject.FindWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (!playerNearby && player != null)
        {
            if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
                playerNearby = true;
        }

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle panel
            if (noteUI != null)
            {
                if (noteUI.IsOpen)
                    noteUI.CloseNote();
                else
                    noteUI.OpenNote(noteTitle, noteBody, noteImage);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (noteUI) noteUI.ShowPrompt(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (noteUI)
            {
                noteUI.ShowPrompt(false);
                // also close if open
                if (noteUI.IsOpen) noteUI.CloseNote();
            }
        }
    }
}
