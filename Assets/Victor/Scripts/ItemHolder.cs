using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform holdPoint;

    [Header("Settings")]
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private float dropDistance = 1f;

    private PickableItem heldItem;

    public PickableItem HeldItem => heldItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                TryPickup();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    private void TryPickup()
    {
        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            pickupDistance))
        {
            PickableItem item =
                hit.collider.GetComponentInParent<PickableItem>();

            if (item == null)
                return;

            heldItem = item;

            heldItem.PickUp(holdPoint);
        }
    }

    private void DropItem()
    {
        if (heldItem == null)
            return;

        Vector3 dropPos =
            playerCamera.transform.position +
            playerCamera.transform.forward * dropDistance;

        heldItem.Drop(dropPos);

        heldItem = null;
    }

    public bool IsHolding(string itemID)
    {
        return heldItem != null &&
               heldItem.ItemID == itemID;
    }

    public void ConsumeHeldItem()
    {
        if (heldItem == null)
            return;

        Destroy(heldItem.gameObject);

        heldItem = null;
    }
}