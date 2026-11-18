
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Pilar")]
    public GameObject pilar;

    [Header("Os 5 canos do caminho")]
    public GirarCano[] canos;

    [Header("Rotação correta de cada cano (Z)")]
    public float[] rotacoesCorretas;

    [Header("Margem de erro")]
    public float tolerancia = 2f;

    private bool resolvido = false;

    void Update()
    {
        if (resolvido) return;

        if (canos == null || rotacoesCorretas == null)
            return;

        if (canos.Length != 5 || rotacoesCorretas.Length != 5)
            return;

        for (int i = 0; i < 5; i++)
        {
            if (canos[i] == null) return;

            float rotacaoAtual = canos[i].transform.localEulerAngles.z;

            float diferenca = Mathf.Abs(
                Mathf.DeltaAngle(rotacaoAtual, rotacoesCorretas[i])
            );

            if (diferenca > tolerancia)
                return;
        }

        resolvido = true;

        if (pilar != null)
            Destroy(pilar);

        Debug.Log("PUZZLE RESOLVIDO!");
    }
}
