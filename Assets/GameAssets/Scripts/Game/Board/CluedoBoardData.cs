using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that describes the Cluedo board layout.
/// Create via: right-click in Project → Create → Cluedo → Board Data
/// </summary>
[CreateAssetMenu(menuName = "Cluedo/Board Data", fileName = "CluedoBoardData")]
public class CluedoBoardData : ScriptableObject
{
    // ─── Grid dimensions ──────────────────────────────────────────────────────

    [Header("Grid")]
    public int   gridWidth  = 24;
    public int   gridHeight = 25;
    public float cellSize   = 1f;

    [Tooltip("World-space position of the bottom-left corner of cell (0,0)")]
    public Vector3 boardOrigin = new(-12f, -12.5f, 0f);

    // ─── Rooms ────────────────────────────────────────────────────────────────

    [Header("Rooms")]
    public List<RoomDefinition> rooms = new();

    // ─── Blocked cells (walls / inaccessible areas) ──────────────────────────

    [Header("Blocked Cells")]
    [Tooltip("Cells that can never be entered (walls, decoration, centre square)")]
    public List<Vector2Int> blockedCells = new();

    // ─── Starting positions ───────────────────────────────────────────────────

    [Header("Starting Positions")]
    public List<SuspectStart> startPositions = new();

    // ─── Helpers ──────────────────────────────────────────────────────────────

    public Vector3 GridToWorld(int x, int y) =>
        boardOrigin + new Vector3(x * cellSize + cellSize * 0.5f,
                                  y * cellSize + cellSize * 0.5f, 0f);

    public Vector2Int WorldToGrid(Vector3 world) =>
        new(Mathf.FloorToInt((world.x - boardOrigin.x) / cellSize),
            Mathf.FloorToInt((world.y - boardOrigin.y) / cellSize));

    public RoomDefinition GetRoom(CluedoEnums.Room room) =>
        rooms.Find(r => r.room == room);
}

// ─── Supporting data classes ──────────────────────────────────────────────────

[Serializable]
public class RoomDefinition
{
    public CluedoEnums.Room room;
    public Color            debugColour = Color.cyan;

    [Tooltip("All cells that are inside this room")]
    public List<Vector2Int> interiorCells = new();

    [Tooltip("Corridor cells adjacent to this room's entrances (movement passes through these)")]
    public List<Vector2Int> doorCells = new();

    [Tooltip("Cell where tokens are placed when entering the room")]
    public Vector2Int tokenAnchor;

    /// <summary>Does this room have a secret passage to another room?</summary>
    public bool           hasSecretPassage;
    public CluedoEnums.Room secretPassageTo;
}

[Serializable]
public class SuspectStart
{
    public CluedoEnums.Suspect suspect;
    public Vector2Int          gridPosition;
}