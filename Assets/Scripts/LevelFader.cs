using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelFader : MonoBehaviour
{
    public static LevelFader instance;

    [SerializeField] float fadeOutTime = 1.5f;
    [SerializeField] float blackStayTime = 0.5f;
    [SerializeField] float fadeInTime = 1f;

    [SerializeField] SpriteFader spriteFader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(LoadLevelFade(level));
    }

    IEnumerator LoadLevelFade(int level)
    {
        // This is swapped because level "fading out" is black panel "fading in"
        spriteFader.FadeIn(fadeOutTime);
        // Stop and save the current time
        SpeedrunManager.instance.StopLevel();

        yield return new WaitForSeconds(fadeOutTime);

        // Loading the level
        float startTime = Time.time;
        LevelLoader.LoadLevel(level);
        yield return new WaitUntil(() => LevelLoader.levelLoaded);
        float elapsedTime = Time.time - startTime;

        if (elapsedTime < blackStayTime)
        {
            yield return new WaitForSeconds(blackStayTime - elapsedTime);
        }
        
        // Fading into the level
        spriteFader.FadeOut(fadeInTime);
        //yield return new WaitForSeconds(fadeInTime);
    }

    public void FadeOutIn(float fadeOutTime, float fadeInTime)
    {
        StartCoroutine(FadeOutInCR(fadeOutTime, fadeInTime));
    }

    IEnumerator FadeOutInCR(float fadeOutTime, float fadeInTime)
    {
        spriteFader.FadeIn(fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime);
        spriteFader.FadeOut(fadeInTime);
    }
}
