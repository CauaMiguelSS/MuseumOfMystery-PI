using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Camera playerCamera;

    private ItemPickup heldItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Se já estiver segurando algo,
            // tenta colocar no expositor.
            if (heldItem != null)
            {
                if (TryPlaceOnExpositor())
                    return;
            }

            // Se não estiver segurando nada,
            // tenta pegar.
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

    private void TryPickup()
    {
        if (playerCamera == null)
        {
            Debug.LogError("PlayerPickup: A câmera não foi configurada!");
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Debug.Log("Raycast acertou: " + hit.collider.name);

            ItemPickup item = hit.collider.GetComponent<ItemPickup>();

            if (item == null)
            {
                Debug.Log("O objeto acertado não possui ItemPickup.");
                return;
            }

            if (holdPoint == null)
            {
                Debug.LogError("PlayerPickup: Hold Point não foi configurado!");
                return;
            }

            heldItem = item;
            heldItem.isHeld = true;

            heldItem.transform.SetParent(holdPoint);

            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;

            heldItem.rb.isKinematic = true;
            heldItem.col.enabled = false;

            if (heldItem.outline != null)
                heldItem.outline.enabled = false;

            Debug.Log("Pegou: " + heldItem.itemID);
        }
    }

    private bool TryPlaceOnExpositor()
    {
        if (playerCamera == null)
            return false;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

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

    private void Drop()
    {
        heldItem.transform.SetParent(null);

        heldItem.isHeld = false;

        heldItem.rb.isKinematic = false;
        heldItem.col.enabled = true;

        heldItem = null;

        Debug.Log("Item largado.");
    }

    public ItemPickup GetHeldItem()
    {
        return heldItem;
    }
}