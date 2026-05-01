using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour
{

    [Header("Identity")]
    public CluedoEnums.Suspect suspect;
    public bool                isWeapon = false;
    public CluedoEnums.Weapon  weapon;

    [Header("Movement")]
    [SerializeField] private float moveSpeed  = 6f;
    [SerializeField] private float bobHeight  = 0.08f;   
    [SerializeField] private float bobSpeed   = 12f;


    public GridCell currentCell { get; private set; }
    public bool     isMoving    { get; private set; }

    private Coroutine activeMove;


    /// <summary>Instantly place the token on a cell (used during setup).</summary>
    public void PlaceOn(GridCell cell)
    {
        if (currentCell != null) currentCell.occupant = null;
        currentCell          = cell;
        currentCell.occupant = this;
        transform.position   = cell.worldPos;
    }

    /// <summary>Slide smoothly to a single cell.</summary>
    public Coroutine SlideTo(GridCell cell)
    {
        if (activeMove != null) StopCoroutine(activeMove);
        activeMove = StartCoroutine(SlideRoutine(cell));
        return activeMove;
    }

    /// <summary>Walk along a multi-cell path, one step at a time.</summary>
    public Coroutine WalkPath(List<GridCell> path)
    {
        if (activeMove != null) StopCoroutine(activeMove);
        activeMove = StartCoroutine(WalkRoutine(path));
        return activeMove;
    }

    /// <summary>Teleport to a room anchor (used when pulled in by a suggestion).</summary>
    public void TeleportToRoom(GridCell anchor)
    {
        if (activeMove != null) StopCoroutine(activeMove);
        PlaceOn(anchor);
    }


    private IEnumerator SlideRoutine(GridCell target)
    {
        isMoving = true;

        if (currentCell != null) currentCell.occupant = null;

        Vector3 origin      = transform.position;
        Vector3 destination = target.worldPos;
        float   elapsed     = 0f;
        float   duration    = Vector3.Distance(origin, destination) / moveSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float bob     = Mathf.Sin(t * Mathf.PI) * bobHeight;

            transform.position = Vector3.Lerp(origin, destination, smoothT)
                                 + new Vector3(0f, bob, 0f);
            yield return null;
        }

        transform.position   = destination;
        currentCell          = target;
        currentCell.occupant = this;

        isMoving   = false;
        activeMove = null;
    }

    private IEnumerator WalkRoutine(List<GridCell> path)
    {
        isMoving = true;

        foreach (var cell in path)
        {

            yield return StartCoroutine(SlideRoutine(cell));
            yield return new WaitForSeconds(0.05f);  
        }

        isMoving   = false;
        activeMove = null;
    }
}