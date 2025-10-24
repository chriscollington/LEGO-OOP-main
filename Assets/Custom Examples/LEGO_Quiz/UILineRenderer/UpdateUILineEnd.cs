using UnityEngine;

public class UpdateUILineEnd : MonoBehaviour
{
    public UILineRenderer lineRenderer; // Reference to the UILineRenderer script

    private RectTransform rectTransform; // The RectTransform of the attached GameObject

    private void Awake()
    {
        // Get the RectTransform of the GameObject this script is attached to
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("UpdateUILineEnd: No RectTransform found on this GameObject.");
        }
    }

    private void Update()
    {
        if (lineRenderer == null || rectTransform == null) return;

        // Get the current position of the attached RectTransform in local space
        Vector2 localPosition = rectTransform.anchoredPosition;

        // Update the last point in the UILineRenderer's points array
        var points = lineRenderer.points; // Assuming points is a Vector2[] in UILineRenderer
        if (points != null && points.Length > 0)
        {
            points[points.Length - 1] = localPosition; // Set the last point to the RectTransform's position
            lineRenderer.points = points; // Reassign to trigger any necessary updates
        }
    }
}
