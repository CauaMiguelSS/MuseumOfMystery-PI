
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogo")]
    [SerializeField] private DialogueManager dialogueManager;

    [TextArea(2, 4)]
    [SerializeField] private string[] falas;

    private bool ativado;

    private void OnTriggerEnter(Collider other)
    {
        if (ativado)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager nao configurado!");
            return;
        }

        ativado = true;

        dialogueManager.IniciarDialogo(falas, () => {Destroy(gameObject);});
    }
}
