using System.Collections;
using UnityEngine;

public class ExposicaoPuzzle : MonoBehaviour
{
    [SerializeField] private int quantidadeTotal = 4;

    private int quantidadeCorreta = 0;

    [SerializeField] private GameObject objetoAoCompletar;


    public void EsculturaColocada()
    {
        quantidadeCorreta++;

        if (quantidadeCorreta >= quantidadeTotal)
        {
            PuzzleCompleto();
        }
    }

    private void PuzzleCompleto()
    {
        if (objetoAoCompletar != null)
        {
            Destroy(objetoAoCompletar);
        }
    }
}

