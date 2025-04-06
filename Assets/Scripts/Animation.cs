using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animation
{
    private static IEnumerator AnimationOnCurve(float time, Action<float> animation, AnimationCurve curve)
    {
        float currentTime = 0f;

        while (currentTime < time)
        {
            animation(curve.Evaluate(currentTime / time));
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    #region Fade
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
    #region Image
    public static IEnumerator FadeIn(float t, Image i)
    {
        i.gameObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public static IEnumerator FadeOut(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        i.gameObject.SetActive(false);
    }
    #endregion

    #endregion

    #region Slide
    public static IEnumerator Slide(GameObject o, Vector3 to, float time, AnimationCurve a)
    {
        Vector3 from = o.transform.position;
        yield return AnimationOnCurve(time, t => o.transform.position = Vector3.Lerp(from, to, t), a);
    }
    #endregion
}
