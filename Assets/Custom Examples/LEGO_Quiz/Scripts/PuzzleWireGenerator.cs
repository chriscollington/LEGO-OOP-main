using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleWireGenerator : MonoBehaviour
{
    public static PuzzleWireGenerator Instance { get; private set; } // Singleton instance

    private PuzzleData puzzleData; // Reference to the puzzle data
    public GameObject boxPrefab;
    public GameObject startSlotPrefab;
    public GameObject endSlotPrefab;
    public GameObject wirePrefab;
    public Transform startSlotContainer;
    public Transform endSlotContainer;
    public Transform wireContainer;

    public List<Transform> generatedStartSlots = new List<Transform>();
    public List<Transform> generatedEndSlots = new List<Transform>();
    public List<BoxDragHandler> boxHandlers = new List<BoxDragHandler>();
    public List<UILineRenderer> wireRenderers = new List<UILineRenderer>();

    private PuzzleCollection puzzleCollection;  // Reference to the PuzzleCollection


    private void Awake()
    {
        Instance = this;
        puzzleCollection = FindObjectOfType<PuzzleCollection>(); // Find PuzzleCollection in the scene
    }

      private void Start()
    {
        if (puzzleCollection != null)
        {
            // Get a random PuzzleData from PuzzleCollection (or select based on some logic)
            int randomIndex = Random.Range(0, puzzleCollection.scriptableQuestions.Length);
            puzzleData = puzzleCollection.scriptableQuestions[randomIndex];  // Assign a random PuzzleData
            
            GeneratePuzzle();  // Generate puzzle once puzzleData is assigned
        }
        else
        {
            Debug.LogError("PuzzleCollection not found in the scene.");
        }
    }

    public void GeneratePuzzle()
    {
        // Validate inputs
        if (puzzleData == null || boxPrefab == null || startSlotPrefab == null || endSlotPrefab == null ||
            startSlotContainer == null || endSlotContainer == null || wirePrefab == null || wireContainer == null)
        {
            Debug.LogError("PuzzleWireGenerator: Missing required references. Check puzzleData, prefabs, and containers.");
            return;
        }

        // Clear existing children
        foreach (Transform child in startSlotContainer) Destroy(child.gameObject);
        foreach (Transform child in endSlotContainer) Destroy(child.gameObject);
        foreach (Transform child in wireContainer) Destroy(child.gameObject);

        generatedStartSlots.Clear();
        generatedEndSlots.Clear();
        boxHandlers.Clear();
        wireRenderers.Clear();

        // Create Start Slots, End Slots, Wires, and Boxes
        for (int i = 0; i < puzzleData.boxes.Length; i++)
        {
            var boxData = puzzleData.boxes[i];

            // Instantiate Start Slot
            var startSlot = Instantiate(startSlotPrefab, startSlotContainer);
            startSlot.name = $"StartSlot_{i}";
            generatedStartSlots.Add(startSlot.transform);

            // Set Start Slot color
            var startSlotImage = startSlot.GetComponent<Image>();
            if (startSlotImage != null) startSlotImage.color = boxData.boxColor;

            // Instantiate End Slot
            var endSlot = Instantiate(endSlotPrefab, endSlotContainer);
            endSlot.name = $"EndSlot_{i}";
            generatedEndSlots.Add(endSlot.transform);

            // Instantiate Wire in the wire container
            var wire = Instantiate(wirePrefab, wireContainer);
            if (wire.TryGetComponent(out UILineRenderer lineRenderer))
            {
                // Initialize with three points
                lineRenderer.points = new Vector2[3]
                {
                    Vector2.zero, // Start point (default)
                    Vector2.zero, // Middle point (default)
                    Vector2.zero  // End point (default)
                };

                // Set wire color
                lineRenderer.color = boxData.boxColor;

                wireRenderers.Add(lineRenderer);
            }

            // Instantiate Box as a child of Start Slot
            var box = Instantiate(boxPrefab, startSlot.transform);
            var dragChild = box.transform.Find("DragImage");

            if (dragChild != null)
            {
                var childImage = dragChild.transform.Find("Image")?.GetComponent<Image>();
                if (childImage != null && boxData.boxImage != null)
                    childImage.sprite = boxData.boxImage;

                var childColor = dragChild.transform.Find("Background")?.GetComponent<Image>();
                if (childColor != null)
                    childColor.color = boxData.boxColor;

                var childText = dragChild.GetComponentInChildren<Text>();
                if (childText != null)
                    childText.text = boxData.boxText;
            }

            // Assign the wire and index to the box's drag handler
            var dragHandler = box.GetComponent<BoxDragHandler>();
            if (dragHandler != null)
            {
                dragHandler.SetWire(wire.GetComponent<UILineRenderer>(), i);
                boxHandlers.Add(dragHandler);
            }
        }

        PuzzleManager.Instance.Initialize(puzzleData, generatedEndSlots.ToArray(), generatedStartSlots.ToArray());
    }

    public Vector2 GetCanvasPosition(RectTransform rectTransform)
    {
        var canvas = rectTransform.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("The RectTransform is not part of a Canvas.");
            return Vector2.zero;
        }

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 localPoint);

        return localPoint;
    }
    private void Update()
    {
        for (int i = 0; i < boxHandlers.Count; i++)
        {
            if (boxHandlers[i] != null && wireRenderers[i] != null)
            {
                // Get the world positions of the start slot and box
                Vector3 startWorldPos = generatedStartSlots[i].position;
                Vector3 boxWorldPos = boxHandlers[i].transform.position;

                // Convert world positions to local positions relative to the wire container
                Vector2 startLocalPos = wireContainer.InverseTransformPoint(startWorldPos);
                Vector2 boxLocalPos = wireContainer.InverseTransformPoint(boxWorldPos);

                // Always set the third point as transparent
                Vector2 thirdPointPos = Vector2.zero; // Keep it at (0, 0)
                bool thirdPointVisible = false;

                // Check if you want to use the third point at all
                if (thirdPointVisible)
                {
                    thirdPointPos = boxLocalPos;
                }

                // Update the wire's points
                wireRenderers[i].points = new Vector2[]
                {
                    startLocalPos, // Start point of the wire
                    boxLocalPos,   // Box position (second point)
                    startLocalPos  // Invisible third point (transparent)
                };
            }
        }
    }

    

}


