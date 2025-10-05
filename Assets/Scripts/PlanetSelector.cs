using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlanetSelector : MonoBehaviour
{
    public GameObject loadingPanel; // ������ "��������..."
    public Text loadingText;        // ����� "��������"
    public string sceneToLoad;      // �������� �����
    public float hoverScale = 1.2f; // ��������� ������������� ������� ��� ���������

    private Vector3 originalScale;
    private bool isHovered = false;

    private void Start()
    {
        originalScale = transform.localScale;
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        transform.localScale = originalScale * hoverScale;
    }

    private void OnMouseExit()
    {
        isHovered = false;
        transform.localScale = originalScale;
    }

    private void OnMouseDown()
    {
        // ����� ������ � ��������� ��������
        StartCoroutine(ShowLoadingAndLoadScene());
    }

    private IEnumerator ShowLoadingAndLoadScene()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        float timer = 0f;
        int dotCount = 0;
        loadingText.text = "��������";

        // ������ �����
        while (timer < 3f) // 3 �������, ����� ��������
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = "��������" + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        // ��������� ����� �����
        SceneManager.LoadScene(sceneToLoad);
    }
}
