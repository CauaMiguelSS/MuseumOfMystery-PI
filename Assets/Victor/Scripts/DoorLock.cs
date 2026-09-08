using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Header("Save")]
    [SerializeField] private string lockID;

    [Header("Item Necessário")]
    [SerializeField] private string requiredItemID;
    [SerializeField] private Animator doorAnimator;

    [Header("Caixa de Vidro")]
    [SerializeField] private GlassBox glassBox;

    [Header("Som do Cadeado")]
    [SerializeField] private AudioSource lockSound;

    [Header("Som de Destrancar")]
    [SerializeField] private AudioSource unlockSound;

    void Start()
    {
        /*
        if (SaveSystem.Instance != null && SaveSystem.Instance.CadeadoJaAberto(lockID))
        {
            AplicarEstadoDestrancado();
        }
        */
    }

    public void TryUnlock(PlayerPickup player)
    {
        ItemPickup heldItem = player.GetHeldItem();

        // Não está segurando nada
        if (heldItem == null)
        {
            TocarSomCadeado();
            return;
        }

        // Está segurando a chave errada
        if (heldItem.itemID != requiredItemID)
        {
            TocarSomCadeado();
            return;
        }

        // Chave correta
        Debug.Log("Cadeado correto destrancado: " + gameObject.name);

        // Toca o som de destrancar
        TocarSomDestrancar();

        /*
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AbrirCadeado(lockID);
        }
        */

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }

        Destroy(heldItem.gameObject);

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

    void TocarSomCadeado()
    {
        if (lockSound != null)
        {
            lockSound.Play();
        }
    }

    void TocarSomDestrancar()
    {
        if (unlockSound != null)
        {
            unlockSound.Play();
        }
    }

    /*
    void AplicarEstadoDestrancado()
    {
        if (glassBox != null)
        {
            glassBox.LockUnlocked();
        }

        Destroy(gameObject);
    }
    */
}