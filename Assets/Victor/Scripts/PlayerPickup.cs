using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private Transform holdPoint;

    private ItemPickup heldItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem != null)
            {
                if (TryPlaceOnExpositor())
                {
                    return;
                }
            }

            if (heldItem == null)
            {
                TryPickup();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldItem != null)
            {
                Drop();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
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
        }
    }

    bool TryPlaceOnExpositor()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Expositor expositor = hit.collider.GetComponent<Expositor>();

            if (expositor == null)
                return false;

            expositor.TentarColocar(heldItem);

            if (!heldItem.isHeld)
            {
                heldItem = null;
            }

            return true;
        }

        return false;
    }

    void Drop()
    {
        heldItem.transform.SetParent(null);

        heldItem.isHeld = false;

        heldItem.rb.isKinematic = false;
        heldItem.col.enabled = true;

        heldItem = null;
    }

    public ItemPickup GetHeldItem()
    {
        return heldItem;
    }
}