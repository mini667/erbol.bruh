using UnityEngine;

public class AIMapGenerator : MonoBehaviour
{
    public GameObject grassTile;
    public GameObject waterTile;
    public GameObject houseTile;
    public int width = 10;
    public int height = 10;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        string[,] map = new string[height, width];

        // Пример простого заполнения
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y < 2) map[y, x] = "W"; // вода сверху
                else if (x == 5 && y == 5) map[y, x] = "H"; // дом
                else map[y, x] = "G"; // трава
            }
        }

        // Спавн префабов
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject tile = null;
                switch (map[y, x])
                {
                    case "G": tile = grassTile; break;
                    case "W": tile = waterTile; break;
                    case "H": tile = houseTile; break;
                }

                if (tile != null)
                    Instantiate(tile, new Vector3(x, -y, 0), Quaternion.identity);
            }
        }
    }
}

