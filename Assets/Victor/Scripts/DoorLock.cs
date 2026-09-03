using UnityEngine;
        
public class DoorLock : MonoBehaviour
{       
    [Header("Save")]
    [SerializeField] private string lockID;
        
    [SerializeField] private string requiredItemID;
    [SerializeField] private Animator doorAnimator;
        
    [Header("Caixa de Vidro")]
    [SerializeField] private GlassBox glassBox;
        
    void Start()
    {   
        if (SaveSystem.Instance != null && SaveSystem.Instance.CadeadoJaAberto(lockID))
        {
            AplicarEstadoDestrancado();
        }
    }   
        
    public void TryUnlock(PlayerPickup player)
    {   
        ItemPickup heldItem = player.GetHeldItem();

        if (heldItem == null)
            return;

        if (heldItem.itemID != requiredItemID)
            return;

        Debug.Log("Cadeado correto destrancado: " + gameObject.name);

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AbrirCadeado(lockID);
        }

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
        
    void AplicarEstadoDestrancado()
    {   
        
        if (glassBox != null)
        {
            glassBox.LockUnlocked();
        }
        
        Destroy(gameObject);
    }   
}       