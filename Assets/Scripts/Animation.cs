using System.Collections;
using UnityEngine;

public class Animation
{
    public static IEnumerator FadeIn(float t, CanvasGroup c)
    {
        c.gameObject.SetActive(true);
        c.alpha = 0f;
        while (c.alpha < 1.0f)
        {
            c.alpha += Time.deltaTime / t;
            yield return null;
        }
    }

    public static IEnumerator FadeOut(float t, CanvasGroup c)
    {
        c.alpha = 1f;
        while (c.alpha > 0.0f)
        {
            c.alpha -= Time.deltaTime / t;
            yield return null;
        }
        c.gameObject.SetActive(false);
    }
}
