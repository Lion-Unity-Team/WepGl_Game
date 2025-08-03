using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMove : MonoBehaviour
{
    public Tilemap tilemap;
    public float scrollInterval = 1f;
    private float timer;

    private BoundsInt bounds;

    private void Start()
    {
        bounds = tilemap.cellBounds;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= scrollInterval)
        {
            ScrollDown();
            timer = 0f;
        }
    }

    void ScrollDown()
    {
        for (int y = bounds.yMin; y < bounds.yMax - 1; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int from = new Vector3Int(x, y + 1, 0);
                Vector3Int to = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(from);
                tilemap.SetTile(to, tile);
            }
        }

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            Vector3Int top = new Vector3Int(x, bounds.yMax - 1, 0);
            tilemap.SetTile(top, GetRandomTile()); // ·£´ý Å¸ÀÏ »ðÀÔ or null·Î ºñ¿ì±â
        }
    }

    TileBase GetRandomTile()
    {
        return null; 
    }
}
