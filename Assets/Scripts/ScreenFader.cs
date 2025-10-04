using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;          // ������ Image �� ���� �����
    public float fadeDuration = 1f;  // ����� �����
    public bool fadeInOnStart = true; // ������ �� ����� ����������� ��� ������� �����

    private void Awake()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;

            if (fadeInOnStart)
            {
                // ���� � ������ ����� ���������� � ������ ������ �����
                c.a = 1f;
            }
            else
            {
                // ���� ����� ������ ���������� � �������� ����������
                c.a = 0f;
            }

            fadeImage.color = c;
        }
    }

    private void Start()
    {
        if (fadeInOnStart)
        {
            StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(1f - (t / fadeDuration));
            yield return null;
        }
        SetAlpha(0f);
    }

    public IEnumerator FadeOut(string nextScene)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(t / fadeDuration);
            yield return null;
        }
        SetAlpha(1f);

        SceneManager.LoadScene(nextScene);
    }

    private void SetAlpha(float a)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;
        }
    }
}
