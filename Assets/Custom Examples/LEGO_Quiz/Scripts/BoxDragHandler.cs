

using UnityEngine;
using UnityEngine.EventSystems;

public class BoxDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private UILineRenderer lineRenderer;

     public Vector2 CurrentPosition { get; private set; } // To store the current position of the box
    private bool isDragging = false;
    private Vector2 dragOffset;

    private int boxIndex;

    //public static PuzzleWireGenerator Instance { get; private set; } // Singleton instance
        public static object Instance { get; internal set; }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


   private void Update()
    {
        // Handle the drag logic
        if (isDragging)
        {
            Vector2 mousePosition = Input.mousePosition;

            // Offset the mouse position to the initial drag position
            rectTransform.position = mousePosition - dragOffset;

            // Update the current position in world space (so we can track it globally)
            CurrentPosition = rectTransform.position;

        }
    }
    public void SetWire(UILineRenderer lineRenderer, int index)
    {
        this.lineRenderer = lineRenderer;
        this.boxIndex = index;
    }
  

    private void OnMouseDown()
    {
        // Start dragging when the mouse is pressed down on the box
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging when the mouse is released
        isDragging = false;
    }



    public void OnDrag(PointerEventData eventData)
    {

        // Dynamically update the wire's second point to follow the box's position
        if (lineRenderer != null)
        {
            Vector2 boxPosition = PuzzleWireGenerator.Instance.GetCanvasPosition(rectTransform);
            var points = lineRenderer.points;
            points[1] = boxPosition; // Set the wire's second point to follow the box
            lineRenderer.points = points; // Update the wire points
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Transform closestEndSlot = GetClosestEndSlot();
        if (closestEndSlot != null)
        {
            Vector2 endSlotPosition = PuzzleWireGenerator.Instance.GetCanvasPosition(closestEndSlot.GetComponent<RectTransform>());
            rectTransform.anchoredPosition = endSlotPosition;

            if (lineRenderer != null)
            {
                var points = lineRenderer.points;
                points[1] = endSlotPosition; // Snap wire's second point to the end slot
                lineRenderer.points = points;
            }
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition;
            if (lineRenderer != null)
            {
                var points = lineRenderer.points;
                points[1] = initialPosition; // Reset wire's second point
                lineRenderer.points = points;
            }
        }
    }

    private Transform GetClosestEndSlot()
    {
        Transform closestSlot = null;
        float minDistance = float.MaxValue;

        foreach (var endSlot in PuzzleWireGenerator.Instance.generatedEndSlots)
        {
            float distance = Vector2.Distance(rectTransform.anchoredPosition, 
                            PuzzleWireGenerator.Instance.GetCanvasPosition(endSlot.GetComponent<RectTransform>()));
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlot = endSlot;
            }
        }

        return closestSlot;
    }

     public void StartDrag(Vector2 initialPosition)
    {
        isDragging = true;
        dragOffset = (Vector2)Input.mousePosition - initialPosition;
    }

    public void StopDrag()
    {
        isDragging = false;
    }


}
