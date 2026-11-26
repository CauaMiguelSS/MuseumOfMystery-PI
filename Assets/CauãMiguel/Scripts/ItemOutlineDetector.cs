
using UnityEngine;

public class ItemOutlineDetector : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Camera playerCamera;

    [Header("Configurações")]
    [SerializeField] private float distance = 3f;

    private ItemPickup currentItem;
    private Outline currentOutline;

    private void Start()
    {

        ItemPickup[] items = FindObjectsByType<ItemPickup>(
            FindObjectsSortMode.None
        );

        foreach (ItemPickup item in items)
        {
            Outline outline = item.GetComponent<Outline>();

            if (outline == null)
                outline = item.GetComponentInChildren<Outline>(true);

            if (outline != null)
                outline.enabled = false;
        }
    }

    private void Update()
    {
        DetectItem();
    }

    private void DetectItem()
    {
        ItemPickup detectedItem = null;
        Outline detectedOutline = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            distance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        System.Array.Sort(hits, (a, b) =>
            a.distance.CompareTo(b.distance)
        );

        foreach (RaycastHit hit in hits)
        {
            ItemPickup item =
                hit.collider.GetComponentInParent<ItemPickup>();

            if (item == null || item.isHeld)
                continue;

            Outline outline = item.GetComponent<Outline>();

            if (outline == null)
                outline = item.GetComponentInChildren<Outline>(true);

            if (outline == null)
                continue;

            detectedItem = item;
            detectedOutline = outline;

            break;
        }

        if (currentOutline != detectedOutline)
        {
            if (currentOutline != null)
                currentOutline.enabled = false;

            currentItem = detectedItem;
            currentOutline = detectedOutline;
        }

        if (currentOutline != null && !currentOutline.enabled)
        {
            currentOutline.enabled = true;
        }
    }

    private void OnDisable()
    {
        if (currentOutline != null)
            currentOutline.enabled = false;

        currentItem = null;
        currentOutline = null;
    }
}

