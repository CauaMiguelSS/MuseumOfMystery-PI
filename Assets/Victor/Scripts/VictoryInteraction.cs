using UnityEngine;
using System.Collections;

public class VictoryInteraction : MonoBehaviour
{
    [Header("Interação")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distancia = 3f;

    [Header("Painel de Vitória")]
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private float fadeDuration = 1f;

    private bool venceu = false;

    private void Update()
    {
        // Depois da vitória, mantém o jogo congelado
        // e o cursor liberado.
        if (venceu)
        {
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            VerificarInteracao();
        }
    }

    private void VerificarInteracao()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, distancia))
        {
            if (hit.transform == transform)
            {
                StartCoroutine(MostrarVitoria());
            }
        }
    }

    private IEnumerator MostrarVitoria()
    {
        venceu = true;

        // Congela imediatamente
        Time.timeScale = 0f;

        // Libera o cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Ativa o painel
        victoryPanel.gameObject.SetActive(true);

        // Começa invisível
        victoryPanel.alpha = 0f;

        float tempo = 0f;

        // Fade-in
        while (tempo < fadeDuration)
        {
            tempo += Time.unscaledDeltaTime;

            victoryPanel.alpha = Mathf.Clamp01(tempo / fadeDuration);

            // Garante que o cursor continue liberado
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            yield return null;
        }

        victoryPanel.alpha = 1f;

        // Garante novamente
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}