using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Header("Cena")]
    [SerializeField] private string nomeDaCena;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Áudio")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private float audioFadeDuration = 1f;

    private bool changingScene = false;

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void Sair()
    {
        Time.timeScale = 1f;

        Application.Quit();
    }

    public void ChangeScene()
    {
        if (changingScene)
            return;

        changingScene = true;

        Time.timeScale = 1f;

        StartCoroutine(ChangeSceneCoroutine());
    }


    private IEnumerator ChangeSceneCoroutine()
    {
        float timer = 0f;

        float initialVolume = 1f;

        if (backgroundMusic != null)
        {
            initialVolume = backgroundMusic.volume;
        }

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                timer / fadeDuration
            );

            if (fadeImage != null)
            {
                Color color = fadeImage.color;

                color.a = progress;

                fadeImage.color = color;
            }

            if (backgroundMusic != null)
            {
                float audioProgress = Mathf.Clamp01(
                    timer / audioFadeDuration
                );

                backgroundMusic.volume = Mathf.Lerp(
                    initialVolume,
                    0f,
                    audioProgress
                );
            }


            yield return null;
        }

        if (fadeImage != null)
        {
            Color color = fadeImage.color;

            color.a = 1f;

            fadeImage.color = color;
        }


        if (backgroundMusic != null)
        {
            backgroundMusic.volume = 0f;
        }

        Time.timeScale = 1f;

        Debug.Log("Mudando para a cena: " + nomeDaCena);

        SceneManager.LoadScene(nomeDaCena);
    }
}