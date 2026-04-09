using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    public Image fadeImage;      // imagem preta (full screen)
    public GameObject winUI;     // painel de vitória (objeto inteiro)
    public Image winUIImage;     // imagem principal do painel (background)
    public AudioSource music;    // música de fundo
    public float fadeTime = 2f;

    bool activated = false;

    void Start()
    {
        // fade preto começa invisível
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        // UI começa invisível
        if (winUIImage != null)
        {
            Color uiColor = winUIImage.color;
            uiColor.a = 0f;
            winUIImage.color = uiColor;
        }

        if (winUI != null)
            winUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        float t = 0f;
        float startVolume = music != null ? music.volume : 0f;
        Color c = fadeImage.color;

        // fade preto + diminuir música
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float v = t / fadeTime;

            c.a = Mathf.Lerp(0f, 1f, v);
            fadeImage.color = c;

            if (music != null)
                music.volume = Mathf.Lerp(startVolume, 0f, v);

            yield return null;
        }

        // parar música
        if (music != null)
            music.Stop();

        // pausar jogo
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ativar UI
        if (winUI != null)
            winUI.SetActive(true);

        // fade do painel (usando unscaled time)
        if (winUIImage != null)
        {
            float uiT = 0f;
            Color uiColor = winUIImage.color;

            while (uiT < fadeTime)
            {
                uiT += Time.unscaledDeltaTime; // funciona mesmo pausado
                float v = uiT / fadeTime;

                uiColor.a = Mathf.Lerp(0f, 1f, v);
                winUIImage.color = uiColor;

                yield return null;
            }
        }
    }
}