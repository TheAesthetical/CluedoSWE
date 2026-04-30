using System.Collections.Generic;

using UnityEngine;
using System.Collections;

#pragma warning disable CA1010 // Declare types in namespaces
public class BoardPiece : MonoBehaviour
#pragma warning restore CA1010 // Declare types in namespaces
{
    public GridCell currentCell;

    private GridManager gridManager;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    // Snap instantly
    public void PlaceOn(GridCell cell)
    {
        if (currentCell != null) currentCell.occupant = null;
        currentCell = cell;
        currentCell.occupant = this;
        transform.position = cell.worldPos;
    }

    // Smooth slide (call with StartCoroutine)
    public IEnumerator SlideTo(GridCell cell, float speed = 5f)
    {
        if (currentCell != null) currentCell.occupant = null;
        currentCell = cell;
        currentCell.occupant = this;

        while (Vector3.Distance(transform.position, cell.worldPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, cell.worldPos, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = cell.worldPos;
    }

    // Walk along a path step by step
    public IEnumerator WalkPath(List<GridCell> path, float speed = 5f)
    {
        foreach (var cell in path)
            yield return StartCoroutine(SlideTo(cell, speed));
    }
}