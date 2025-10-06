using UnityEngine;
using System.Collections;

// Важно: Убедитесь, что имя файла совпадает с именем класса "KeyAndChestTrigger2D"
public class KeyAndChestTrigger2D : MonoBehaviour
{
    // Типы триггеров
    public enum TriggerType { Key, Chest, Reward, VictoryItem }
    public TriggerType triggerType = TriggerType.Key;

    [Header("Игрок")]
    public GameObject player;

    [Header("Панели UI")]
    public GameObject keyPanel;
    public GameObject findKeyPanel;
    public GameObject openChestPanel;

    [Header("Объекты для Ключа")]
    public GameObject keyObject;

    [Header("Объекты для Сундука")]
    public GameObject closedChest;
    public GameObject openedChestPrefab;
    public GameObject heartPrefab;
    public float heartLifetime = 1.5f;
    public GameObject rewardObjectInScene;
    public Transform rewardSpawnPoint;

    [Header("Победа (настроить у VictoryItem)")]
    public SceneLoader sceneLoader;
    public string victorySceneName = "VictoryScene";

    [Header("Настройки")]
    public string interactKey = "e";

    // <<< ЭТИ ПЕРЕМЕННЫЕ БЫЛИ ПРОПУЩЕНЫ
    private bool playerInRange = false;

    void Start()
    {
        HideAllPanels();
        if (GameManager.Instance == null)
        {
            Debug.LogError("Ошибка: GameManager не найден в сцене!");
        }
    }

    void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(interactKey)) return;

        switch (triggerType)
        {
            case TriggerType.Key:
                if (!GameManager.Instance.HasKey) PickUpKey();
                break;
            case TriggerType.Chest:
                if (!GameManager.Instance.ChestOpened) OpenChest();
                break;
            case TriggerType.Reward:
                if (!GameManager.Instance.HasReward) PickUpReward();
                break;
            case TriggerType.VictoryItem:
                if (!GameManager.Instance.VictoryItemCollected) PickUpVictoryItem();
                break;
        }
    }

    // <<< ВСЕ МЕТОДЫ НИЖЕ БЫЛИ ПРОПУЩЕНЫ

    void PickUpKey()
    {
        GameManager.Instance.SetHasKey(true);
        SafeSetActive(keyPanel, false);
        Destroy(keyObject != null ? keyObject : gameObject);
    }

    void PickUpReward()
    {
        GameManager.Instance.SetHasReward(true);
        SafeSetActive(keyPanel, false);
        Destroy(gameObject);
    }

    void PickUpVictoryItem()
    {
        GameManager.Instance.SetVictoryItemCollected(true);
        if (sceneLoader != null)
        {
            sceneLoader.StartLoadingSequence(victorySceneName);
        }
        else
        {
            Debug.LogError("На победном предмете не назначен SceneLoader!");
        }
        gameObject.GetComponent<Collider2D>().enabled = false;
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void OpenChest()
    {
        if (!GameManager.Instance.HasKey)
        {
            Debug.Log("Сундук закрыт — нужен ключ!");
            return;
        }

        GameManager.Instance.SetChestOpened(true);
        HideAllPanels();

        if (openedChestPrefab != null && closedChest != null)
        {
            Instantiate(openedChestPrefab, closedChest.transform.position, closedChest.transform.rotation);
            Destroy(closedChest);
        }

        if (rewardObjectInScene != null && rewardSpawnPoint != null)
        {
            rewardObjectInScene.transform.position = rewardSpawnPoint.position;
            rewardObjectInScene.transform.rotation = rewardSpawnPoint.rotation;
            rewardObjectInScene.SetActive(true);
        }

        if (heartPrefab != null)
            StartCoroutine(SpawnHeart());
    }

    IEnumerator SpawnHeart()
    {
        Vector3 pos = closedChest != null ? closedChest.transform.position + Vector3.up * 0.5f : transform.position;
        GameObject heart = Instantiate(heartPrefab, pos, Quaternion.identity);
        Destroy(heart, heartLifetime); // Упрощенный вариант уничтожения
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player == null || other.gameObject != player) return;
        playerInRange = true;

        switch (triggerType)
        {
            case TriggerType.Key:
                if (!GameManager.Instance.HasKey) SafeSetActive(keyPanel, true);
                break;
            case TriggerType.Chest:
                if (!GameManager.Instance.ChestOpened)
                {
                    SafeSetActive(GameManager.Instance.HasKey ? openChestPanel : findKeyPanel, true);
                }
                break;
            case TriggerType.Reward:
                if (!GameManager.Instance.HasReward) SafeSetActive(keyPanel, true);
                break;
            case TriggerType.VictoryItem:
                if (!GameManager.Instance.VictoryItemCollected) SafeSetActive(keyPanel, true);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player == null || other.gameObject != player) return;
        playerInRange = false;
        HideAllPanels();
    }

    private void HideAllPanels()
    {
        SafeSetActive(keyPanel, false);
        SafeSetActive(findKeyPanel, false);
        SafeSetActive(openChestPanel, false);
    }

    private void SafeSetActive(GameObject obj, bool active)
    {
        if (obj != null)
            obj.SetActive(active);
    }
}