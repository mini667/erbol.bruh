using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UniversalDialogueSystem2D : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isPlayerTalking; // true = игрок, false = NPC
        [TextArea(2, 5)]
        public string sentence;
    }

    [Header("NPC Настройки")]
    public string npcName = "NPC";
    public Sprite npcSprite;
    public DialogueLine[] dialogueLines;

    [Header("Ссылки на UI (одни для всех NPC)")]
    public GameObject pressKPanel;   // "Нажми K чтобы начать"
    public GameObject dialoguePanel; // Панель диалога

    [Header("Игрок (слева)")]
    public string playerName = "Игрок";
    public Sprite playerSprite;
    public Image playerPortrait;
    public Text playerNameLegacy;
    public TMP_Text playerNameTMP;

    [Header("NPC (справа)")]
    public Image npcPortrait;
    public Text npcNameLegacy;
    public TMP_Text npcNameTMP;

    [Header("Текст диалога")]
    public Text dialogueLegacy;
    public TMP_Text dialogueTMP;
    public Button nextButton;

    [Header("Настройки эффекта")]
    public float typingSpeed = 0.03f;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Скрываем UI в начале
        pressKPanel?.SetActive(false);
        dialoguePanel?.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);

        // Настраиваем игрока
        SetText(playerNameLegacy, playerNameTMP, playerName);
        if (playerPortrait) playerPortrait.sprite = playerSprite;
    }

    void Update()
    {
        if (playerInRange && !dialogueActive && Input.GetKeyDown(KeyCode.K))
        {
            StartDialogue();
        }
    }

    // ==== Основная логика ====
    void StartDialogue()
    {
        dialogueActive = true;
        pressKPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);

        // Настраиваем NPC
        SetText(npcNameLegacy, npcNameTMP, npcName);
        if (npcPortrait) npcPortrait.sprite = npcSprite;

        currentLine = 0;
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentLine];

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(line.sentence));

            if (line.isPlayerTalking)
            {
                SetAlpha(playerPortrait, 1f);
                SetAlpha(npcPortrait, 0.3f);
            }
            else
            {
                SetAlpha(playerPortrait, 0.3f);
                SetAlpha(npcPortrait, 1f);
            }
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeText(string sentence)
    {
        isTyping = true;
        nextButton.interactable = false;

        SetText(dialogueLegacy, dialogueTMP, "");

        for (int i = 0; i < sentence.Length; i++)
        {
            if (dialogueLegacy)
                dialogueLegacy.text += sentence[i];
            if (dialogueTMP)
                dialogueTMP.text += sentence[i];

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        nextButton.interactable = true;
    }

    public void NextLine()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            DialogueLine line = dialogueLines[currentLine];
            SetText(dialogueLegacy, dialogueTMP, line.sentence);
            isTyping = false;
            nextButton.interactable = true;
            return;
        }

        currentLine++;
        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel?.SetActive(false);
        dialogueActive = false;
    }

    // ==== Триггеры ====
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!dialogueActive)
                pressKPanel?.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressKPanel?.SetActive(false);
        }
    }

    // ==== Вспомогательные ====
    void SetText(Text legacy, TMP_Text tmp, string text)
    {
        if (legacy) legacy.text = text;
        if (tmp) tmp.text = text;
    }

    void SetAlpha(Image img, float alpha)
    {
        if (!img) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
