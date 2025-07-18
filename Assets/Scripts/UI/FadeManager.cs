using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [SerializeField] private GameObject fadeScreen;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        fadeScreen.SetActive(false);
    }

    public void FadeToScene(string sceneName)
    {
        fadeScreen.SetActive(true);
        StartCoroutine(FadeRoutine(sceneName));
    }

    private IEnumerator FadeRoutine(string sceneName)
    {
        yield return FadeOut();
        Debug.Log("before loading scene");
        SceneManager.LoadScene(sceneName);
        Debug.Log("after loading scene");
        Debug.Log("before fade");
        yield return FadeIn();
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color c = fadeImage.color;
        while (timer < fadeDuration)
        {
            c.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = c;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color c = fadeImage.color;
        while (timer < fadeDuration)
        {
            c.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            fadeImage.color = c;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        c.a = 0f;
        fadeImage.color = c;
    }
}
