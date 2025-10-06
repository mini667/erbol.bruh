using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UniversalDialogueSystem2D : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isPlayerTalking; // true = �����, false = NPC
        [TextArea(2, 5)]
        public string sentence;
    }

    [Header("NPC ���������")]
    public string npcName = "NPC";
    public Sprite npcSprite;
    public DialogueLine[] dialogueLines;

    [Header("������ �� UI (���� ��� ���� NPC)")]
    public GameObject pressKPanel;   // "����� K ����� ������"
    public GameObject dialoguePanel; // ������ �������

    [Header("����� (�����)")]
    public string playerName = "�����";
    public Sprite playerSprite;
    public Image playerPortrait;
    public Text playerNameLegacy;
    public TMP_Text playerNameTMP;

    [Header("NPC (������)")]
    public Image npcPortrait;
    public Text npcNameLegacy;
    public TMP_Text npcNameTMP;

    [Header("����� �������")]
    public Text dialogueLegacy;
    public TMP_Text dialogueTMP;
    public Button nextButton;

    [Header("��������� �������")]
    public float typingSpeed = 0.03f;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // �������� UI � ������
        pressKPanel?.SetActive(false);
        dialoguePanel?.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);

        // ����������� ������
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

    // ==== �������� ������ ====
    void StartDialogue()
    {
        dialogueActive = true;
        pressKPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);

        // ����������� NPC
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

    // ==== �������� ====
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

    // ==== ��������������� ====
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
