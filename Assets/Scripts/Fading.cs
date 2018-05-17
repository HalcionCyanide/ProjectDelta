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
        StartCoroutine(LoadScene(levelName));   
        //level loader uses the index if needed.
    }

    IEnumerator LoadScene(string levelName)
    {
        splashImage.enabled = true;
        splashImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(fadeInTime);
        //FadeOut();
        //yield return new WaitForSeconds(2.5f);
        splashImage.enabled = false;
        SceneManager.LoadScene(levelName);
    }

    IEnumerator Start()
    {
        if(activeOnLoad)
        {
            splashImage.enabled = true;
            splashImage.canvasRenderer.SetAlpha(0.0f);

            FadeIn();
            yield return new WaitForSeconds(fadeInTime);
            FadeOut();
            yield return new WaitForSeconds(fadeOutTime);
            SceneManager.LoadScene(loadLevel);
        }
        else
        {
            splashImage.enabled = true;
            FadeOut();
            yield return new WaitForSeconds(fadeOutTime);
            splashImage.enabled = false;
        }
    }

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, fadeInTime, false);
    }

    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, fadeOutTime, false);
    }
}