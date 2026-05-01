using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and destroys highlight overlays on valid destination cells.
/// Attach to an empty "CellHighlights" GameObject.
/// </summary>
public class CellHighlighter : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("A flat sprite or quad that tints the cell. Should be 1×1 world units.")]
    [SerializeField] private GameObject highlightPrefab;

    [Header("Colours")]
    [SerializeField] private Color reachableColour = new(0.2f, 0.8f, 1f, 0.55f);
    [SerializeField] private Color doorColour       = new(1f,   0.8f, 0f, 0.70f);
    [SerializeField] private Color hoverColour      = new(1f,   1f,   1f, 0.85f);


    private readonly List<GameObject> activeHighlights = new();
    private readonly Dictionary<GridCell, GameObject> cellToHighlight = new();

    private GridCell hoveredCell;


    /// <summary>Show highlights on a set of reachable cells.</summary>
    public void ShowReachable(List<GridCell> cells)
    {
        ClearAll();

        foreach (var cell in cells)
        {
            var go = Spawn(cell);
            SetColour(go, cell.isDoor ? doorColour : reachableColour);
            cellToHighlight[cell] = go;
        }
    }

    /// <summary>Brighten the cell the mouse is over.</summary>
    public void SetHover(GridCell cell)
    {
        // Reset previous hover
        if (hoveredCell != null && cellToHighlight.TryGetValue(hoveredCell, out var prev))
            SetColour(prev, hoveredCell.isDoor ? doorColour : reachableColour);

        hoveredCell = cell;

        if (cell != null && cellToHighlight.TryGetValue(cell, out var go))
            SetColour(go, hoverColour);
    }

    /// <summary>Remove all highlight overlays.</summary>
    public void ClearAll()
    {
        foreach (var go in activeHighlights)
            if (go != null) Destroy(go);

        activeHighlights.Clear();
        cellToHighlight.Clear();
        hoveredCell = null;
    }

    /// <summary>Returns the highlighted cell at a world position, or null.</summary>
    public GridCell GetHighlightedCellAt(Vector3 worldPos, float tolerance = 0.5f)
    {
        foreach (var kvp in cellToHighlight)
        {
            if (Vector3.Distance(kvp.Key.worldPos, worldPos) < tolerance)
                return kvp.Key;
        }
        return null;
    }

    public bool IsHighlighted(GridCell cell) => cellToHighlight.ContainsKey(cell);


    private GameObject Spawn(GridCell cell)
    {
        var go = highlightPrefab != null
            ? Instantiate(highlightPrefab, cell.worldPos, Quaternion.identity, transform)
            : CreateDefaultHighlight(cell.worldPos);

        activeHighlights.Add(go);
        return go;
    }

    private GameObject CreateDefaultHighlight(Vector3 pos)
    {
        var go = new GameObject("Highlight");
        go.transform.SetParent(transform);
        go.transform.position = pos;

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite        = CreateSquareSprite();
        sr.sortingOrder  = 5;  
        return go;
    }

    private static Sprite CreateSquareSprite()
    {
        var tex = new Texture2D(4, 4);
        var pixels = new Color[16];
        for (int i = 0; i < 16; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
    }

    private static void SetColour(GameObject go, Color colour)
    {
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr) sr.color = colour;
    }
}