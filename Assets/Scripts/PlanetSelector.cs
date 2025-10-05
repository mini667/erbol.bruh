using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlanetSelector : MonoBehaviour
{
    public GameObject loadingPanel; // Панель "Загрузка..."
    public Text loadingText;        // Текст "Загрузка"
    public string sceneToLoad;      // Название сцены
    public float hoverScale = 1.2f; // Насколько увеличивается планета при наведении

    private Vector3 originalScale;
    private bool isHovered = false;

    private void Start()
    {
        originalScale = transform.localScale;
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        transform.localScale = originalScale * hoverScale;
    }

    private void OnMouseExit()
    {
        isHovered = false;
        transform.localScale = originalScale;
    }

    private void OnMouseDown()
    {
        // Когда нажали — запускаем загрузку
        StartCoroutine(ShowLoadingAndLoadScene());
    }

    private IEnumerator ShowLoadingAndLoadScene()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        float timer = 0f;
        int dotCount = 0;
        loadingText.text = "Загрузка";

        // Эффект точек
        while (timer < 3f) // 3 секунды, потом загрузка
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = "Загрузка" + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        // Загружаем новую сцену
        SceneManager.LoadScene(sceneToLoad);
    }
}
