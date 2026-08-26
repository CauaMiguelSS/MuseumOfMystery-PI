using UnityEngine;

public class OutlineDetector : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private Camera playerCamera;

    [Header("Painel do Quadro")]
    [SerializeField] private PaintingInteraction paintingInteraction;

    private Outline currentOutline;
    private RaycastHit currentHit;
    private bool hasHit;

    void Start()
    {
        if (interactionText != null)
            interactionText.SetActive(false);
    }

    void Update()
    {
        // ========================================
        // PAINEL DO QUADRO ABERTO
        // ========================================

        if (paintingInteraction != null && paintingInteraction.IsPanelOpen)
        {
            ClearInteraction();
            return;
        }

        // ========================================
        // RAYCAST
        // ========================================

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        hasHit = Physics.Raycast(ray, out currentHit, distance);

        if (!hasHit)
        {
            ClearInteraction();
            return;
        }

        // ========================================
        // PROCURA OUTLINE
        // ========================================

        Outline outline =
            currentHit.collider.GetComponentInParent<Outline>();

        // ========================================
        // ITEM SENDO SEGURADO
        // ========================================

        ItemPickup item =
            currentHit.collider.GetComponentInParent<ItemPickup>();

        if (item != null && item.isHeld)
        {
            outline = null;
        }

        // ========================================
        // ATUALIZA OUTLINE
        // ========================================

        if (outline != currentOutline)
        {
            if (currentOutline != null)
                currentOutline.enabled = false;

            currentOutline = outline;

            if (currentOutline != null)
                currentOutline.enabled = true;
        }

        // ========================================
        // TEXTO
        // ========================================

        if (interactionText != null)
        {
            interactionText.SetActive(currentOutline != null);
        }

        // ========================================
        // INTERAÇÃO
        // ========================================

        HandleInteraction();
    }

    private void ClearInteraction()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }

        if (interactionText != null)
            interactionText.SetActive(false);
    }

    private void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (!hasHit)
            return;

        // ========================================
        // TV
        // ========================================

        TVInteraction tv =
            currentHit.collider.GetComponentInParent<TVInteraction>();

        if (tv != null)
        {
            tv.EnterTV();
            return;
        }

        // ========================================
        // CADEADO
        // ========================================

        DoorLock lockObject =
            currentHit.collider.GetComponentInParent<DoorLock>();

        if (lockObject != null)
        {
            PlayerPickup player =
                GetComponent<PlayerPickup>();

            if (player != null)
            {
                lockObject.TryUnlock(player);
            }

            return;
        }

        // ========================================
        // NOTA
        // ========================================

        InteractableNote note =
            currentHit.collider.GetComponentInParent<InteractableNote>();

        if (note != null)
        {
            note.Interact();
            return;
        }
    }
}