using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array of tile prefabs
    public Transform sourceGrid;    // Source grid (for dragging)
    public Transform targetGrid;    // Target grid (for arranging)
    public GameObject[] correctArrangement; // Correct order as GameObject references
    public int gridSize = 4;         // Number of tiles
    public Sprite sourceGridBackground; // Background sprite for the source grid
    public Color sourceGridBackgroundColor = Color.white; // Background color

    private void Start()
    {
        SetupGridBackground(sourceGrid, sourceGridBackground, sourceGridBackgroundColor);
        SetupGridBackground(targetGrid, null, new Color(0.8f, 0.8f, 0.8f)); // Optional for TargetGrid
        InitializeGrids();
    }

    // Set up a grid's background with an image or color
    void SetupGridBackground(Transform grid, Sprite backgroundSprite, Color backgroundColor)
    {
        Image backgroundImage = grid.GetComponent<Image>();
        if (backgroundImage == null)
        {
            backgroundImage = grid.gameObject.AddComponent<Image>();
        }

        backgroundImage.sprite = backgroundSprite;
        backgroundImage.color = backgroundColor;
        backgroundImage.type = Image.Type.Sliced; // For better scaling, if using a sprite
    }

    // Initialize the grids with tiles and empty slots
    void InitializeGrids()
    {
        ClearGrid(sourceGrid);
        ClearGrid(targetGrid);

        // Add draggable tiles to the source grid
        for (int i = 0; i < gridSize; i++)
        {
            GameObject tile = Instantiate(tilePrefabs[i], sourceGrid);
            DraggableTile draggable = tile.GetComponent<DraggableTile>();
            draggable.prefabName = tilePrefabs[i].name; // Assign prefab name for comparison
            draggable.gridManager = this;
        }

        // Add empty slots to the target grid
        for (int i = 0; i < gridSize; i++)
        {
            GameObject emptySlot = new GameObject($"Slot {i}", typeof(RectTransform), typeof(Image));
            emptySlot.transform.SetParent(targetGrid);
            emptySlot.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f); // Light grey
            emptySlot.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        }
    }

    // Handle when a tile is dropped
    public void HandleTileDrop(DraggableTile tile)
    {
        // Find the closest empty slot
        int closestSlotIndex = GetClosestSlotIndex(tile.transform.position);

        if (closestSlotIndex != -1)
        {
            Transform slot = targetGrid.GetChild(closestSlotIndex);

            if (slot.childCount == 0) // If the slot is empty
            {
                tile.transform.SetParent(slot);
                tile.transform.localPosition = Vector3.zero; // Snap to slot center
                CheckForCompletion(); // Validate the puzzle
            }
        }
    }

    // Check if the current arrangement matches the correct answer
    void CheckForCompletion()
    {
        for (int i = 0; i < targetGrid.childCount; i++)
        {
            Transform slot = targetGrid.GetChild(i);

            if (slot.childCount == 0)
            {
                Debug.Log("Puzzle incomplete. Missing tiles.");
                return;
            }

            DraggableTile tile = slot.GetChild(0).GetComponent<DraggableTile>();
            if (tile.prefabName != correctArrangement[i].name) // Compare prefab name
            {
                Debug.Log($"Puzzle incomplete. Slot {i} expected {correctArrangement[i].name} but got {tile.prefabName}.");
                return;
            }
        }

        Debug.Log("Puzzle Completed!");
    }

    // Get the index of the closest empty slot
    int GetClosestSlotIndex(Vector3 tilePosition)
    {
        float minDistance = float.MaxValue;
        int closestSlotIndex = -1;

        for (int i = 0; i < targetGrid.childCount; i++)
        {
            Transform slot = targetGrid.GetChild(i);
            if (slot.childCount == 0) // Only consider empty slots
            {
                float distance = Vector3.Distance(tilePosition, slot.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSlotIndex = i;
                }
            }
        }

        return closestSlotIndex;
    }

    // Clear all children from a grid
    void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}
