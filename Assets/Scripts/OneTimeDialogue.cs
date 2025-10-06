using UnityEngine;

// ���� ������ ������ ������ � NPC �����������.
// �� ������ ������ �� ��� �� �������, ��� � NPCDialogueTrigger.
[RequireComponent(typeof(NPCDialogueTrigger))] // �����������, ��� NPCDialogueTrigger ���� ���� �� �������
public class OneTimeDialogue : MonoBehaviour
{
    [Header("�������� ������")]
    [Tooltip("���������� ���� ��� DialogueManager")]
    public DialogueSystem2D dialogueSystem;

    // ������ �� ������� ������� �� ���� �� �������
    private NPCDialogueTrigger dialogueTrigger;
    // ����, ��� ������ ��� ���
    private bool dialogueCompleted = false;

    // ���������� ��� �������
    void Awake()
    {
        // ������� ��������� NPCDialogueTrigger �� ���� �� �������
        dialogueTrigger = GetComponent<NPCDialogueTrigger>();

        // ���� dialogueSystem �� �������� � ����������, �������� ����� ��� � �����
        if (dialogueSystem == null)
        {
            dialogueSystem = FindObjectOfType<DialogueSystem2D>();
        }

        if (dialogueSystem == null)
        {
            Debug.LogError("OneTimeDialogue �� ����� ����� DialogueSystem!", this);
            enabled = false; // ��������� ���� ������, ���� ������� �� �������
        }
    }

    // ������������� �� �������, ����� ������ ���������� ��������
    private void OnEnable()
    {
        if (dialogueSystem != null)
        {
            // ������� ������� ��������: "����� ������ ����������, ������ ��� ����� OnDialogueFinished"
            dialogueSystem.onDialogueEnd += OnDialogueFinished;
        }
    }

    // ������������ �� �������, ����� ������ ����������� (����� ��� ��������� ������)
    private void OnDisable()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.onDialogueEnd -= OnDialogueFinished;
        }
    }

    // ���� ����� ����� ������ DialogueSystem'��, ����� ����� ������ � ���� ����������
    private void OnDialogueFinished()
    {
        // ���� ������ ��� ��� ��������, ������ �� ������
        if (dialogueCompleted) return;

        // ���������, � ��� �� ������ ����������, ������� ����������� ����� NPC?
        // �� ������� ��� NPC � ������� � ������ NPC � ����� ��������.
        if (dialogueSystem.npcName == dialogueTrigger.npcName)
        {
            Debug.Log($"������ � '{dialogueTrigger.npcName}' ��������. �������� ����������� ���������� �������.", this);

            // ������ ��������!
            dialogueCompleted = true;

            // --- ����� ������� ---
            // ��������� ������ NPCDialogueTrigger, ����� �� ������ �� ���������� �� ������.
            dialogueTrigger.enabled = false;

            // ����� ����� ��������� ���������, ����� ���� ������� OnTriggerEnter �� �����������
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            // ������ ������ "����� �", ���� ��� ����� �������� ������
            if (dialogueSystem.pressEPanel != null)
            {
                dialogueSystem.pressEPanel.SetActive(false);
            }
        }
    }
}