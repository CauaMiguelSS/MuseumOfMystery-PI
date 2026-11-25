
using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("Interface")]
    public CanvasGroup textGroup;
    public float textFadeDuration = 1f;

    [Header("Carro")]
    public Animator carAnimator;
    public AnimationClip carAnimation;

    [Header("Transição")]
    public FadeEffect fadeEffect;

    private bool started;

    public void StartCutscene()
    {
        if (started) return;

        started = true;
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        float time = 0f;

        while (time < textFadeDuration)
        {
            time += Time.deltaTime;
            textGroup.alpha = 1f - (time / textFadeDuration);
            yield return null;
        }

        textGroup.alpha = 0f;

        carAnimator.Play(carAnimation.name, 0, 0f);

        yield return new WaitForSeconds(carAnimation.length);

        fadeEffect.StartFade();
    }
}
