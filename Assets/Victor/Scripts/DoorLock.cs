
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

        if (item == null)
        {
            Debug.Log("ERRO: Nenhum item está sendo segurado.");
            return;
        }

        Debug.Log("Item encontrado: " + item.itemID);
        Debug.Log("Item necessário: " + requiredItemID);

        if (item.itemID != requiredItemID)
        {
            Debug.Log("ERRO: ID incorreto!");
            lockSound?.Play();
            return;
        }

        Debug.Log("CHAVE CORRETA! Desbloqueando...");

        isUnlocked = true;

        unlockSound?.Play();

        if (lockType == LockType.NormalDoor)
        {
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Open");
                Debug.Log("Trigger Open enviado!");
            }
            else
            {
                Debug.LogError("DoorAnimator não foi atribuído!");
            }
        }
        else if (lockType == LockType.GlassBox)
        {
            if (glassBox != null)
                glassBox.LockUnlocked();
            else
                Debug.LogError("GlassBox não foi atribuído!");
        }

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
