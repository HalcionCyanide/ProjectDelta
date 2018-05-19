using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fading : MonoBehaviour
{
    public bool activeOnLoad = false;
    public float fadeInTime = 1.5f;
    public float fadeOutTime = 2.5f;
    public Image splashImage;
    public string loadLevel;

    public void StartFade(string levelName)
    {
        StopAllCoroutines();
        StartCoroutine(LoadScene(levelName));   
        //level loader uses the index if needed.
    }

    IEnumerator LoadScene(string levelName)
    {
        splashImage.enabled = true;
        if(!(splashImage.canvasRenderer.GetAlpha() < 1f && splashImage.canvasRenderer.GetAlpha() > 0f))
            splashImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSecondsRealtime(fadeInTime);
        //splashImage.enabled = false;
        SceneManager.LoadScene(levelName);
    }

    IEnumerator Start()
    {
        if(activeOnLoad)
        {
            splashImage.enabled = true;
            splashImage.canvasRenderer.SetAlpha(0.0f);

            FadeIn();
            yield return new WaitForSecondsRealtime(fadeInTime);
            FadeOut();
            yield return new WaitForSecondsRealtime(fadeOutTime);
            SceneManager.LoadScene(loadLevel);
        }
        else
        {
            splashImage.enabled = true;
            FadeOut();
            yield return new WaitForSecondsRealtime(fadeOutTime);
            splashImage.enabled = false;
        }
    }

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, fadeInTime, true);
    }

    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, fadeOutTime, true);
    }
}