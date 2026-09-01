using UnityEngine;

public class OutlineDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private Camera playerCamera;

    [Header("Interações")]
    [SerializeField] private PaintingInteraction paintingInteraction;
    [SerializeField] private TVInteraction tvInteraction;
    [SerializeField] private NotebookInteract notebookInteract;

    private Outline currentOutline;
    private RaycastHit currentHit;

    private void Start()
    {
        interactionText?.SetActive(false);
    }

    private void Update()
    {
        if (IsBlocked())
        {
            ClearInteraction();
            return;
        }

        if (!Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out currentHit,
            distance))
        {
            ClearInteraction();
            return;
        }

        Outline outline =
            currentHit.collider.GetComponentInParent<Outline>();

        ItemPickup item =
            currentHit.collider.GetComponentInParent<ItemPickup>();

        if (item != null && item.isHeld)
            outline = null;

        if (outline != currentOutline)
        {
            if (currentOutline != null)
                currentOutline.enabled = false;

            currentOutline = outline;

            if (currentOutline != null)
                currentOutline.enabled = true;
        }

        interactionText?.SetActive(currentOutline != null);

        HandleInteraction();
    }

    private bool IsBlocked()
    {
        return
            (tvInteraction != null && tvInteraction.IsTVOpen) ||
            (paintingInteraction != null && paintingInteraction.IsPanelOpen) ||
            (notebookInteract != null && notebookInteract.IsNotebookOpen);
    }

    private void ClearInteraction()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }

        interactionText?.SetActive(false);
    }

    private void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        TVInteraction tv =
            currentHit.collider.GetComponentInParent<TVInteraction>();

        if (tv != null)
        {
            tv.EnterTV();
            ClearInteraction();
            return;
        }

        DoorLock lockObject =
            currentHit.collider.GetComponentInParent<DoorLock>();

        if (lockObject != null)
        {
            PlayerPickup player =
                GetComponent<PlayerPickup>();

            if (player != null)
                lockObject.TryUnlock(player);

            return;
        }

        InteractableNote note =
            currentHit.collider.GetComponentInParent<InteractableNote>();

        note?.Interact();
    }
}