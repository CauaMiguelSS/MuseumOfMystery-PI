
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Configuracoes")]
    [SerializeField] private float tempoPorFala = 3f;

    public static bool IsPlaying { get; private set; }

    private Coroutine dialogueCoroutine;

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
        dialoguePanel.SetActive(true);

        foreach (string fala in falas)
        {
            if (string.IsNullOrWhiteSpace(fala))
                continue;

            dialogueText.text = fala;

            yield return new WaitForSecondsRealtime(tempoPorFala);
        }

        dialoguePanel.SetActive(false);

        IsPlaying = false;
        dialogueCoroutine = null;
    }
}
