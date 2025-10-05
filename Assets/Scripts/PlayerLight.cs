using UnityEngine;

using UnityEngine.UI;

public class PlayerLight : MonoBehaviour
{
    public Transform player;
    public float radius = 3f; // Радиус света
    public Color darknessColor = new Color(0, 0, 0, 0.85f); // Цвет фона

    private Texture2D texture;
    private Image darkImage;
    private RectTransform rectTransform;

    void Start()
    {
        // Создаём Canvas
        GameObject canvasObj = new GameObject("NightCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Создаём Image
        GameObject imageObj = new GameObject("DarkImage");
        imageObj.transform.parent = canvasObj.transform;
        darkImage = imageObj.AddComponent<Image>();
        rectTransform = darkImage.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Создаём текстуру
        texture = new Texture2D(Screen.width, Screen.height);
        texture.filterMode = FilterMode.Bilinear;
        darkImage.sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
    }

    void Update()
    {
        if (player == null) return;

        // Заполняем текстуру черным
        Color[] colors = new Color[Screen.width * Screen.height];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = darknessColor;

        // Позиция игрока в координатах экрана
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

        // Рисуем свет вокруг игрока
        float radPixels = radius * 50f; // масштаб радиуса
        for (int y = 0; y < Screen.height; y++)
        {
            for (int x = 0; x < Screen.width; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(screenPos.x, screenPos.y));
                if (dist < radPixels)
                {
                    float alpha = Mathf.Lerp(0, darknessColor.a, dist / radPixels);
                    colors[y * Screen.width + x].a = alpha;
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
    }
}
