using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private string nomeCenaJogo = "Jogo";

    [Header("UI")]
    [SerializeField] private Button botaoContinue;

    void Start()
    {
        // Desabilita o Continue se não existir save
        if (botaoContinue != null)
        {
            botaoContinue.interactable = SaveSystem.Instance.ExisteSave();
        }
    }

    public void OnClickNewGame()
    {
        SaveSystem.Instance.ApagarSave();
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void OnClickContinue()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }
}
