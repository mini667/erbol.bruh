using UnityEngine;

public class   : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isPlayerTalking;
        [TextArea(2, 5)]
        public string sentence;
    }

    [Header("������ ��� ����� NPC")]
    public string npcName = "������";
    public Sprite npcSprite;

    [Header("������� ������ (�� ���������� ������)")]
    public DialogueLine[] dialogueLines;

    [Header("��������� ������ (����� �������� �������)")]
    public bool isQuestGiver = false;
    public DialogueLine[] completionDialogueLines;

    [Header("������� �� ����� (��������� � NPC)")]
    public GameObject itemToSpawnOnCompletion;
    public Transform itemSpawnPoint;

    [Header("������ �� ������� DialogueSystem2D")]
    public DialogueSystem2D dialogueSystem;

    private bool playerInRange = false;
    private bool questCompleted = false;

    void Start()
    {
        if (dialogueSystem == null) dialogueSystem = FindObjectOfType<DialogueSystem2D>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isQuestGiver && !questCompleted && GameManager.Instance != null && GameManager.Instance.HasReward)
            {
                StartQuestDialogue();
            }
            else
            {
                StartDialogue(dialogueLines);
            }
        }
    }

    void StartDialogue(DialogueLine[] lines)
    {
        if (dialogueSystem == null || lines.Length == 0) return;
        dialogueSystem.npcName = npcName;
        dialogueSystem.npcSprite = npcSprite;
        dialogueSystem.dialogueLines = ConvertDialogue(lines);
        dialogueSystem.onDialogueEnd = null;
        dialogueSystem.StartDialogue();
    }

    void StartQuestDialogue()
    {
        StartDialogue(completionDialogueLines);
        dialogueSystem.onDialogueEnd += OnQuestDialogueEnd;
    }

    void OnQuestDialogueEnd()
    {
        questCompleted = true;
        Debug.Log("��������� ������ ��������. ������� �������!");

        if (itemToSpawnOnCompletion != null && itemSpawnPoint != null)
        {
            Instantiate(itemToSpawnOnCompletion, itemSpawnPoint.position, itemSpawnPoint.rotation);
        }
    }

    // <<< ���� ����� ��� ������ � ������� ������
    private DialogueSystem2D.DialogueLine[] ConvertDialogue(DialogueLine[] lines)
    {
        DialogueSystem2D.DialogueLine[] converted = new DialogueSystem2D.DialogueLine[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            converted[i] = new DialogueSystem2D.DialogueLine
            {
                isPlayerTalking = lines[i].isPlayerTalking,
                sentence = lines[i].sentence
            };
        }
        return converted; // �� ������ ���������� ���������
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}