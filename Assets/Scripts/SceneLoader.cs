// SceneLoader.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Важно для работы со сценами

public class SceneLoader : MonoBehaviour
{
    [Header("Объекты экрана загрузки")]
    public GameObject loadingScreenPanel; // Панель-фон, которая активируется
    public Image[] loadingDots;           // Картинки с точками, которые появятся по очереди

    [Header("Настройки")]
    public float dotInterval = 0.7f;      // Время между появлением точек

    void Start()
    {
        // В начале игры экран загрузки выключен
        loadingScreenPanel.SetActive(false);
        foreach (var dot in loadingDots)
        {
            dot.gameObject.SetActive(false);
        }
    }

    // Этот метод будет вызываться из другого скрипта (KeyAndChestTrigger2D)
    public void StartLoadingSequence(string sceneNameToLoad)
    {
        StartCoroutine(LoadSceneCoroutine(sceneNameToLoad));
    }

    IEnumerator LoadSceneCoroutine(string sceneNameToLoad)
    {
        // 1. Показываем фон
        loadingScreenPanel.SetActive(true);
        yield return new WaitForSeconds(dotInterval); // Небольшая пауза

        // 2. Показываем точки по очереди
        foreach (var dot in loadingDots)
        {
            dot.gameObject.SetActive(true);
            yield return new WaitForSeconds(dotInterval);
        }

        // 3. Загружаем новую сцену
        Debug.Log($"Загружаю сцену: {sceneNameToLoad}");
        SceneManager.LoadScene(sceneNameToLoad);
    }
}