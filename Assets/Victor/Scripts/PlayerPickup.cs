
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Camera playerCamera;

    [Header("Dialogo")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Pickup Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    private ItemPickup heldItem;

    private void Update()
    {
        if (DialogueManager.IsPlaying || InteractableNote.IsAnyNoteOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem != null)
            {
                if (TryInsertVHS())
                    return;

                if (TryPlaceOnExpositor())
                    return;
            }

            if (heldItem == null)
                TryPickup();
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            Drop();
        }
    }

    private void TryPickup()
    {
        if (playerCamera == null || holdPoint == null)
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, distance))
            return;

        ItemPickup item = hit.collider.GetComponent<ItemPickup>();

        if (item == null)
            return;

        heldItem = item;
        heldItem.isHeld = true;

        heldItem.transform.SetParent(holdPoint);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        heldItem.rb.isKinematic = true;
        heldItem.col.enabled = false;

        if (heldItem.outline != null)
            heldItem.outline.enabled = false;

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        IniciarDialogo(heldItem.dialogueOnPickup);

        Debug.Log("Pegou: " + heldItem.itemID);
    }

    private bool TryInsertVHS()
    {
        if (playerCamera == null)
            return false;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, distance))
            return false;

        VHSPlayer vhsPlayer = hit.collider.GetComponent<VHSPlayer>();

        if (vhsPlayer == null)
            return false;

        ItemPickup item = heldItem;
        string[] falas = item.dialogueOnPlace;

        if (vhsPlayer.TryInsertVHS(item))
        {
            item.isHeld = false;
            heldItem = null;

            IniciarDialogo(falas);

            Debug.Log("VHS colocado no aparelho.");
            return true;
        }

        return false;
    }

    private bool TryPlaceOnExpositor()
    {
        if (playerCamera == null)
            return false;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, distance))
            return false;

        Expositor expositor = hit.collider.GetComponent<Expositor>();

        if (expositor == null)
            return false;

        ItemPickup item = heldItem;
        string[] falas = item.dialogueOnPlace;

        expositor.TentarColocar(item);

        if (!item.isHeld)
        {
            heldItem = null;
            IniciarDialogo(falas);
        }

        return true;
    }

    private void Drop()
    {
        ItemPickup item = heldItem;
        string[] falas = item.dialogueOnDrop;

        item.transform.SetParent(null);
        item.isHeld = false;

        item.rb.isKinematic = false;
        item.col.enabled = true;

        heldItem = null;

        IniciarDialogo(falas);

        Debug.Log("Item largado.");
    }

    private void IniciarDialogo(string[] falas)
    {
        if (dialogueManager != null)
            dialogueManager.IniciarDialogo(falas);
    }

    public ItemPickup GetHeldItem()
    {
        return heldItem;
    }
}
