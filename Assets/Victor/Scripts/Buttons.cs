using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Header("Cena")]
    public string nomeDaCena;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Áudio")]
    public AudioSource backgroundMusic;
    public float audioFadeDuration = 1f;

    private bool changingScene = false;

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void ChangeScene()
    {
        if (changingScene)
            return;

        changingScene = true;

        StartCoroutine(ChangeSceneCoroutine());
    }

    private IEnumerator ChangeSceneCoroutine()
    {
        Time.timeScale = 1f;

        float timer = 0f;

        float initialVolume = 1f;

        if (backgroundMusic != null)
            initialVolume = backgroundMusic.volume;

        // Fade da tela + fade do áudio simultaneamente
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(timer / fadeDuration);

            // Fade da tela para preto
            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = progress;
                fadeImage.color = color;
            }

            // Fade do áudio
            if (backgroundMusic != null)
            {
                float audioProgress =
                    Mathf.Clamp01(timer / audioFadeDuration);

                backgroundMusic.volume =
                    Mathf.Lerp(initialVolume, 0f, audioProgress);
            }

            yield return null;
        }

        // Garante que terminou completamente
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;
        }

        if (backgroundMusic != null)
            backgroundMusic.volume = 0f;

        // Troca de cena
        SceneManager.LoadScene(nomeDaCena);
    }
}