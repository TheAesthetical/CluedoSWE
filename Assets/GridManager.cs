using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Dimensions")]
    public int width  = 24;
    public int height = 25;
    public float cellSize = 1f;

    private GridCell[,] grid;

    void Awake()
    {
        BuildGrid();
    }

    void BuildGrid()
    {
        grid = new GridCell[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            grid[x, y] = new GridCell(x, y, GridToWorld(x, y));
        }

        // Wire up neighbours AFTER all cells exist
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (x > 0)           grid[x, y].neighbours.Add(grid[x-1, y]);
            if (x < width - 1)   grid[x, y].neighbours.Add(grid[x+1, y]);
            if (y > 0)           grid[x, y].neighbours.Add(grid[x, y-1]);
            if (y < height - 1)  grid[x, y].neighbours.Add(grid[x, y+1]);
        }
    }

    // Convert grid coords → Unity world position (centred on cell)
    public Vector3 GridToWorld(int x, int y)
        => new Vector3(x * cellSize + cellSize * 0.5f, 0f, y * cellSize + cellSize * 0.5f);

    // Convert world position → grid coords
    public Vector2Int WorldToGrid(Vector3 worldPos)
        => new Vector2Int(Mathf.FloorToInt(worldPos.x / cellSize),
                          Mathf.FloorToInt(worldPos.z / cellSize));

    public GridCell GetCell(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return grid[x, y];
    }

    public GridCell GetCell(Vector2Int pos) => GetCell(pos.x, pos.y);
}