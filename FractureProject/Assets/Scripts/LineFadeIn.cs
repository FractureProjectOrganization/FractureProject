using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LineFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 5.0f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!spriteRenderer) return;

        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void StartCoroutine()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
            
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            
            yield return null;
        }
    }
}
