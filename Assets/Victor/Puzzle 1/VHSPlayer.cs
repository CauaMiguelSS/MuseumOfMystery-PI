using UnityEngine;
using UnityEngine.Video;

public class VHSPlayer : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TVInteraction tvInteraction;

    [Header("Luz da TV")]
    [Tooltip("Luz que será ligada permanentemente quando o VHS for inserido.")]
    [SerializeField] private GameObject tvLight;

    [Header("Configuração do VHS")]
    [SerializeField] private string vhsID = "VHS";

    [Header("Visual")]
    [Tooltip("Ponto onde a fita seria colocada dentro do VHS Player.")]
    [SerializeField] private Transform vhsInsertPoint;

    private bool hasVHS;

    public bool TryInsertVHS(ItemPickup item)
    {
        if (item == null)
            return false;

        // Verifica se é a fita correta
        if (item.itemID != vhsID)
        {
            Debug.Log("Esse item não é uma fita VHS.");
            return false;
        }

        // Impede colocar outra fita
        if (hasVHS)
        {
            Debug.Log("Já existe uma fita no VHS Player.");
            return false;
        }

        hasVHS = true;

        // Coloca a fita no ponto de inserção
        if (vhsInsertPoint != null)
        {
            item.transform.SetParent(vhsInsertPoint);

            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        // A fita desaparece
        item.gameObject.SetActive(false);

        // Inicia o vídeo
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("VHSPlayer: VideoPlayer não foi configurado!");
        }

        // =====================================================
        // LIGA A LUZ DA TV E NÃO DESLIGA MAIS
        // =====================================================
        if (tvLight != null)
        {
            tvLight.SetActive(true);
        }

        // Libera a interação com a TV
        if (tvInteraction != null)
        {
            tvInteraction.SetTVAvailable(true);
        }

        Debug.Log("VHS inserido! Vídeo iniciado e luz ligada permanentemente.");

        return true;
    }
}