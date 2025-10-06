using UnityEngine;
using UnityEngine.UI;
public class NPCDialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isPlayerTalking; // true = игрок, false = NPC
        [TextArea(2, 5)]
        public string sentence;
    }

    [Header("ƒиалог дл€ этого NPC")]
    public string npcName = "—тату€";
    public Sprite npcSprite;
    public DialogueLine[] dialogueLines;

    [Header("—сылка на главный DialogueSystem2D")]
    public UniversalDialogueSystem2D dialogueSystem;

    private bool playerInRange = false;

    private void Start()
    {
        if (dialogueSystem == null)
        {
            dialogueSystem = FindObjectOfType<UniversalDialogueSystem2D>();
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.K))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        if (dialogueSystem == null) return;

        // ѕередаЄм данные из этого NPC в общий диалог-систему
        dialogueSystem.npcName = npcName;
        dialogueSystem.npcSprite = npcSprite;
        dialogueSystem.dialogueLines = ConvertDialogue(dialogueLines);

        dialogueSystem.StartDialogue();
    }

    private UniversalDialogueSystem2D.DialogueLine[] ConvertDialogue(DialogueLine[] lines)
    {
        UniversalDialogueSystem2D.DialogueLine[] converted = new UniversalDialogueSystem2D.DialogueLine[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            converted[i] = new UniversalDialogueSystem2D.DialogueLine
            {
                isPlayerTalking = lines[i].isPlayerTalking,
                sentence = lines[i].sentence
            };
        }
        return converted;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialogueSystem?.pressKPanel?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueSystem?.pressKPanel?.SetActive(false);
        }
    }
}
