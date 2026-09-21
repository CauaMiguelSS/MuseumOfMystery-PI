using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    [Header("Configuração")]
    [SerializeField] private float tempoMensagem = 2f;

    private string caminho => Application.persistentDataPath + "/save.json";

    private DadosSalvos dados;


    private void Awake()
    {
        // Singleton
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


    private void CarregarDados()
    {
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);

            dados = JsonUtility.FromJson<DadosSalvos>(json);

            if (dados == null)
            {
                dados = new DadosSalvos();
            }

            Debug.Log("Save carregado!");
        }
        else
        {
            dados = new DadosSalvos();

            Debug.Log("Nenhum save encontrado.");
        }
    }

    private void SalvarDados()
    {
        string json = JsonUtility.ToJson(dados, true);

        File.WriteAllText(caminho, json);

        Debug.Log("Jogo salvo em:");
        Debug.Log(caminho);

        Debug.Log(json);

        MostrarMensagemSalvo();
    }

    public void SalvarJogo()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Não encontrei nenhum objeto com a Tag 'Player'.");
        }
        else
        {
            Transform playerTransform = player.transform;

            dados.playerX = playerTransform.position.x;
            dados.playerY = playerTransform.position.y;
            dados.playerZ = playerTransform.position.z;

            dados.playerRotX = playerTransform.eulerAngles.x;
            dados.playerRotY = playerTransform.eulerAngles.y;
            dados.playerRotZ = playerTransform.eulerAngles.z;

            dados.possuiPosicaoPlayer = true;
        }

        SalvarDados();
    }

    public void AbrirCadeado(string idCadeado)
    {
        if (!dados.cadeadosAbertos.Contains(idCadeado))
        {
            dados.cadeadosAbertos.Add(idCadeado);

            Debug.Log("Cadeado salvo: " + idCadeado);

            SalvarJogo();
        }
    }

    public bool CadeadoJaAberto(string idCadeado)
    {
        if (dados == null)
            return false;

        return dados.cadeadosAbertos.Contains(idCadeado);
    }

    public void RestaurarPlayer()
    {
        if (!dados.possuiPosicaoPlayer)
        {
            Debug.Log("Não existe posição de jogador salva.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Não encontrei o Player para restaurar.");
            return;
        }

        Vector3 posicao = new Vector3(
            dados.playerX,
            dados.playerY,
            dados.playerZ
        );

        Vector3 rotacao = new Vector3(
            dados.playerRotX,
            dados.playerRotY,
            dados.playerRotZ
        );

        player.transform.position = posicao;
        player.transform.eulerAngles = rotacao;

        Debug.Log("Posição do jogador restaurada!");
    }

    public void PrepararContinue(string nomeCena)
    {
        SceneManager.sceneLoaded += AoCarregarCenaContinue;

        SceneManager.LoadScene(nomeCena);
    }


    private void AoCarregarCenaContinue(Scene cena, LoadSceneMode modo)
    {
        SceneManager.sceneLoaded -= AoCarregarCenaContinue;

        StartCoroutine(RestaurarDepoisDeCarregar());
    }


    private IEnumerator RestaurarDepoisDeCarregar()
    {
        yield return null;

        RestaurarPlayer();
    }

    public void ApagarSave()
    {
        if (File.Exists(caminho))
        {
            File.Delete(caminho);

            Debug.Log("Save apagado!");
        }

        dados = new DadosSalvos();
    }

    public bool ExisteSave()
    {
        return File.Exists(caminho);
    }

    private void MostrarMensagemSalvo()
    {
        TextMeshProUGUI texto = FindFirstObjectByType<TextMeshProUGUI>();

        if (texto == null)
            return;

        if (!texto.gameObject.name.ToLower().Contains("salvo"))
            return;

        StopAllCoroutines();

        StartCoroutine(ExibirTemporariamente(texto));
    }


    private IEnumerator ExibirTemporariamente(TextMeshProUGUI texto)
    {
        texto.text = "Jogo foi Salvo";
        texto.gameObject.SetActive(true);

        yield return new WaitForSeconds(tempoMensagem);

        if (texto != null)
            texto.gameObject.SetActive(false);
    }
}
