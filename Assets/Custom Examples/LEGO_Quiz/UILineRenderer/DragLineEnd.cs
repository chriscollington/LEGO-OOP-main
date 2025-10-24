using UnityEngine;
using UnityEngine.EventSystems;

public class DragLineEnd : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private LineRenderer lineRenderer;    // Reference to the LineRenderer component
    private Camera mainCamera;           // Reference to the main camera for converting mouse position
    private int draggingPointIndex = -1; // Index of the point being dragged (0 = start, 1 = end)

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        if (lineRenderer == null)
        {
            Debug.LogError("DragLineEnd: No LineRenderer component found on this GameObject.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (lineRenderer == null) return;

        // Convert the mouse position to world space
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ensure we work in 2D space

        // Find which point (start or end) is closer to the mouse
        Vector3 startPos = lineRenderer.GetPosition(0);
        Vector3 endPos = lineRenderer.GetPosition(1);

        // Determine which point is closer to the mouse
        if (Vector3.Distance(mouseWorldPos, startPos) < Vector3.Distance(mouseWorldPos, endPos))
        {
            draggingPointIndex = 0; // Start point
        }
        else
        {
            draggingPointIndex = 1; // End point
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (lineRenderer == null || draggingPointIndex == -1) return;

        // Update the position of the dragged point to follow the mouse
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ensure we work in 2D space

        lineRenderer.SetPosition(draggingPointIndex, mouseWorldPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset dragging index
        draggingPointIndex = -1;
    }
}
