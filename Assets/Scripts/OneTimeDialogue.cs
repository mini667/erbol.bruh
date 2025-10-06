using UnityEngine;

// Этот скрипт делает диалог с NPC одноразовым.
// Он должен висеть на том же объекте, что и NPCDialogueTrigger.
[RequireComponent(typeof(NPCDialogueTrigger))] // Гарантирует, что NPCDialogueTrigger тоже есть на объекте
public class OneTimeDialogue : MonoBehaviour
{
    [Header("Ключевые ссылки")]
    [Tooltip("Перетащите сюда ваш DialogueManager")]
    public DialogueSystem2D dialogueSystem;

    // Ссылка на триггер диалога на этом же объекте
    private NPCDialogueTrigger dialogueTrigger;
    // Флаг, что диалог уже был
    private bool dialogueCompleted = false;

    // Вызывается при запуске
    void Awake()
    {
        // Находим компонент NPCDialogueTrigger на этом же объекте
        dialogueTrigger = GetComponent<NPCDialogueTrigger>();

        // Если dialogueSystem не назначен в инспекторе, пытаемся найти его в сцене
        if (dialogueSystem == null)
        {
            dialogueSystem = FindObjectOfType<DialogueSystem2D>();
        }

        if (dialogueSystem == null)
        {
            Debug.LogError("OneTimeDialogue не может найти DialogueSystem!", this);
            enabled = false; // Выключаем этот скрипт, если система не найдена
        }
    }

    // Подписываемся на событие, когда объект становится активным
    private void OnEnable()
    {
        if (dialogueSystem != null)
        {
            // Говорим системе диалогов: "Когда диалог закончится, позови мой метод OnDialogueFinished"
            dialogueSystem.onDialogueEnd += OnDialogueFinished;
        }
    }

    // Отписываемся от события, когда объект выключается (важно для избежания ошибок)
    private void OnDisable()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.onDialogueEnd -= OnDialogueFinished;
        }
    }

    // Этот метод будет вызван DialogueSystem'ом, когда ЛЮБОЙ диалог в игре завершится
    private void OnDialogueFinished()
    {
        // Если диалог уже был завершен, ничего не делаем
        if (dialogueCompleted) return;

        // Проверяем, а тот ли диалог завершился, который принадлежит ЭТОМУ NPC?
        // Мы сверяем имя NPC в системе с именем NPC в нашем триггере.
        if (dialogueSystem.npcName == dialogueTrigger.npcName)
        {
            Debug.Log($"Диалог с '{dialogueTrigger.npcName}' завершен. Отключаю возможность повторного диалога.", this);

            // Диалог завершен!
            dialogueCompleted = true;

            // --- САМОЕ ГЛАВНОЕ ---
            // Выключаем скрипт NPCDialogueTrigger, чтобы он больше не реагировал на игрока.
            dialogueTrigger.enabled = false;

            // Также можно выключить коллайдер, чтобы даже событие OnTriggerEnter не срабатывало
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            // Прячем панель "Нажми Е", если она вдруг осталась висеть
            if (dialogueSystem.pressEPanel != null)
            {
                dialogueSystem.pressEPanel.SetActive(false);
            }
        }
    }
}