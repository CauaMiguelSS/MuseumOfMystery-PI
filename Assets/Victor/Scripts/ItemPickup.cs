
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;
    [HideInInspector] public Outline outline;

    public bool isHeld;

    [Header("ID do Item")]
    public string itemID;

    [Header("Dialogos")]
    [TextArea(2, 4)]
    public string[] dialogueOnPickup;

    [TextArea(2, 4)]
    public string[] dialogueOnDrop;

    [TextArea(2, 4)]
    public string[] dialogueOnPlace;

    private void Awake()
    {
        // Rigidbody
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            rb = GetComponentInChildren<Rigidbody>(true);

        // Collider
        col = GetComponent<Collider>();

        if (col == null)
            col = GetComponentInChildren<Collider>(true);

        // Outline
        outline = GetComponent<Outline>();

        if (outline == null)
            outline = GetComponentInChildren<Outline>(true);

        if (outline == null)
            outline = GetComponentInParent<Outline>();
    }
}
