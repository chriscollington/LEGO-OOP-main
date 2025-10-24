using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PuzzleGenerator : MonoBehaviour
{
    public PuzzleData puzzleData;             // Assign the ScriptableObject in the Inspector
    public GameObject boxPrefab;              // Prefab for the draggable box
    public GameObject slotPrefab;             // Prefab for the slot
    public Transform endSlotContainer;        // Renamed parent object for the slots
    public Transform startSlotContainer;      // Renamed parent object for the starting slots

    private List<Transform> generatedBoxes = new List<Transform>();
    private List<Transform> generatedSlotsA = new List<Transform>();
    private List<Transform> generatedSlotsB = new List<Transform>();

    private void Start()
    {
        GeneratePuzzle(); // Automatically call this when the scene starts
    }

    public void GeneratePuzzle()
    {
        // Debug: Validate inputs
        if (puzzleData == null || boxPrefab == null || slotPrefab == null || endSlotContainer == null || startSlotContainer == null)
        {
            // Debug.LogError("PuzzleGenerator: Missing required references. Check puzzleData, prefabs, and containers.");
            return;
        }

        // Clear existing children (if regenerating the puzzle)
        foreach (Transform child in endSlotContainer) Destroy(child.gameObject);
        foreach (Transform child in startSlotContainer) Destroy(child.gameObject);

        generatedBoxes.Clear();
        generatedSlotsA.Clear();
        generatedSlotsB.Clear();

        // Debug.Log("Generating puzzle...");

        // Create Slots and Boxes
        for (int i = 0; i < puzzleData.boxes.Length; i++)
        {
            // Debug.Log($"Instantiating Slot {i}...");

            // Instantiate the starting slot in the startSlotContainer
            var slot = Instantiate(slotPrefab, startSlotContainer);
            if (slot == null)
            {
                // Debug.LogError("Failed to instantiate slotPrefab.");
                return;
            }

            slot.name = $"Slot_{i}";
            generatedSlotsB.Add(slot.transform);

            // Instantiate the box as a child of the slot
            var box = Instantiate(boxPrefab, slot.transform);
            if (box == null)
            {
                // Debug.LogError("Failed to instantiate boxPrefab.");
                return;
            }

            var boxData = puzzleData.boxes[i];

            // Configure the box's data (image, color, text, etc.)
            Transform dragChild = box.transform.Find("DragImage");
            if (dragChild != null)
            {
                var childImage = dragChild.transform.Find("Image").GetComponent<Image>();
                if (childImage != null && boxData.boxImage != null)
                {
                    childImage.sprite = boxData.boxImage;
                }

                var childColor = dragChild.transform.Find("Background").GetComponent<Image>();
                if (childColor)
                {
                    childColor.color = boxData.boxColor;
                }

                var childText = dragChild.GetComponentInChildren<Text>();
                if (childText)
                {
                    childText.text = boxData.boxText;
                }
            }

            box.name = boxData.boxName;
            generatedBoxes.Add(box.transform);
        }

        // Create Slots in the End Slot Container
        for (int i = 0; i < puzzleData.boxes.Length; i++)
        {
            // Debug.Log($"Instantiating End Slot {i}...");

            var endSlot = Instantiate(slotPrefab, endSlotContainer); // Instantiate under the endSlotContainer
            if (endSlot == null)
            {
                // Debug.LogError("Failed to instantiate slotPrefab.");
                return;
            }

            endSlot.name = $"EndSlot_{i}";
            generatedSlotsA.Add(endSlot.transform);
        }

        // Pass the generated data to the PuzzleManager
        PuzzleManager.Instance.Initialize(puzzleData, generatedSlotsA.ToArray(), generatedBoxes.ToArray());
        // Debug.Log("Puzzle generation completed.");
    }
}
