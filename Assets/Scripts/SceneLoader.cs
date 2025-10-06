// SceneLoader.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ����� ��� ������ �� �������

public class SceneLoader : MonoBehaviour
{
    [Header("������� ������ ��������")]
    public GameObject loadingScreenPanel; // ������-���, ������� ������������
    public Image[] loadingDots;           // �������� � �������, ������� �������� �� �������

    [Header("���������")]
    public float dotInterval = 0.7f;      // ����� ����� ���������� �����

    void Start()
    {
        // � ������ ���� ����� �������� ��������
        loadingScreenPanel.SetActive(false);
        foreach (var dot in loadingDots)
        {
            dot.gameObject.SetActive(false);
        }
    }

    // ���� ����� ����� ���������� �� ������� ������� (KeyAndChestTrigger2D)
    public void StartLoadingSequence(string sceneNameToLoad)
    {
        StartCoroutine(LoadSceneCoroutine(sceneNameToLoad));
    }

    IEnumerator LoadSceneCoroutine(string sceneNameToLoad)
    {
        // 1. ���������� ���
        loadingScreenPanel.SetActive(true);
        yield return new WaitForSeconds(dotInterval); // ��������� �����

        // 2. ���������� ����� �� �������
        foreach (var dot in loadingDots)
        {
            dot.gameObject.SetActive(true);
            yield return new WaitForSeconds(dotInterval);
        }

        // 3. ��������� ����� �����
        Debug.Log($"�������� �����: {sceneNameToLoad}");
        SceneManager.LoadScene(sceneNameToLoad);
    }
}