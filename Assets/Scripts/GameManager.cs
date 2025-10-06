// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool HasKey { get; private set; } = false;
    public bool ChestOpened { get; private set; } = false;
    public bool HasReward { get; private set; } = false; // Это наше "сердце"
    public bool VictoryItemCollected { get; private set; } = false; // <<< НОВОЕ: Для финального предмета

    // ... (Awake, SetHasKey, SetChestOpened, SetHasReward остаются без изменений) ...

    // <<< НОВЫЙ МЕТОД
    public void SetVictoryItemCollected(bool value)
    {
        VictoryItemCollected = value;
        if (value) Debug.Log($"[{System.DateTime.Now:HH:mm:ss}] 🏆 Победа! Финальный предмет собран.");
    }

    // Код который уже был
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }
    public void SetHasKey(bool value) { HasKey = value; Debug.Log($"[{System.DateTime.Now:HH:mm:ss}] 🔑 Ключ {(value ? "поднят!" : "потерян!")}"); }
    public void SetChestOpened(bool value) { ChestOpened = value; Debug.Log($"[{System.DateTime.Now:HH:mm:ss}] 💰 Сундук {(value ? "открыт!" : "закрыт!")}"); if (value && QuestManager.Instance != null) { QuestManager.Instance.OnChestOpened(); } }
    public void SetHasReward(bool value) { HasReward = value; Debug.Log($"[{System.DateTime.Now:HH:mm:ss}] ✨ Награда {(value ? "получена!" : "потеряна!")}"); }
}