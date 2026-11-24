
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public enum LockType
    {
        NormalDoor,
        GlassBox
    }

    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Lock Settings")]
    [SerializeField] private LockType lockType;
    [SerializeField] private string lockID;
    [SerializeField] private string requiredItemID;

    [Header("References")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GlassBox glassBox;
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Sounds")]
    [SerializeField] private AudioSource lockSound;
    [SerializeField] private AudioSource unlockSound;

    [Header("Unlock Dialogue")]
    [TextArea(2, 4)]
    [SerializeField]
    private string[] dialogueOnUnlock =
    {
        "Leonardo: Muito bem... você encontrou a chave.",
        "Leonardo: Mas este é apenas o primeiro segredo deste museu."
    };

    private bool isUnlocked;

    private static Object currentTarget;

    private void Start()
    {
        interactionText?.SetActive(false);
    }

    private void Update()
    {
        if (isUnlocked || playerCamera == null || DialogueManager.IsPlaying)
        {
            HideInteraction();
            return;
        }

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            interactionDistance))
        {
            DoorLock lockTarget =
                hit.collider.GetComponentInParent<DoorLock>();

            if (lockTarget == this)
            {
                ShowInteraction();
                return;
            }
        }

        HideInteraction();
    }

    private void ShowInteraction()
    {
        currentTarget = this;
        interactionText?.SetActive(true);
    }

    private void HideInteraction()
    {
        if (currentTarget == this)
        {
            interactionText?.SetActive(false);
            currentTarget = null;
        }
    }

    public void TryUnlock(PlayerPickup player)
    {
        if (isUnlocked || player == null)
            return;

        ItemPickup item = player.GetHeldItem();

        if (item == null || item.itemID != requiredItemID)
        {
            lockSound?.Play();
            return;
        }

        isUnlocked = true;

        HideInteraction();
        unlockSound?.Play();

        if (lockType == LockType.NormalDoor)
            doorAnimator?.SetTrigger("Open");
        else if (lockType == LockType.GlassBox)
            glassBox?.LockUnlocked();

        if (dialogueManager != null)
            dialogueManager.IniciarDialogo(dialogueOnUnlock);

        Destroy(item.gameObject);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        HideInteraction();
    }
}
