
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
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        outline = GetComponentInParent<Outline>();
    }
}
