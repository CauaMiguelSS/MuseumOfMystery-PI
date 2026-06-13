using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private string itemID;

    public string ItemID => itemID;

    private Collider[] colliders;
    private Rigidbody rb;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform holdPoint)
    {
        transform.SetParent(holdPoint);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    public void Drop(Vector3 dropPosition)
    {
        transform.SetParent(null);

        transform.position = dropPosition;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }
}