using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private string nomeCenaJogo = "Jogo";

    [Header("UI")]
    [SerializeField] private Button botaoContinue;


    private void Start()
    {
        AtualizarBotaoContinue();
    }


    private void AtualizarBotaoContinue()
    {
        if (botaoContinue == null)
            return;
    }

    public void OnClickNewGame()
    {
        Debug.Log("Começando novo jogo...");

        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void OnClickContinue()
    {
        Debug.Log("Continuando jogo...");
    }
}
