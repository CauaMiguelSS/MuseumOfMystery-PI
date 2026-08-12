using UnityEngine;
using UnityEngine.Video;

public class VHSPlayer : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TVInteraction tvInteraction;

    [Header("Luz da TV")]
    [Tooltip("Luz que será ligada quando o VHS for inserido.")]
    [SerializeField] private GameObject tvLight;

    [Header("Configuração do VHS")]
    [SerializeField] private string vhsID = "VHS";

    [Header("Visual")]
    [Tooltip("Ponto onde a fita será colocada dentro do VHS Player.")]
    [SerializeField] private Transform vhsInsertPoint;

    private bool hasVHS;

    private void Start()
    {
        if (tvLight != null)
        {
            tvLight.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "VHSPlayer: TV Light não foi configurada no Inspector!"
            );
        }

        if (tvInteraction != null)
        {
            tvInteraction.SetTVAvailable(false);
        }
        else
        {
            Debug.LogWarning(
                "VHSPlayer: TV Interaction não foi configurado no Inspector!"
            );
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
        else
        {
            Debug.LogWarning(
                "VHSPlayer: VideoPlayer não foi configurado no Inspector!"
            );
        }
    }

    public bool TryInsertVHS(ItemPickup item)
    {
        if (item == null)
        {
            Debug.LogWarning(
                "VHSPlayer: Item recebido é nulo."
            );

            return false;
        }

        if (item.itemID != vhsID)
        {
            Debug.Log(
                "Esse item não é uma fita VHS."
            );

            return false;
        }

        if (hasVHS)
        {
            Debug.Log(
                "Já existe uma fita no VHS Player."
            );

            return false;
        }

        hasVHS = true;

        if (vhsInsertPoint != null)
        {
            item.transform.SetParent(vhsInsertPoint);

            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning(
                "VHSPlayer: VHS Insert Point não foi configurado."
            );
        }

        item.gameObject.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.Play();

            Debug.Log(
                "VHSPlayer: vídeo iniciado."
            );
        }

        if (tvLight != null)
        {
            tvLight.SetActive(true);

            Debug.Log(
                "VHSPlayer: luz da TV ligada."
            );
        }

        if (tvInteraction != null)
        {
            tvInteraction.SetTVAvailable(true);

            Debug.Log(
                "VHSPlayer: TV liberada para interação."
            );
        }

        Debug.Log(
            "VHS inserido! Vídeo iniciado, luz ligada e TV liberada."
        );

        return true;
    }
}