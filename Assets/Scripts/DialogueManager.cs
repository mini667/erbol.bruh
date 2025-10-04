using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea] public string text;
    public Sprite characterSprite;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;   // Панель диалога
    public Text characterNameText;     // Имя персонажа (Legacy UI Text)
    public Text dialogueText;          // Текст диалога (Legacy UI Text)
    public Image characterImage;       // Спрайт персонажа
    public Button continueButton;      // Кнопка "Продолжить"

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public DialogueLine[] dialogueLines;

    private int currentLine = 0;

    private void Start()
    {
        Debug.Log("DialogueManager запущен. Проверка ссылок...");

        if (dialoguePanel == null)
        {
            Debug.LogError("❌ dialoguePanel не привязан в инспекторе!");
            return;
        }

        Debug.Log("Панель: " + dialoguePanel.name +
                  " | activeSelf=" + dialoguePanel.activeSelf +
                  " | activeInHierarchy=" + dialoguePanel.activeInHierarchy);

        dialoguePanel.SetActive(false);
        if (characterImage != null) characterImage.gameObject.SetActive(false);
        if (continueButton != null) continueButton.onClick.AddListener(NextLine);

        StartCoroutine(StartDialogueAfterDelay(5f));
        Debug.Log("Диалог начнется через 5 секунд...");
    }

    private IEnumerator StartDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Попытка включить панель: " + dialoguePanel.name);
        dialoguePanel.SetActive(true);

        Debug.Log("После SetActive(true): activeSelf=" + dialoguePanel.activeSelf +
                  " | activeInHierarchy=" + dialoguePanel.activeInHierarchy);

        // Проверяем предков панели
        Transform t = dialoguePanel.transform;
        while (t != null)
        {
            Debug.Log("Предок: " + t.name +
                      " | activeSelf=" + t.gameObject.activeSelf +
                      " | activeInHierarchy=" + t.gameObject.activeInHierarchy +
                      " | scale=" + t.localScale);
            t = t.parent;
        }

        // CanvasGroup (если есть)
        CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
        if (cg != null) Debug.Log("CanvasGroup: alpha=" + cg.alpha);

        if (characterImage != null) characterImage.gameObject.SetActive(true);
        ShowLine();
        Debug.Log("✅ Диалог начался!");
    }

    private void ShowLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            continueButton.gameObject.SetActive(false);

            characterNameText.text = dialogueLines[currentLine].characterName;
            characterImage.sprite = dialogueLines[currentLine].characterSprite;

            StartCoroutine(TypeText(dialogueLines[currentLine].text));
        }
        else
        {
            Debug.Log("✅ Конец диалога");
            dialoguePanel.SetActive(false);
            characterImage.gameObject.SetActive(false);
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
}






//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//[System.Serializable]
//public class DialogueLine
//{
//    public string characterName;    
//    [TextArea] public string text;  
//    public Sprite characterSprite;  
//}

//public class DialogueManager : MonoBehaviour
//{
//    [Header("UI")]
//    public GameObject dialoguePanel;   
//    public Text characterNameText;     
//    public Text dialogueText;         
//    public Image characterImage;       
//    public Button continueButton;      

//    [Header("Settings")]
//    public float typingSpeed = 0.05f;
//    public DialogueLine[] dialogueLines;

//    private int currentLine = 0;

//    //private void Start()
//    //{
//    //    dialoguePanel.SetActive(false);
//    //    characterImage.gameObject.SetActive(false);
//    //    continueButton.onClick.AddListener(NextLine);

//    //    StartCoroutine(StartDialogueAfterDelay(5f));
//    //    print("Диалог начнется через 5 секунд...");
//    //}
//    private void Start()
//    {
//        Debug.Log("DialogueManager Start. Проверяем references...");
//        if (dialoguePanel == null)
//        {
//            Debug.LogError("dialoguePanel == NULL! Привяжи панель в инспекторе.");
//            return;
//        }

//        Debug.Log("Assigned panel: " + dialoguePanel.name +
//                  " | activeSelf=" + dialoguePanel.activeSelf +
//                  " | activeInHierarchy=" + dialoguePanel.activeInHierarchy);

//        // Инфо о CanvasGroup (если есть)
//        CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
//        if (cg != null) Debug.Log("CanvasGroup: alpha=" + cg.alpha + " interactable=" + cg.interactable + " blocks=" + cg.blocksRaycasts);

//        // Инфо о RectTransform / scale
//        RectTransform rt = dialoguePanel.GetComponent<RectTransform>();
//        if (rt != null) Debug.Log("RectTransform: localScale=" + rt.localScale + " anchoredPos=" + rt.anchoredPosition + " size=" + rt.sizeDelta);

//        // Инфо о Canvas в родителях
//        Canvas parentCanvas = dialoguePanel.GetComponentInParent<Canvas>();
//        if (parentCanvas != null) Debug.Log("Parent Canvas: enabled=" + parentCanvas.enabled + " renderMode=" + parentCanvas.renderMode);

//        // Лог по всем родителям (activeSelf/activeInHierarchy)
//        Transform t = dialoguePanel.transform;
//        while (t != null)
//        {
//            Debug.Log("Ancestor: " + t.name + " | activeSelf=" + t.gameObject.activeSelf + " | activeInHierarchy=" + t.gameObject.activeInHierarchy + " | scale=" + t.localScale);
//            t = t.parent;
//        }

//        // Устанавливаем начальные значения и запускаем корутину
//        dialoguePanel.SetActive(false);
//        if (characterImage != null) characterImage.gameObject.SetActive(false);
//        if (continueButton != null) continueButton.onClick.AddListener(NextLine);

//        StartCoroutine(StartDialogueAfterDelay(5f));
//        Debug.Log("Диалог начнется через 5 секунд...");
//    }

//    private IEnumerator StartDialogueAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);

//        Debug.Log("Попытка включить панель: " + dialoguePanel.name);
//        dialoguePanel.SetActive(true);

//        Debug.Log("После SetActive(true): activeSelf=" + dialoguePanel.activeSelf + " | activeInHierarchy=" + dialoguePanel.activeInHierarchy);

//        // Если activeInHierarchy == false — посмотрим цепочку родителей ещё раз
//        Transform t = dialoguePanel.transform;
//        while (t != null)
//        {
//            Debug.Log("Ancestor after try: " + t.name + " | activeSelf=" + t.gameObject.activeSelf + " | activeInHierarchy=" + t.gameObject.activeInHierarchy + " | scale=" + t.localScale);
//            t = t.parent;
//        }

//        // CanvasGroup снова проверим
//        CanvasGroup cg = dialoguePanel.GetComponent<CanvasGroup>();
//        if (cg != null) Debug.Log("CanvasGroup after try: alpha=" + cg.alpha);

//        if (characterImage != null) characterImage.gameObject.SetActive(true);
//        ShowLine();
//        Debug.Log("Диалог начался!");
//    }

//    private IEnumerator StartDialogueAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        dialoguePanel.SetActive(true);
//        characterImage.gameObject.SetActive(true);
//        ShowLine();
//        print("Диалог начался!");
//    }

//    private void ShowLine()
//    {
//        if (currentLine < dialogueLines.Length)
//        {
//            continueButton.gameObject.SetActive(false);
//            characterNameText.text = dialogueLines[currentLine].characterName;
//            characterImage.sprite = dialogueLines[currentLine].characterSprite;
//            StartCoroutine(TypeText(dialogueLines[currentLine].text));
//        }
//        else
//        {
//            Debug.Log("Конец");
//            dialoguePanel.SetActive(false);
//            characterImage.gameObject.SetActive(false);
//        }
//    }

//    private IEnumerator TypeText(string text)
//    {
//        dialogueText.text = "";
//        foreach (char c in text)
//        {
//            dialogueText.text += c;
//            yield return new WaitForSeconds(typingSpeed);
//        }
//        continueButton.gameObject.SetActive(true);
//    }

//    private void NextLine()
//    {
//        currentLine++;
//        ShowLine();
//    }
//}
