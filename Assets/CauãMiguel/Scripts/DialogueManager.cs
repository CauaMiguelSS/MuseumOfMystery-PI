
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private FirstPersonController playerController;

    [Header("Configuracoes")]
    [SerializeField] private float tempoPorFala = 3f;

    public static bool IsPlaying { get; private set; }

    private Coroutine dialogueCoroutine;
    private bool cameraCanMove;
    private bool playerCanMove;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    public void IniciarDialogo(string[] falas)
    {
        if (IsPlaying || falas == null || falas.Length == 0)
            return;

        dialogueCoroutine = StartCoroutine(ExecutarDialogo(falas));
    }

    private IEnumerator ExecutarDialogo(string[] falas)
    {
        IsPlaying = true;

        if (playerController != null)
        {
            cameraCanMove = playerController.cameraCanMove;
            playerCanMove = playerController.playerCanMove;

            playerController.cameraCanMove = false;
            playerController.playerCanMove = false;
        }

        dialoguePanel.SetActive(true);

        foreach (string fala in falas)
        {
            if (string.IsNullOrWhiteSpace(fala))
                continue;

            dialogueText.text = fala;

            yield return new WaitForSecondsRealtime(tempoPorFala);
        }

        dialoguePanel.SetActive(false);

        if (playerController != null)
        {
            playerController.cameraCanMove = cameraCanMove;
            playerController.playerCanMove = playerCanMove;
        }

        IsPlaying = false;
        dialogueCoroutine = null;
    }
}

