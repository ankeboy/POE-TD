using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;

    void Start()
    {
        StartCoroutine(FadeIn());       //this is how to call a coroutine function
    }

    public void FadeTo(string scene)
    {
        WaveSpawner.waveIndex = 0;
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime * 2f;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;     //wait a frame and then return to the start of the loop
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;     //wait a frame and then return to the start of the loop
        }

        SceneManager.LoadScene(scene);

    }
}
