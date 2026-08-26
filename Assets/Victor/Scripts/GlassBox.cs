using UnityEngine;

public class GlassBox : MonoBehaviour
{
    [Header("Animação da Caixa")]
    [SerializeField] private Animator boxAnimator;

    [Header("Configuração")]
    [SerializeField] private int totalLocks = 3;

    private int unlockedLocks = 0;
    private bool boxOpened = false;

    public void LockUnlocked()
    {
        if (boxOpened)
            return;

        unlockedLocks++;

        Debug.Log(
            "CADEADOS DA MONA LISA: " +
            unlockedLocks + "/" + totalLocks
        );

        if (unlockedLocks >= totalLocks)
        {
            OpenBox();
        }
    }

    private void OpenBox()
    {
        if (boxOpened)
            return;

        boxOpened = true;

        Debug.Log("TODOS OS CADEADOS FORAM DESTRANCADOS! ABRINDO CAIXA!");

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.LogWarning("GlassBox não possui um Box Animator configurado!");
        }
    }
}