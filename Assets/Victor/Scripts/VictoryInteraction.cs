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

    [Header("Controle da Câmera")]
    [SerializeField] private MonoBehaviour cameraScript;

    private bool venceu = false;

    private void Update()
    {
        if (venceu)
            return;

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

        // Congela o jogo
        Time.timeScale = 0f;

        // Para a câmera
        if (cameraScript != null)
            cameraScript.enabled = false;

        // Libera o mouse
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

            yield return null;
        }

        victoryPanel.alpha = 1f;
    }
}