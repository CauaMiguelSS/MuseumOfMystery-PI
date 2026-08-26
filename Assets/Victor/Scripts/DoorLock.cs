using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField] private string requiredItemID;
    [SerializeField] private Animator doorAnimator;

    [Header("Caixa de Vidro")]
    [SerializeField] private GlassBox glassBox;

    public void TryUnlock(PlayerPickup player)
    {
        ItemPickup heldItem = player.GetHeldItem();

        if (heldItem == null)
            return;

        if (heldItem.itemID != requiredItemID)
            return;

        Debug.Log("Cadeado correto destrancado: " + gameObject.name);

        // Para portas normais:
        // se houver Animator, toca a animação.
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }

        // Remove a chave
        Destroy(heldItem.gameObject);

        // Para a caixa da Mona Lisa:
        // avisa que um cadeado foi destrancado.
        if (glassBox != null)
        {
            Debug.Log("Avisando GlassBox que o cadeado foi destrancado.");
            glassBox.LockUnlocked();
        }
        else
        {
            Debug.LogWarning("GlassBox NÃO está configurado no cadeado: " + gameObject.name);
        }

        // Remove o cadeado
        Destroy(gameObject);
    }
}