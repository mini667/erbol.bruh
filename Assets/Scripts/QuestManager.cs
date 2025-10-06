// QuestManager.cs
using UnityEngine;
using System.Collections;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    // Состояния квеста
    public enum HeartQuestState { NotStarted, Requested, HeartFound, Delivered }
    public HeartQuestState currentQuestState = HeartQuestState.NotStarted;

    // Глобальное состояние для предмета
    public bool hasHeart = false;
    public bool hasArtifact = false;

    // Ссылки на UI и Prefabs для анимации
    [Header("UI и Префабы для анимации")]
    public GameObject heartCanvasImage; // Ваш Canvas Image с сердцем (скрыт по умолчанию)
    public GameObject artifactPrefab;     // Префаб/объект артефакта (для 2D/3D сцены)
    public Transform artifactSpawnPoint; // Место, где появляется артефакт
    public float itemFadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Вызывается из KeyAndChestTrigger2D при открытии сундука (через GameManager)
    public void OnChestOpened()
    {
        if (currentQuestState == HeartQuestState.Requested && !hasHeart)
        {
            hasHeart = true;
            currentQuestState = HeartQuestState.HeartFound;
            Debug.Log("✅ Игрок нашел сердце!");

            // Запускаем анимацию UI сердца
            StartCoroutine(AnimateCanvasItem(heartCanvasImage));
        }
    }

    // Вызывается из UniversalDialogueSystem2D после сдачи сердца
    public void DeliverHeartAndGetArtifact()
    {
        if (currentQuestState == HeartQuestState.HeartFound && hasHeart)
        {
            hasHeart = false;
            currentQuestState = HeartQuestState.Delivered;

            Debug.Log("🎉 Сердце сдано, получен Артефакт!");

            // Запускаем анимацию артефакта
            if (artifactPrefab != null && artifactSpawnPoint != null)
            {
                GameObject artifact = Instantiate(artifactPrefab, artifactSpawnPoint.position, Quaternion.identity);
                StartCoroutine(ArtifactAnimation(artifact));
            }
            hasArtifact = true;
        }
    }

    // ========================================================
    // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ АНИМАЦИИ
    // ========================================================

    // Анимация для Canvas Image (Сердце)
    private IEnumerator AnimateCanvasItem(GameObject itemImage)
    {
        if (itemImage == null) yield break;

        // Убедимся, что есть CanvasGroup для плавности
        CanvasGroup cg = itemImage.GetComponent<CanvasGroup>();
        if (cg == null) cg = itemImage.AddComponent<CanvasGroup>();

        itemImage.SetActive(true);
        cg.alpha = 0f; // Начинаем с невидимости
        itemImage.transform.localScale = Vector3.one * 0.5f; // Начинаем с маленького размера

        float t = 0f;

        while (t < itemFadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / itemFadeDuration;

            // Вырастает и появляется
            itemImage.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * 1.5f, progress);
            cg.alpha = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        // Теперь плавно исчезает
        t = 0f;
        float fadeOutDuration = 0.5f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }

        itemImage.SetActive(false);
        cg.alpha = 1f;
        itemImage.transform.localScale = Vector3.one; // Возвращаем к исходному размеру
    }

    // Анимация для 2D/3D объекта (Артефакт)
    private IEnumerator ArtifactAnimation(GameObject artifact)
    {
        // Аналогично анимации сердца, Артефакт появляется
        artifact.transform.localScale = Vector3.zero;
        float growTime = 1.0f;
        float t = 0f;

        while (t < growTime)
        {
            t += Time.deltaTime;
            artifact.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / growTime);
            yield return null;
        }

        // Оставляем артефакт в сцене для подбора или отображения
    }
}