using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem2D : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isPlayerTalking; // true = �����, false = NPC
        [TextArea(2, 5)]
        public string sentence;
    }

    [Header("Dialogue Data")]
    public string playerName = "�����";
    public Sprite playerSprite;
    public string npcName = "NPC";
    public Sprite npcSprite;
    public DialogueLine[] dialogueLines;

    [Header("UI Elements")]
    public GameObject pressKPanel;   // ������ "����� K ����� ������"
    public GameObject dialoguePanel; // ������ �������

    [Header("Player Side (Left)")]
    public Image playerPortrait;
    public Text playerNameLegacy;
    public TMP_Text playerNameTMP;

    [Header("NPC Side (Right)")]
    public Image npcPortrait;
    public Text npcNameLegacy;
    public TMP_Text npcNameTMP;

    [Header("Main Dialogue")]
    public Text dialogueLegacy;
    public TMP_Text dialogueTMP;
    public Button nextButton;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Start()
    {
        // �������� ������
        pressKPanel?.SetActive(false);
        dialoguePanel?.SetActive(false);

        // ����������� ������
        if (nextButton != null) nextButton.onClick.AddListener(NextLine);

        // ����������� �����
        SetText(playerNameLegacy, playerNameTMP, playerName);
        SetText(npcNameLegacy, npcNameTMP, npcName);

        // ����������� ��������
        if (playerPortrait) playerPortrait.sprite = playerSprite;
        if (npcPortrait) npcPortrait.sprite = npcSprite;
    }

    void Update()
    {
        if (playerInRange && !dialogueActive && Input.GetKeyDown(KeyCode.K))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        pressKPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);
        currentLine = 0;
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentLine];
            SetText(dialogueLegacy, dialogueTMP, line.sentence);

            if (line.isPlayerTalking)
            {
                // ����� �������
                SetAlpha(playerPortrait, 1f);
                SetAlpha(npcPortrait, 0.3f);
            }
            else
            {
                // NPC �������
                SetAlpha(playerPortrait, 0.3f);
                SetAlpha(npcPortrait, 1f);
            }
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel?.SetActive(false);
        dialogueActive = false;
    }

    // ===== 2D �������� =====
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!dialogueActive) pressKPanel?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressKPanel?.SetActive(false);
        }
    }

    // ===== ��������������� ������ =====
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
