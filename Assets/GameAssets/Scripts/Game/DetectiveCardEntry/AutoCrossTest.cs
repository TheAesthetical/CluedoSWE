using UnityEngine;

/// <summary>
/// Temporary test script to prove auto cross works end to end
/// Delete this script once the AI is wired up properly
/// Keys:
///   1  - Simulate "I was dealt these cards" (auto crosses 3 cards in column 0)
///   2  - Simulate "Player 1 showed me the Dagger" (auto-crosses Dagger in column 1)
///   3  - Simulate "Player 2 showed me the Library" (auto-crosses Library in column 2)
///   4  - Simulate manual cross (toggles Wrench in column 0 manually)
///   R  - Reset (rebuild a fresh sheet)
/// </summary>
public class AutoCrossTest : MonoBehaviour
{
    private DetectiveSheetBuilder builder;

    private void Start()
    {
        builder = GetComponent<DetectiveSheetBuilder>();
        if (builder == null)
        {
            Debug.LogError("AutoCrossTest: no DetectiveSheetBuilder on this GameObject.");
            return;
        }

        Debug.Log("AutoCrossTest ready. Press 1 / 2 / 3 / 4 / R to test.");
    }
    
    private void Update()
    {
        if (builder == null || builder.Sheet == null) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Simulate hand-dealt: I (column 0) was dealt these three cards
            Debug.Log("[Test] Simulating hand-dealt for player 0");
            builder.Sheet.MarkAuto("Miss Scarlett", 0);
            builder.Sheet.MarkAuto("Candlestick", 0);
            builder.Sheet.MarkAuto("Kitchen", 0);
            builder.RefreshAll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Simulate suggestion result: Player 1 showed me the Dagger
            Debug.Log("[Test] Simulating: player 1 showed me the Dagger");
            builder.Sheet.MarkAuto("Dagger", 1);
            builder.RefreshAll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Simulate suggestion result: Player 2 showed me the Library
            Debug.Log("[Test] Simulating: player 2 showed me the Library");
            builder.Sheet.MarkAuto("Library", 2);
            builder.RefreshAll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Simulate the AI making a manual deduction note
            Debug.Log("[Test] Simulating: manual cross on Wrench in column 0");
            DetectiveCardEntry wrench = builder.Sheet.GetEntryByName("Wrench");
            builder.Sheet.ToggleManualCrossOut(wrench, 0);
            builder.RefreshAll();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[Test] Resetting sheet");
            // Rebuild from scratch
            DetectiveSheet fresh = new DetectiveSheet(builder.Sheet.GetActivePlayerCount());
            builder.SetSheet(fresh);
            builder.Build();
        }

        // Continuous deduction check
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("[Test] --- Deduction check ---");
            Debug.Log("Miss Scarlett ruled out of envelope? " + builder.Sheet.IsRuledOutOfEnvelope("Miss Scarlett"));
            Debug.Log("Professor Plum ruled out of envelope? " + builder.Sheet.IsRuledOutOfEnvelope("Professor Plum"));
            Debug.Log("Dagger ruled out of envelope? " + builder.Sheet.IsRuledOutOfEnvelope("Dagger"));
            Debug.Log("Rope ruled out of envelope? " + builder.Sheet.IsRuledOutOfEnvelope("Rope"));
        }
    }
}
