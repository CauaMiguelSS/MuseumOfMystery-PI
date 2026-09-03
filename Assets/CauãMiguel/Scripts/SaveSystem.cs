using System.IO;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    [SerializeField] private TextMeshProUGUI textoSalvo;
    [SerializeField] private float tempoVisivel = 2f;

    string caminho => Application.persistentDataPath + "/save.json";
    DadosSalvos dados;

    void Awake()
    {
        // Padrão Singleton: garante que só existe 1 SaveSystem na cena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CarregarDados();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CarregarDados()
    {
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            dados = JsonUtility.FromJson<DadosSalvos>(json);
        }
        else
        {
            dados = new DadosSalvos(); // primeira vez jogando
        }
    }

    void SalvarDados()
    {
        string json = JsonUtility.ToJson(dados, true);
        File.WriteAllText(caminho, json);

        MostrarMensagemSalvo();
    }

    public void AbrirCadeado(string idCadeado)
    {
        if (!dados.cadeadosAbertos.Contains(idCadeado))
        {
            dados.cadeadosAbertos.Add(idCadeado);
            SalvarDados();
        }
    }

    public bool CadeadoJaAberto(string idCadeado)
    {
        return dados.cadeadosAbertos.Contains(idCadeado);
    }

    void MostrarMensagemSalvo()
    {
        if (textoSalvo == null) return;

        StopAllCoroutines();
        StartCoroutine(ExibirTemporariamente());
    }

    System.Collections.IEnumerator ExibirTemporariamente()
    {
        textoSalvo.text = "Jogo foi Salvo";
        textoSalvo.gameObject.SetActive(true);

        yield return new WaitForSeconds(tempoVisivel);

        textoSalvo.gameObject.SetActive(false);
    }
}
