using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea] public string text;
    public Sprite characterSprite;
    public bool isLeftCharacter; // true — левый, false — правый
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public Text leftNameText;
    public Text rightNameText;
    public Button continueButton;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public DialogueLine[] dialogueLines;
    public string nextSceneName = "NextScene"; // название сцены

    private int currentLine = 0;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        leftCharacterImage.gameObject.SetActive(false);
        rightCharacterImage.gameObject.SetActive(false);
        leftNameText.gameObject.SetActive(false);
        rightNameText.gameObject.SetActive(false);
        continueButton.onClick.AddListener(NextLine);

        StartCoroutine(StartDialogueAfterDelay(5f));
    }

    private IEnumerator StartDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            continueButton.gameObject.SetActive(false);
            var line = dialogueLines[currentLine];

            leftCharacterImage.gameObject.SetActive(line.isLeftCharacter);
            leftNameText.gameObject.SetActive(line.isLeftCharacter);
            rightCharacterImage.gameObject.SetActive(!line.isLeftCharacter);
            rightNameText.gameObject.SetActive(!line.isLeftCharacter);

            if (line.isLeftCharacter)
            {
                leftCharacterImage.sprite = line.characterSprite;
                leftNameText.text = line.characterName;
            }
            else
            {
                rightCharacterImage.sprite = line.characterSprite;
                rightNameText.text = line.characterName;
            }

            StopAllCoroutines();
            StartCoroutine(TypeText(line.text));
        }
        else
        {
            StartCoroutine(EndDialogue());
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        continueButton.gameObject.SetActive(true);
    }

    private void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    private IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(3f);

        ScreenFader fader = FindFirstObjectByType<ScreenFader>();
        if (fader != null)
        {
            yield return fader.StartCoroutine(fader.FadeOut(nextSceneName));
        }
    }
}
