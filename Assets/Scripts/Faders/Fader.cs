using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Fader : MonoBehaviour
{
    [MinMaxSlider(0, 1)] [SerializeField] Vector2 minMax;

    public UnityEvent onFadeIn;
    public UnityEvent onFadeOut;

    Coroutine cr;

    protected virtual void Awake()
    {

    }

    private void Reset()
    {
        Awake();
        minMax.y = InitStartValue();
    }

    protected abstract float InitStartValue();
    
    protected abstract void SetValue(float value);

    public void Fade(float time, int direction)
    {
        if (cr != null) StopCoroutine(cr);
        cr = StartCoroutine(FadeCR(time, direction));
    }

    public void FadeIn(float time)
    {
        Fade(time, +1);
    }

    public void FadeOut(float time)
    {
        Fade(time, -1);
    }

    // Symmetric fade in and out
    public void FadeInOut(float time)
    {
        StartCoroutine(FadeInOutCR(time));
    }

    IEnumerator FadeCR(float time, int direction)
    {
        float startTime = Time.time;
        float startValue = InitStartValue();
        float endValue = direction == +1 ? minMax.y : minMax.x; // Set end to either min or max
        float elapsedTime;
        float value;

        do
        {
            // Set current value to interpolated value
            elapsedTime = Time.time - startTime;
            value = Mathf.Lerp(startValue, endValue, elapsedTime / time);
            SetValue(value);
            yield return new WaitForEndOfFrame();
        }
        while (elapsedTime < time);

        // Set value to final value
        SetValue(endValue);

        // Activate callback
        if (direction == +1)
            onFadeIn.Invoke();
        else
            onFadeOut.Invoke();
    }

    IEnumerator FadeInOutCR(float time)
    {
        float halfTime = time / 2;
        Coroutine cr = StartCoroutine(FadeCR(halfTime, +1));
        yield return new WaitForSeconds(halfTime);
        StopCoroutine(cr);
        StartCoroutine(FadeCR(halfTime, -1));
    }
}
