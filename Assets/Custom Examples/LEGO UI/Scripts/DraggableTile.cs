using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTile : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public string prefabName; // The name of the tile prefab
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent; // Store the original parent of the tile

    public GridManager gridManager; // Reference to GridManager

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Save the original parent
        transform.SetParent(canvas.transform); // Temporarily make the tile a child of the canvas
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // Follow the mouse position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform) // If still attached to canvas
        {
            transform.SetParent(originalParent); // Return to the original grid
        }

        gridManager.HandleTileDrop(this); // Attempt to drop into a valid target slot
    }
}
