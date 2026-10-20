
using UnityEngine;

public class NoteCloseButton : MonoBehaviour
{
    public void FecharNotaAtual()
    {
        if (InteractableNote.NotaAtual != null)
        {
            InteractableNote.NotaAtual.FecharNota();
        }
    }
}
