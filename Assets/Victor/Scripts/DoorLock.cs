
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Header("Save")]
    [SerializeField] private string lockID;

    [Header("Item Necessario")]
    [SerializeField] private string requiredItemID;
    [SerializeField] private Animator doorAnimator;

    [Header("Caixa de Vidro")]
    [SerializeField] private GlassBox glassBox;

    [Header("Sons")]
    [SerializeField] private AudioSource lockSound;
    [SerializeField] private AudioSource unlockSound;

    [Header("Dialogo")]
    [SerializeField] private DialogueManager dialogueManager;

    [TextArea(2, 4)]
    [SerializeField]
    private string[] dialogueOnUnlock =
    {
        "Leonardo: Muito bem... voce encontrou a chave.",
        "Leonardo: Mas este e apenas o primeiro segredo deste museu."
    };

    public void TryUnlock(PlayerPickup player)
    {
        ItemPickup heldItem = player.GetHeldItem();

        if (heldItem == null)
        {
            TocarSomCadeado();
            return;
        }

        if (heldItem.itemID != requiredItemID)
        {
            TocarSomCadeado();
            return;
        }

        Debug.Log("Cadeado correto destrancado: " + gameObject.name);

        TocarSomDestrancar();

        if (doorAnimator != null)
            doorAnimator.SetTrigger("Open");

        // Inicia o dialogo ao destrancar.
        if (dialogueManager != null)
        {
            dialogueManager.IniciarDialogo(dialogueOnUnlock);
        }
        else
        {
            Debug.LogError(
                "DialogueManager nao configurado no cadeado: "
                + gameObject.name
            );
        }

        Destroy(heldItem.gameObject);

        if (glassBox != null)
            glassBox.LockUnlocked();
        else
            Debug.LogWarning(
                "GlassBox nao esta configurado: " + gameObject.name
            );

        Destroy(gameObject);
    }

    private void TocarSomCadeado()
    {
        if (lockSound != null)
            lockSound.Play();
    }

    private void TocarSomDestrancar()
    {
        if (unlockSound != null)
            unlockSound.Play();
    }
}
