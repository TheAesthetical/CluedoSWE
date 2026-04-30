using UnityEngine;
using System.Collections.Generic;

public class GridCell
{
    public Vector2Int gridPos;
    public Vector3    worldPos;
    public bool       isWalkable = true;
    public List<GridCell> neighbours = new();

    // Optional: track which piece is here
    public BoardPiece occupant;

    public GridCell(int x, int y, Vector3 world)
    {
        gridPos  = new Vector2Int(x, y);
        worldPos = world;
    }
}