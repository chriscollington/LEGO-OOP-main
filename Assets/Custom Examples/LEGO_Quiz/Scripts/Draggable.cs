using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 originalPosition;

    private LineRenderer wire;   // LineRenderer for the wire
    public GameObject wirePrefab; // Reference to a prefab for creating wires

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(transform.root); // Temporarily unparent for dragging

        // Create a wire (LineRenderer)
        wire = Instantiate(wirePrefab).GetComponent<LineRenderer>();
        wire.positionCount = 2; // Start and end points
        wire.SetPosition(0, transform.position); // Set start position
        wire.SetPosition(1, transform.position); // Initially same as start
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // Follow the mouse

        // Update wire's end position to follow the mouse
        if (wire != null)
        {
            wire.SetPosition(1, transform.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter; // Get the object under the mouse

        if (target != null && target.CompareTag("Slot"))
        {
            // Snap to slot
            transform.SetParent(target.transform);
            transform.position = target.transform.position;

            // Update wire to connect box and slot
            wire.SetPosition(1, target.transform.position); // Connect to slot
        }
        else
        {
            // Return to the original position and parent
            transform.SetParent(originalParent);
            transform.position = originalPosition;

            // Destroy the wire
            if (wire != null)
            {
                Destroy(wire.gameObject);
            }
        }

        // Trigger solution check
        PuzzleManager.Instance.CheckSolution();
    }
}

