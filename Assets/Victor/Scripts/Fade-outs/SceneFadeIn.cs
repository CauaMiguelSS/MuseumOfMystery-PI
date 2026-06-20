using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private float fadeTime = 2f;

    IEnumerator Start()
    {
        playerMovement.enabled = false;

        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            blackScreen.alpha = 1 - (t / fadeTime);
            yield return null;
        }

        blackScreen.alpha = 0;
        playerMovement.enabled = true;
    }
}