using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField] private string requiredItemID;
    [SerializeField] private Animator doorAnimator;

    public void TryUnlock(PlayerPickup player)
    {
        ItemPickup heldItem = player.GetHeldItem();

        if (heldItem == null)
            return;

        if (heldItem.itemID != requiredItemID)
            return;

        doorAnimator.SetTrigger("Open");

        Destroy(heldItem.gameObject);

        Destroy(gameObject);
    }
}