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

        botaoContinue.interactable = SaveSystem.Instance.ExisteSave();
    }

    public void OnClickNewGame()
    {
        Debug.Log("Começando novo jogo...");

        SaveSystem.Instance.ApagarSave();

        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void OnClickContinue()
    {
        Debug.Log("Continuando jogo...");

        SaveSystem.Instance.PrepararContinue(nomeCenaJogo);
    }
}
