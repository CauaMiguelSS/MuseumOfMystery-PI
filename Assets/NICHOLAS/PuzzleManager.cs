
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject pilar;
    public GirarCano[] canos;
    public float[] rotacoesCorretas;
    public float tolerancia = 2f;

    private bool puzzleResolvido = false;

    void Update()
    {
        if (puzzleResolvido)
            return;

        if (canos == null || rotacoesCorretas == null)
            return;

        if (canos.Length != rotacoesCorretas.Length)
            return;

        bool todosCorretos = true;

        for (int i = 0; i < canos.Length; i++)
        {
            if (canos[i] == null)
            {
                todosCorretos = false;
                break;
            }

            float anguloAtual =
                canos[i].transform.localEulerAngles.z;

            float diferenca = Mathf.Abs(
                Mathf.DeltaAngle(anguloAtual, rotacoesCorretas[i])
            );

            // O elemento 2 é o cano em formato de X.
            // Ele continua correto mesmo girando 90 graus.
            if (i == 2)
            {
                float resto = diferenca % 90f;
                diferenca = Mathf.Min(resto, 90f - resto);
            }

            if (diferenca > tolerancia)
            {
                todosCorretos = false;
                break;
            }
        }

        if (todosCorretos)
        {
            puzzleResolvido = true;

            if (pilar != null)
                pilar.SetActive(false);

            Debug.Log("PUZZLE RESOLVIDO!");
        }
    }
}
