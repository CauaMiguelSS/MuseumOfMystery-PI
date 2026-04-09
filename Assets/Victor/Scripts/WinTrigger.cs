using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    public GameObject winUI;   // painel completo (imagem + texto + botão)
    public float fadeTime = 1.5f;

    private Graphic[] graphics;
    private bool activated = false;

    void Start()
    {
        // pega todos elementos visuais
        winUI.SetActive(true);
        graphics = winUI.GetComponentsInChildren<Graphic>();

        // começa invisível
        SetAlpha(0f);

        winUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            // congela o jogo IMEDIATAMENTE
            Time.timeScale = 0f;

            // ativa UI e começa fade
            winUI.SetActive(true);
            StartCoroutine(FadeInUI());
        }
    }

    IEnumerator FadeInUI()
    {
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; // funciona com o jogo pausado

            float v = t / fadeTime;
            SetAlpha(v);

            yield return null;
        }

        SetAlpha(1f);

        // AGORA libera o cursor (depois do fade)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void SetAlpha(float alpha)
    {
        foreach (Graphic g in graphics)
        {
            if (g == null) continue;

            Color c = g.color;
            c.a = alpha;
            g.color = c;
        }
    }
}