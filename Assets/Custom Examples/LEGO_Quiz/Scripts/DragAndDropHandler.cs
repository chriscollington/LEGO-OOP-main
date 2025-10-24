using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        originalParent = rectTransform.parent;
        rectTransform.SetParent(canvas.transform); // Bring to top-level canvas while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject dropTarget = GetDropTarget(eventData);

        if (dropTarget != null && dropTarget.CompareTag("DropZone") && dropTarget.transform.childCount == 0)
        {
            rectTransform.SetParent(dropTarget.transform);
            rectTransform.position = dropTarget.transform.position;
        }
        else
        {
            rectTransform.SetParent(originalParent);
            rectTransform.position = originalPosition; // Reset position if not dropped on a valid target
        }

        PuzzleManager.Instance.CheckSolution(); // Check if the puzzle is solved
    }

    private GameObject GetDropTarget(PointerEventData eventData)
    {
        // Perform a raycast to detect the valid drop target
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.CompareTag("DropZone"))
            {
                return result.gameObject;
            }
        }
        return null;
    }
}
