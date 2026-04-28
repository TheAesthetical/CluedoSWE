using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Debug tool that logs all UI elemetns hit by a mouse click
/// Created to debug Detective Sheet issues
/// Useful for finding UI raycast issues
/// </summary>
public class UIClickDebugger : MonoBehaviour
{
    /// <summary>
    /// Checks for mouse clicks and logs all UI raycast hits
    /// </summary>
    void Update()
    {
        //Only run when mouse button is clicked
        if (!Input.GetMouseButtonDown(0)) return;

        //Creates pointer data at mouse postion
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;

        //Performs UI raycast
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        //Logs reults
        Debug.Log("CLICK RAYCAST RESULTS:");
        foreach (RaycastResult r in results)
        {
            Debug.Log(r.gameObject.name + " / parent: " + r.gameObject.transform.parent?.name);
        }
    }
}