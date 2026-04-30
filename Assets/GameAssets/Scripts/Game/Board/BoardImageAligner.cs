using UnityEngine;

/// <summary>
/// Editor helper — draws a grid over your board image so you can verify alignment.
/// Only active in the Editor; has zero runtime cost.
/// Attach to the same GameObject as GridManager.
/// </summary>
[ExecuteInEditMode]
public class BoardImageAligner : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Aligner Settings")]
    [SerializeField] private CluedoBoardData boardData;
    [SerializeField] private bool showRoomColours = true;
    [SerializeField] private bool showCoordinates = false;

    // Fine-tune offset if the image doesn't align perfectly
    [SerializeField] private Vector2 pixelNudge = Vector2.zero;

    private void OnDrawGizmos()
    {
        if (boardData == null) return;

        int   w    = boardData.gridWidth;
        int   h    = boardData.gridHeight;
        float size = boardData.cellSize;
        var   orig = boardData.boardOrigin + (Vector3)pixelNudge;

        // ── Grid lines ─────────────────────────────────────────────────────

        Gizmos.color = new Color(1f, 1f, 0f, 0.4f);

        // Vertical lines
        for (int x = 0; x <= w; x++)
        {
            var from = orig + new Vector3(x * size, 0, 0);
            var to   = orig + new Vector3(x * size, h * size, 0);
            Gizmos.DrawLine(from, to);
        }
        // Horizontal lines
        for (int y = 0; y <= h; y++)
        {
            var from = orig + new Vector3(0, y * size, 0);
            var to   = orig + new Vector3(w * size, y * size, 0);
            Gizmos.DrawLine(from, to);
        }

        // ── Room colour overlays ───────────────────────────────────────────

        if (showRoomColours && boardData.rooms != null)
        {
            foreach (var room in boardData.rooms)
            {
                Color rc = room.debugColour;
                rc.a = 0.25f;
                Gizmos.color = rc;

                foreach (var pos in room.interiorCells)
                {
                    Vector3 centre = orig + new Vector3(
                        pos.x * size + size * 0.5f,
                        pos.y * size + size * 0.5f, 0f);
                    Gizmos.DrawCube(centre, new Vector3(size * 0.95f, size * 0.95f, 0.01f));
                }

                // Door cells — orange circles
                Gizmos.color = new Color(1f, 0.5f, 0f, 0.8f);
                foreach (var pos in room.doorCells)
                {
                    Vector3 centre = orig + new Vector3(
                        pos.x * size + size * 0.5f,
                        pos.y * size + size * 0.5f, 0f);
                    Gizmos.DrawSphere(centre, size * 0.2f);
                }

                // Anchor — cyan star approximation (two crossed lines)
                Gizmos.color = Color.cyan;
                Vector3 anchor = orig + new Vector3(
                    room.tokenAnchor.x * size + size * 0.5f,
                    room.tokenAnchor.y * size + size * 0.5f, 0f);
                float r = size * 0.35f;
                Gizmos.DrawLine(anchor + Vector3.left * r, anchor + Vector3.right * r);
                Gizmos.DrawLine(anchor + Vector3.down * r, anchor + Vector3.up * r);
            }
        }

        // ── Blocked cells ──────────────────────────────────────────────────

        Gizmos.color = new Color(0.8f, 0f, 0f, 0.35f);
        foreach (var pos in boardData.blockedCells)
        {
            Vector3 centre = orig + new Vector3(
                pos.x * size + size * 0.5f,
                pos.y * size + size * 0.5f, 0f);
            Gizmos.DrawCube(centre, new Vector3(size * 0.95f, size * 0.95f, 0.01f));
        }

        // ── Start positions ────────────────────────────────────────────────

        Gizmos.color = Color.magenta;
        foreach (var start in boardData.startPositions)
        {
            Vector3 centre = orig + new Vector3(
                start.gridPosition.x * size + size * 0.5f,
                start.gridPosition.y * size + size * 0.5f, 0f);
            Gizmos.DrawSphere(centre, size * 0.3f);
        }
    }
#endif
}