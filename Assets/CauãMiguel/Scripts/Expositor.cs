using UnityEngine;

public class Expositor : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private string itemIDCorreto;

    [Header("Ponto onde a escultura ficará")]
    [SerializeField] private Transform pontoEscultura;

    [Header("Puzzle")]
    [SerializeField] private ExposicaoPuzzle puzzle;

    private bool ocupado = false;

    public void TentarColocar(ItemPickup item)
    {
        if (ocupado)
            return;

        if (item == null)
            return;

        if (item.itemID == itemIDCorreto)
        {
            ColocarEscultura(item);
        }
        else
        {
            Debug.Log("Escultura incorreta para este expositor!");
        }
    }

    private void ColocarEscultura(ItemPickup item)
    {
        ocupado = true;

        item.isHeld = false;

        item.transform.SetParent(pontoEscultura);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        item.rb.isKinematic = true;
        item.col.enabled = false;

        if (item.outline != null)
            item.outline.enabled = false;

        if (puzzle != null)
        {
            puzzle.EsculturaColocada();
        }
    }
}

