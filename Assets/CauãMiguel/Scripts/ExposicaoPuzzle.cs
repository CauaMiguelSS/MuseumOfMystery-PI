using UnityEngine;

public class ExposicaoPuzzle : MonoBehaviour
{
    [Header("Quantidade de esculturas")]
    [SerializeField] private int quantidadeTotal = 3;

    private int quantidadeCorreta = 0;

    [Header("Objeto para ativar quando terminar")]
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
            objetoAoCompletar.SetActive(false);
        }
    }
}

