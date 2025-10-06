using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueSystem2D : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isPlayerTalking;
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
    public GameObject pressEPanel;
    public GameObject dialoguePanel;

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
    private bool dialogueActive = false;
    private Coroutine typingCoroutine;

    public System.Action onDialogueEnd; // ��� ������� �� ���������� ��� ����� �� �������� ��������

    void Start()
    {
        pressEPanel?.SetActive(false);
        dialoguePanel?.SetActive(false);

        if (nextButton != null) nextButton.onClick.AddListener(NextLine);

        SetText(playerNameLegacy, playerNameTMP, playerName);
        // ��� NPC � ������ ������ ��������������� ���������, ��� ��� ��� ������ ����� ������ ��� �������� ��� �������� �� ���������
        // SetText(npcNameLegacy, npcNameTMP, npcName);

        if (playerPortrait) playerPortrait.sprite = playerSprite;
        // if (npcPortrait) npcPortrait.sprite = npcSprite;
    }

    // <<< �������: ����� Update, ��� ��� ������ ������� ������ ���������� ������ �� NPCDialogueTrigger

    public void StartDialogue()
    {
        // ��������� UI � ������� �� NPC
        SetText(npcNameLegacy, npcNameTMP, npcName);
        if (npcPortrait) npcPortrait.sprite = npcSprite;

        dialogueActive = true;
        pressEPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);
        currentLine = 0;
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentLine];

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

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(line.sentence));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeText(string sentence)
    {
        SetText(dialogueLegacy, dialogueTMP, "");
        string currentText = "";

        foreach (char letter in sentence.ToCharArray())
        {
            currentText += letter;
            SetText(dialogueLegacy, dialogueTMP, currentText);
            yield return new WaitForSeconds(0.03f);
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
        onDialogueEnd?.Invoke(); // <<< �����: ��� ������ �������� ����� � ������ �������
    }

    // <<< �������: ������ OnTriggerEnter2D � OnTriggerExit2D, ����� �������� ������������ ������

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