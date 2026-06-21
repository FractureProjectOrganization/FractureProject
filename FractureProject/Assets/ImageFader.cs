using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFader : MonoBehaviour
{
    public Image imageToFade;

    private Coroutine currentFadeCoroutine;

    public void FadeIn(float duration)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeRoutine(1f, duration));
    }

    public void FadeOut(float duration)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeRoutine(0f, duration));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        if (imageToFade == null)
        {
            yield break;
        }

        Color currentColor = imageToFade.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            
            imageToFade.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            
            yield return null;
        }

        imageToFade.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}